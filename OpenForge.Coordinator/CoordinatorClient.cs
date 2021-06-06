// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using OpenForge.Coordinator.Packets;
using OpenForge.Server;
using OpenForge.Server.Database.Memory;
using OpenForge.Server.Extensions;
using OpenForge.Server.Messages;

namespace OpenForge.Coordinator
{
    /// <summary>
    /// Client for the Coordinator server, both used as client and server client
    /// </summary>
    public class CoordinatorClient : IDisposable, IPlayerCoordinator
    {
        private readonly CancellationToken _cancel = CancellationToken.None;
        private readonly TcpClient _client = null;
        private readonly Thread _clientThread = null;
        private readonly CoordinatorHandlers _handler = null;
        private readonly Dictionary<int, Action<CoordinatorPacket>> _respHandlers = new Dictionary<int, Action<CoordinatorPacket>>();
        private readonly object _writerLock = new object();
        private int _requestIdCounter = 1;
        private Stream _stream = null;
        private MessageWriter _writer = null;
        public PlayerDescriptor Descriptor { get; set; }
        private static Logger Logger { get; } = LogManager.GetCurrentClassLogger();

        public event Action<PlayerDescriptor> OnDisconnected;
        public event Action<PlayerDescriptor> OnRegistered;


        public CoordinatorClient(TcpClient client, CancellationToken cancel)
        {
            _handler = new CoordinatorHandlers(this);
            Descriptor = new PlayerDescriptor()
            {
                Address = ((IPEndPoint)client?.Client?.RemoteEndPoint)?.Address?.ToString()
            };
            if (Descriptor.Address.Contains(":"))
            {
                Descriptor.Address = Descriptor.Address[(Descriptor.Address.LastIndexOf(":") + 1)..];
            }

            _client = client;
            _cancel = cancel;
            _clientThread = new Thread(MainLoop);
            _clientThread.Start();

            //Give the thread a second to boot, probably restructure creating reader/writer in constructor.
            Thread.Sleep(100);
        }

