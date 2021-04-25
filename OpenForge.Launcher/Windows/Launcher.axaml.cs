// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using NLog;
using OpenForge.Coordinator;
using OpenForge.Server.Database.Memory;

namespace OpenForge.Launcher.Windows
{
    public class Launcher : Window
    {
        private readonly CoordinatorServer _coordServer = new CoordinatorServer(7400);
        private readonly Thread _coordThread = null;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly BattleForgeManager _manager = new BattleForgeManager();
        private readonly Thread _serverThread = null;
        private CoordinatorClient _coordClient = null;
        private StackPanel _interfaceChoice = null;
        private StackPanel _interfaceHost = null;
        private StackPanel _interfaceHosting = null;
        private StackPanel _interfaceJoin = null;
        private StackPanel _interfaceJoined = null;
        private Button _buttonPlay = null;
        private bool _started = false;
        private Timer _battleforgeSearchTimer;

        public Launcher()
        {
            DataContext = this;
            InitializeComponent();
            CanResize = false;
        }

        public Launcher(bool startServers)
        {
            DataContext = this;

            InitializeComponent();

            CanResize = false;

            if (startServers)
            {
                //GameServer
                _serverThread = new Thread(() =>
                {
                    try
                    {
                        _logger.Info($"Running game server..");
                        _manager.StartServer();
                        _logger.Info($"Game server stopped.");
                    }
                    catch (Exception e)
                    {
                        _logger.Error(e, $"A critical error occurred on the game server.");
                    }
                });
                _serverThread.Start();

                //CoordServer
                _coordServer.OnPlayerJoin += (player) =>
                {
                    _logger.Info($"Played joined {player.Name} from address {player.Address}.");

                    var existing = Player.GetPlayerByAddressOrCreate(player.Address);
                    player.Player.Address = player.Address;
                    player.Player.ReIndex();
                    existing.ReplaceWith(player.Player);

                    JoinedPlayers.Add(player);
                    Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        ShowHosting();
                    });
                };
                _coordServer.OnPlayerLeave += (player) =>
                {
                    _logger.Info($"Played left {player.Name} from address {player.Address}.");

                    JoinedPlayers.Remove(player);
                    if (JoinedPlayers.Count == 0)
                    {
                        Dispatcher.UIThread.InvokeAsync(() =>
                        {
                            ShowJoin();
                        });
                    }
                };
                _coordThread = new Thread(() =>
                {
                    try
                    {
                        _logger.Info($"Starting coordination server..");
                        _coordServer.Start();
                    }
                    catch (Exception e)
                    {
                        _logger.Error(e, $"A critical error occurred on the coordination server.");
                    }
                });
                _coordThread.Start();

