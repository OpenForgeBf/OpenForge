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
    /// Facilitates communication between different launchers to exchange player data (For joining friends)
    /// </summary>
    public class CoordinatorServer
    {
        private readonly CancellationTokenSource _cancel = new CancellationTokenSource();
        private readonly List<CoordinatorClient> _clients = new List<CoordinatorClient>();
        private TcpListener _listener = null;
        private Thread _listenThread = null;

        public CoordinatorServer(int port = 8000)
        {
            Port = port;
        }

        public event Action<CoordinatorClient, PlayerDescriptor> OnPlayerJoin;
        public event Action<CoordinatorClient, PlayerDescriptor> OnPlayerLeave;

        public bool Active { get; private set; }
        public int Port { get; private set; }
        private static Logger Logger { get; } = LogManager.GetCurrentClassLogger();

        public void Start()
        {
            if (Active)
            {
                return;
            }

            Active = true;

            _listener = new TcpListener(IPAddress.Any, Port);
            _listener.Start();

            Logger.Info("Started Coordinator Server");

            _listenThread = new Thread(async () =>
            {
                while (Active)
                {
                    var client = await _listener.AcceptTcpClientAsync();
                    var c = new CoordinatorClient(client, _cancel.Token);
                    Logger.Info($"CoordinationClient: {c.Descriptor.Address}");

                    c.OnRegistered += (descriptor) =>
                    {
                        Logger.Info($"Client [{descriptor.Address}] registered {descriptor.Name}");
                        OnPlayerJoin?.Invoke(c, descriptor);
                    };
                    c.OnDisconnected += (descriptor) =>
                    {
                        Logger.Info($"Client [{descriptor.Address} disconnected");
                        OnPlayerLeave?.Invoke(c, descriptor);
                    };

                    lock (_clients)
                    {
                        _clients.Add(c);
                    }
                }
            });
            _listenThread.Start();
        }

        public void Stop()
        {
            Active = false;
            _listener.Stop();
            _cancel.Cancel();
            _listenThread.Join();
        }
    }
}
