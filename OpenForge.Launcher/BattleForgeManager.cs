// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using NLog;
using OpenForge.Server;
using OpenForge.Server.PacketHandlers;

namespace OpenForge.Launcher
{
    public class BattleForgeManager
    {
        private ListenerSocket _socket = null;
        private static Logger Logger { get; } = LogManager.GetCurrentClassLogger();

        public void StartClient(IPEndPoint ipEndPoint, string path = null)
        {
            Logger.Info("Starting client..");
            //TODO: Capitalize BattleForge? (capital sensitivity in Linux etc)
            var configXmlPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "battleforge/config.xml");

            var configFile = new FileInfo(configXmlPath);

            if (configFile.Exists)
            {
                Logger.Info("Adjusted server URI in config file.");

                var configXml = File.ReadAllText(configXmlPath);

                var regex = new Regex("<network serveruri=\"([a-z0-9\\.:]+)\"");
                var match = regex.Match(configXml);

                if (!match.Success)
                {
                    Console.WriteLine("Failed to find value in configuration.");
                    return;
                }

                var prefix = configXml.Substring(0, match.Captures[0].Index);
                var offset = match.Captures[0].Index + match.Captures[0].Length;
                var suffix = configXml[offset..];

                configXml = $"{prefix}<network serveruri=\"{ipEndPoint.Address}:{ipEndPoint.Port}\"{suffix}";
                File.WriteAllText(configXmlPath, configXml);
            }
            else
            {
                Logger.Info("Created config file.");

                Directory.CreateDirectory(configFile.Directory.FullName);
                File.Copy("config.xml", configXmlPath);
            }

            var battleforgePath = path ?? "Battleforge.exe";
            var workingDirectory = path != null ? Path.GetDirectoryName(battleforgePath) : null;

            Logger.Info($"Starting '{battleforgePath}' in working directory '{workingDirectory}'..");

            var processStartInfo = new ProcessStartInfo
            {
                FileName = path,
                WorkingDirectory = string.IsNullOrEmpty(workingDirectory) ? Environment.CurrentDirectory : workingDirectory,
                Arguments = "-online"
            };

            var p = Process.Start(processStartInfo);
            p.WaitForInputIdle();
            Logger.Info($"Started '{battleforgePath }'.");

            var patches = new Dictionary<IntPtr, byte[]>
            {
                { new IntPtr(0x41EBB5), new byte[] { 0xEB, 0x6A } },
                { new IntPtr(0x1357260), new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 } }
            };

            try
            {
                var handle = Kernel32.OpenProcess(p, ProcessAccessFlags.All);

                foreach (var patch in patches)
                {
                    var oldProtect = Kernel32.VirtualProtectEx(handle, patch.Key, patch.Value.Length, 0x40);
                    Kernel32.WriteProcessMemory(handle, patch.Key, patch.Value);
                    Kernel32.VirtualProtectEx(handle, patch.Key, patch.Value.Length, oldProtect);
                }

                Logger.Info($"Patched.");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Failed to patch cert pinning '{battleforgePath }'.");
                p.CloseMainWindow();
            }

            Logger.Info("Starting client completed.");
        }

        public void StartServer()
        {
            Logger.Info("Starting server..");
            Logger.Info("Mapping handlers..");
            HandlerMap.MapHandlers(typeof(BorderlineHandlers));
            HandlerMap.MapHandlers(typeof(ChatHandlers));
            HandlerMap.MapHandlers(typeof(GameHandlers));
            HandlerMap.MapHandlers(typeof(MatchmakingHandlers));
            HandlerMap.MapHandlers(typeof(PreGameHandlers));
            HandlerMap.MapHandlers(typeof(ShopHandlers));
            HandlerMap.MapHandlers(typeof(WorldHandlers));
            Logger.Info("Mapped handlers.");

            _socket = new ListenerSocket();
            _socket.Run();
        }

        public void StopServer()
        {
            if (_socket != null)
            {
                _socket.Dispose();
            }

            _socket = null;
        }
    }
}