                Closed += (a, b) =>
                {
                    _manager.StopServer();
                    _logger.Info($"Stopped game server.");
                    _coordServer.Stop();
                    _logger.Info($"Stopped coordination server.");
                    if (_serverThread != null)
                    {
                        _serverThread.Join();
                    }

                    if (_coordThread != null)
                    {
                        _coordThread.Join();
                    }
                };
            }
        }

        public bool Started
        {
            get => _started;
            set
            {
                var changed = _started != value;

                if (changed)
                {
                    _started = value;

                    Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        _buttonPlay.IsEnabled = !Started;
                    });
                }
            }
        }

        public bool DoStartClient { get; set; } = true;

        public bool DoStartServer { get; set; } = true;

        public string IngameName { get; set; } = Player.GetLocalAccount()?.Name ?? "Skylord";

        public string InputJoinAddress { get; set; }

        public ObservableCollection<PlayerDescriptor> JoinedPlayers { get; set; } = new ObservableCollection<PlayerDescriptor>()
        {
            /*
            new PlayerDescriptor()
            {
                Address = "123.23.57.34",
                Player = new Player()
                {
                    Name = "Whatever"
                }
            }*/
        };

        public int TargetPort { get; set; } = 7399;

        public string TargetServer { get; set; } = "127.0.0.1";

        private string Path
        {
            get => File.Exists("path.txt") ? File.ReadAllText("path.txt") : "Battleforge.exe";
            set => File.WriteAllText("path.txt", value);
        }

        public async Task JoinServer()
        {
            var address = InputJoinAddress;
            if (_coordClient != null)
            {
                return;
            }

            _logger.Error($"Attempting to join server {address}..");

            try
            {
                _coordClient = await CoordinatorClient.Connect(address, _coordServer.Port);

                var me = Player.GetLocalAccount();

                var resp = await _coordClient.Register(me);
                if (resp == null || !resp.Success)
                {
                    throw new Exception("Failed to register");
                }

                TargetServer = address;
                ShowJoined();

                _logger.Error($"Joined server {address}.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Failed to join server {address}.");
            }
        }

        public void LeaveServer()
        {
            _coordClient.Disconnect();
            _coordClient = null;
            ShowJoin();

            _logger.Info("Disconnected from server.");
        }

        public void Patreon()
        {
            _logger.Info("Patreon clicked.");

            Process.Start(new ProcessStartInfo("https://www.patreon.com/openforge")
            {
                UseShellExecute = true
            });
        }

        public void Play()
        {
            //TODO: Create message box on exceptions
            _logger.Info("Play clicked.");

            var name = IngameName;
            var player = Player.GetLocalAccount();
            player.Name = string.IsNullOrEmpty(name) ? "Skylord" : name;
            player.Update();

            _logger.Info($"Loaded player {player.Name}.");

            if (DoStartClient)
            {
                _logger.Info("Starting client..");

                var path = Path;

                try
                {
                    _manager.StartClient(new IPEndPoint(IPAddress.Parse(TargetServer), TargetPort), path);
                    _logger.Info($"Started client on IP {TargetServer}, port {TargetPort}, path {path}.");
                }
                catch (Exception e)
                {
                    _logger.Error(e, "Failed to start client.");
                }
            }

            /*
            if (!HasNotStarted)
            {
                return;
            }

            HasNotStarted = false;

            if (DoStartServer)
            {
                _serverThread = new Thread(() =>
                {
                    _manager.StartServer();
                });
                _serverThread.Start();
            }*/
        }

        public async void SetPath()
        {
            var dialog = new OpenFileDialog()
            {
                Title = "Select Battleforge.exe"
            };

            var results = await dialog.ShowAsync(this);

            if (results != null && results.Length > 0)
            {
                _logger.Info("BattleForge path selected: " + results[0]);
                Path = results[0];
            }
        }

        public void ShowChoice()
        {
            _interfaceChoice.IsVisible = true;
            _interfaceJoin.IsVisible = false;
            _interfaceHost.IsVisible = false;
            _interfaceHosting.IsVisible = false;
            _interfaceJoined.IsVisible = false;
        }

        public void ShowHost()
        {
            _interfaceChoice.IsVisible = false;
            _interfaceJoin.IsVisible = false;
            _interfaceHost.IsVisible = true;
            _interfaceHosting.IsVisible = false;
            _interfaceJoined.IsVisible = false;
        }

        public void ShowHosting()
        {
            _interfaceChoice.IsVisible = false;
            _interfaceJoin.IsVisible = false;
            _interfaceHost.IsVisible = false;
            _interfaceHosting.IsVisible = true;
            _interfaceJoined.IsVisible = false;
        }

        public void ShowJoin()
        {
            _interfaceChoice.IsVisible = false;
            _interfaceJoin.IsVisible = true;
            _interfaceHost.IsVisible = false;
            _interfaceHosting.IsVisible = false;
            _interfaceJoined.IsVisible = false;
        }

        public void ShowJoined()
        {
            _interfaceChoice.IsVisible = false;
            _interfaceJoin.IsVisible = false;
            _interfaceHost.IsVisible = false;
            _interfaceHosting.IsVisible = false;
            _interfaceJoined.IsVisible = true;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            _interfaceChoice = this.Find<StackPanel>("interfaceChoice");
            _interfaceJoin = this.Find<StackPanel>("interfaceJoin");
            _interfaceHost = this.Find<StackPanel>("interfaceHost");
            _interfaceHosting = this.Find<StackPanel>("interfaceHosting");
            _interfaceJoined = this.Find<StackPanel>("interfaceJoined");
            _buttonPlay = this.Find<Button>("buttonPlay");

            _battleforgeSearchTimer = new Timer((_) =>
            {
                Started = Process.GetProcessesByName("Battleforge").Length > 0;
            }, null, 0, 1000);
        }
    }
}