        private async void MainLoop()
        {
            using (_stream = _client.GetStream())
            using (var reader = new MessageReader(_stream))
            using (_writer = new MessageWriter(_stream))
            {
                try
                {
                    while (_client.Connected)
                    {
                        try
                        {
                            await HandlePacket(reader);
                        }
                        catch (Exception ex)
                        {
                            Logger.Error(ex, $"CoordinationClient [{Descriptor.Address}] Disconnected");
                            break;
                        }
                        Thread.Sleep(1);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Exception in MainLoop");
                }
                finally
                {
                    _client.Dispose();
                    OnDisconnected?.Invoke(Descriptor);
                }
            }
        }


        //Requests
        #region Requests
        public void PushPlayerData(Player player)
        {
            try
            {
                Send(new PushPlayerDataPacket()
                {
                    Player = player
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to push player data.");
            }
        }

        public async Task<GetPlayerDataResponse> GetPlayerData()
        {
            return await Request<GetPlayerDataResponse>(new GetPlayerDataRequest());
        }

        public async Task<GetPlayersResponse> GetPlayers()
        {
            return await Request<GetPlayersResponse>(new GetPlayersRequest());
        }
        public async Task<RegisterPlayerResponse> Register(Player player)
        {
            return await Request<RegisterPlayerResponse>(new RegisterPlayerRequest()
            {
                Player = player
            });
        }
        #endregion

        #region Socket
        public void Request(CoordinatorPacket packet, Action<CoordinatorPacket> resp)
        {
            var header = new CoordinatorPacketHeader()
            {
                PacketID = packet.GetPacketID(),
                RequestID = _requestIdCounter++
            };
            lock (_respHandlers)
            {
                _respHandlers.Add(header.RequestID, resp);
            }
            Send(header, packet);
        }

        public async Task<CoordinatorPacket> Request(CoordinatorPacket packet, CancellationToken cancel)
        {
            var resultSource = new TaskCompletionSource<CoordinatorPacket>(cancel);

            Request(packet, (cbdata) =>
            {
                resultSource.SetResult(cbdata);
            });

            return await resultSource.Task;
        }

        public async Task<CoordinatorPacket> Request(CoordinatorPacket packet)
            => await Request(packet, CancellationToken.None);

        public async Task<T> Request<T>(CoordinatorPacket packet) where T : CoordinatorPacket
            => (T)await Request(packet, CancellationToken.None);

        public async Task<T> Request<T>(CoordinatorPacket packet, CancellationToken cancel) where T : CoordinatorPacket
            => (T)await Request(packet, cancel);

        public void Send(CoordinatorPacket packet, int responseID = -1)
        {
            var header = new CoordinatorPacketHeader()
            {
                PacketID = packet.GetPacketID(),
                RequestID = _requestIdCounter++,
                ResponseID = responseID
            };
            Send(header, packet);
        }

        private void Send(CoordinatorPacketHeader header, CoordinatorPacket packet)
        {
            lock (_writerLock)
            {
                _writer.Serialize(header);
                _writer.Serialize(packet);
                _writer.Flush();
            }
        }
        #endregion

        #region Packet Handling

        private async Task HandlePacket(MessageReader reader)
        {
            var header = reader.Deserialize<CoordinatorPacketHeader>();

            var packetType = CoordinatorPacket.GetPacketType(header.PacketID);

            Logger.Trace(() => $"Packet [{header.PacketID}]: {packetType.Name}");

            var packet = (CoordinatorPacket)reader.Deserialize(packetType);

            if (header.ResponseID > 0)
            {
                HandleResponse(header.ResponseID, packet);
                return;
            }

            var result = await _handler.HandlePacket(packet);

            if (result != null)
            {
                Send(result, header.RequestID);
            }
        }

        private void HandleResponse(int respId, CoordinatorPacket packet)
        {
            Action<CoordinatorPacket> act = null;
            lock (_respHandlers)
            {
                if (_respHandlers.ContainsKey(respId))
                {
                    act = _respHandlers[respId];
                }
            }
            act?.Invoke(packet);
        }
        #endregion

        public void Disconnect()
        {
            _client.Dispose();
        }

        public void Dispose()
        {
            Disconnect();
        }

        public static async Task<CoordinatorClient> Connect(string address, int port, CancellationToken token)
        {
            var client = new TcpClient();
            await client.ConnectAsync(address, port, token);
            return new CoordinatorClient(client, CancellationToken.None);
        }

        public class CoordinatorHandlers
        {
            //Dictionary<ParameterType[0].ID, public instance Method>
            private static readonly Dictionary<ushort, MethodInfo> s_handlers = typeof(CoordinatorHandlers)
                .GetMethods()
                .Where(x => (typeof(CoordinatorPacket).IsAssignableFrom(x.ReturnType) || x.ReturnType == typeof(void)) &&
                    x.GetParameters().Length == 1 && typeof(CoordinatorPacket).IsAssignableFrom(x.GetParameters()[0].ParameterType))
                .ToDictionary(x => CoordinatorPacket.GetPacketID(x.GetParameters()[0].ParameterType), y => y);

            private readonly CoordinatorClient _client = null;

            public CoordinatorHandlers(CoordinatorClient client)
            {
                _client = client;
            }

            public GetPlayerDataResponse GetPlayerData(GetPlayerDataRequest req)
            {
                return new GetPlayerDataResponse()
                {
                    Player = Player.GetPlayerByAddress(_client.Descriptor.Address)
                };
            }

            public GetPlayersResponse GetPlayers(GetPlayersRequest req)
            {
                return new GetPlayersResponse()
                {
                    Players = Player.GetOnline().Select(x => x.Name).ToList()
                };
            }

            public async Task<CoordinatorPacket> HandlePacket(CoordinatorPacket packet)
            {
                var id = CoordinatorPacket.GetPacketID(packet);
                if (!s_handlers.ContainsKey(id))
                {
                    throw new NotImplementedException($"Missing handler for PacketID {id}");
                }

                var result = s_handlers[id].Invoke(this, new object[] { packet });
                return result is Task<CoordinatorPacket> task ? await task : (CoordinatorPacket)result;
            }

            //Handlers

            public void ReceivePushPlayerData(PushPlayerDataPacket data)
            {
                var player = Player.GetLocalAccount();
                data.Player.Address = Player.LocalAddress;
                data.Player.ReIndex();
                player.ReplaceWith(data.Player);

                Logger.Info("Received player object from remote server");
            }

            public RegisterPlayerResponse RegisterPlayer(RegisterPlayerRequest req)
            {
                _client.Descriptor.Player = req.Player;
                _client.OnRegistered?.Invoke(_client.Descriptor);

                return new RegisterPlayerResponse()
                {
                    Success = true
                };
            }
        }
    }
}
