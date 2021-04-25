// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using NLog;

namespace OpenForge.Server
{
    public class ListenerSocket : IDisposable
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly List<Session> _sessions = new List<Session>();
        private TcpListener _listener;

        public ListenerSocket()
        {
        }

        public void Dispose()
        {
            _listener.Stop();

            foreach (var session in _sessions)
            {
                session.Dispose();
            }

            _sessions.Clear();
        }

        public void Run()
        {
            _logger.Info("Starting listener..");

            _listener = new TcpListener(new IPEndPoint(IPAddress.Any, 7399));
            _listener.Start();

            _logger.Info("Listener started.");
            _logger.Info("Waiting for connections..");

            while (true)
            {
                var client = _listener.AcceptTcpClient();
                _logger.Info("New session.");

                _sessions.Add(new Session(client));
            }
        }
    }
}
