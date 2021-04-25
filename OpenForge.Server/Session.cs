// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using NLog;
using OpenForge.Server.Database.Memory;
using OpenForge.Server.Enumerations;
using OpenForge.Server.Extensions;
using OpenForge.Server.Messages;
using OpenForge.Server.PacketHandlers;

namespace OpenForge.Server
{
    public partial class Session : IDisposable
    {
        private readonly SslStream _sslStream;
        private readonly TcpClient _tcpClient;
        private byte[] _buffer = new byte[2048];
        private int _bytesReceived = 0;
        private bool _isDisposed = false;

        public Session(TcpClient tcpClient)
        {
            _tcpClient = tcpClient;
            _sslStream = new SslStream(_tcpClient.GetStream());

            var cert = new X509Certificate2("my.pfx", "1234", X509KeyStorageFlags.MachineKeySet);
            _sslStream.BeginAuthenticateAsServer(cert, false, false, OnAuthenticateAsServerCallback, null);
        }

        public static Logger Logger { get; } = LogManager.GetCurrentClassLogger();
        public static Logger ReceivePacketLogger { get; } = LogManager.GetLogger($"{nameof(Session)}.ReceivePacket");
        public static Logger SendPacketLogger { get; } = LogManager.GetLogger($"{nameof(Session)}.SendPacket");
        public string Address => ((IPEndPoint)_tcpClient?.Client?.RemoteEndPoint)?.Address?.ToString();
        public Player Player { get; set; }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;

            Player?.Logout();

            if (Player != null)
            {
                Player.PlayerLeftChannel(Player);
            }

            var group = Player?.GetActiveGroup();
            if (group != null)
            {
                if (group.OngoingMatch != null)
                {
                    //TODO: Send player leave match
                }

                group.RemoveMember(Player);
            }

            _tcpClient.Dispose();

            Logger.Info("Session disposed.");

            Player.GetActiveGroup()?.OngoingMatch?.StopIfDisconnected();
        }

        public void Send(byte[] response, int size)
        {
            lock (_sslStream)
            {
                _sslStream.Write(BitConverter.GetBytes(size), 0, 4);

                var h = (InterfaceType)BitConverter.ToInt32(response, 4);
                var t = BitConverter.ToInt32(response, 8);

                SendPacketLogger.Trace(() => $"Sent packet {h}:{MessageIdToString(h, t)}.");

                _sslStream.Write(response, 0, size);
            }
        }

        public void Send(Action<MessageWriter> writerAction)
        {
            using var responseStream = new MemoryStream();
            using var writer = new MessageWriter(responseStream);
            writerAction(writer);

            var size = responseStream.Position;
            Send(responseStream.ToArray(), (int)size);
        }

        public virtual void Send(object value)
        {
            try
            {
                Send(w =>
                {
                    MessageSerializer.Serialize(w, value);
                });
            }
            catch (Exception e)
            {
                Logger.Error(e, "Failed to send packet.");
                Dispose();
            }
        }

        private void BeginReceive()
        {
            _sslStream.BeginRead(_buffer, _bytesReceived, _buffer.Length, OnReceive, null);
        }

        private void HandleNextPacket(byte[] data, int offset, int length)
        {
            try
            {
                using var reader = new MessageReader(new MemoryStream(data, offset, length));
                var head = reader.ReadHeader();
                ReceivePacketLogger.Trace(() => $"Received packet {head.Interface}:{MessageIdToString(head.Interface, head.MessageId)}.");
                HandlerMap.HandlePacket(this, head, reader);
            }
            catch (Exception e)
            {
                Logger.Error(e, "Failed to handle packet.");
                Dispose();
            }
        }

        private string MessageIdToString(InterfaceType interfaceType, int messageId)
        {
            return interfaceType switch
            {
                InterfaceType.Borderline => ((BorderlineMessageType)messageId).ToString(),
                InterfaceType.Chat => ((ChatMessageType)messageId).ToString(),
                InterfaceType.Game => ((GameMessageType)messageId).ToString(),
                InterfaceType.Matchmaking => ((MatchmakingMessageType)messageId).ToString(),
                InterfaceType.Observer => ((ObserverMessageType)messageId).ToString(),
                InterfaceType.PreGame => ((PreGameMessageType)messageId).ToString(),
                InterfaceType.Shop => ((ShopMessageType)messageId).ToString(),
                InterfaceType.World => ((WorldMessageType)messageId).ToString(),
                _ => messageId.ToString(),
            };
        }

        private void OnAuthenticateAsServerCallback(IAsyncResult result)
        {
            try
            {
                _sslStream.EndAuthenticateAsServer(result);
                BeginReceive();
            }
            catch (Exception e)
            {
                Logger.Error(e, "Failed to authenticate or begin receiving data.");
                Dispose();
            }
        }

        private void OnReceive(IAsyncResult result)
        {
            try
            {
                var bytesReceived = _sslStream.EndRead(result);
                if (bytesReceived == 0)
                {
                    Dispose();
                    return;
                }

                _bytesReceived += bytesReceived;

                if (_bytesReceived >= sizeof(int))
                {
                    var dataLength = BitConverter.ToInt32(_buffer, 0);
                    var totalPacketLength = dataLength + sizeof(int);

                    if (_buffer.Length < totalPacketLength)
                    {
                        Array.Resize(ref _buffer, totalPacketLength);
                    }
                    else if (_bytesReceived >= totalPacketLength)
                    {
                        HandleNextPacket(_buffer, sizeof(int), dataLength);

                        _bytesReceived -= totalPacketLength;
                        if (_bytesReceived > 0)
                        {
                            Array.Copy(_buffer, totalPacketLength, _buffer, 0, _bytesReceived);
                        }
                    }
                }

                BeginReceive();
            }
            catch (Exception e)
            {
                Logger.Error(e, "Failed to receive data.");
                Dispose();
            }
        }
    }
}
