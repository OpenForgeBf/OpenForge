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
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using NLog;
using OpenForge.Coordinator;
using OpenForge.Server.Database.Memory;

namespace OpenForge.Launcher.Windows
{
    public class Launcher : Window
    {
        private readonly static DirectProperty<Launcher, string> BattleForgePathProperty = AvaloniaProperty.RegisterDirect<Launcher, string>(nameof(BattleForgePath), (x) => x.BattleForgePath, (x, v) => x.BattleForgePath = v);
        
        //TODO: Temporary, use a better way than this
        public string[] Resolutions { get; } = new string[]
        {
            "800x600",
            "1280x800",
            "1280x1024",
            "1366x768",
            "1440x900",
            "1680x1050",
            "1920x1080",
            "1920x1200",
            "2560x1440",
            "3840x2160",
            "7680x4320"
        };

        private readonly CoordinatorServer _coordServer = new CoordinatorServer(7400);
        private readonly Thread _coordThread = null;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly BattleForgeManager _manager = new BattleForgeManager();
        private readonly Thread _serverThread = null;
        private CoordinatorClient _coordClient = null;
        private Thread _reconnectionThread = null;
        private CancellationTokenSource _reconnectionCancel = null;

        //Game Interfaces
        private StackPanel _interfaceChoice = null;
        private StackPanel _interfaceHost = null;

        private StackPanel _interfaceHosting = null;
        private StackPanel _interfaceJoin = null;
        private StackPanel _interfaceJoined = null;
        private StackPanel _interfaceReconnecting = null;

        private IBrush _menuBrushActive = null;
        private IBrush _menuBrushInactive = null;

        private Button _buttonPlay = null;
        private Button _buttonJoin = null;
        private Button _buttonLeave = null;

        private TextBox _inputName = null;
        private TextBox _inputJoinAddress = null;

        private bool _started = false;
        private Timer _battleforgeSearchTimer;

        public bool DoStartClient { get; set; } = true;
        public bool DoStartServer { get; set; } = true;

        public string IngameName { get; set; } = "";
        public string InputJoinAddress { get; set; }

        public bool IsReconnecting { get; set; }
        public string ReconnectStatus { get; set; } = "Reconnecting...";

        public BattleForgeSettings Settings { get; set; } = new BattleForgeSettings();


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

        public string Version => typeof(Launcher).Assembly.GetName().Version.ToString();

        public string BattleForgePath { get; set; }

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

        /// <summary>
        /// Used by designer, Should only do visual setup, no server/database/file/persistent stuff
        /// </summary>
        public Launcher()
        {
            DataContext = this;
            try
            {
                Settings.LoadValuesFromConfig(_manager.GetConfig());
            }
            catch
            {
                //Failed to load settings?
            }
            InitializeComponent();
            CanResize = false;

        }

        /// <summary>
        /// Used when launching the application normally/debug
        /// </summary>
        public Launcher(bool startServers)
        {
            DataContext = this;
            BattleForgePath = string.IsNullOrEmpty(LauncherSettings.Instance.BattleForgePath) ? "BattleForge.exe" : LauncherSettings.Instance.BattleForgePath;
            try
            {
                Settings.LoadValuesFromConfig(_manager.GetConfig());
            }
            catch
            {
                //Failed to load settings
            }

            ExtendClientAreaToDecorationsHint = true;
            ExtendClientAreaChromeHints = Avalonia.Platform.ExtendClientAreaChromeHints.NoChrome;
            ExtendClientAreaTitleBarHeightHint = -1;

            InitializeComponent();


            //Only do it in this constructor, otherwise Designer breaks.
            IngameName = Player.GetLocalAccount()?.Name ?? "";

            //Has to be true to allow dragging
            CanResize = true;

            //For whatever reason CanResize = true causes the view to have margins.
            MinHeight = 530;
            MaxHeight = 530;
            MinWidth = 985;
            MaxWidth = 985;

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
                _coordServer.OnPlayerJoin += (client, player) =>
                {
                    _logger.Info($"Played joined {player.Name} from address {player.Address}.");

                    var existing = Player.GetPlayerByAddressOrCreate(player.Address, player.Player.GetPlayerData());

                    if (existing.LastSave < player.Player?.LastSave)
                    {
                        player.Player.Address = player.Address;
                        player.Player.ReIndex();
                        existing.ReplaceWith(player.Player);
                        _logger.Info($"Updated server player instance for {player.Address}");
                        existing = player.Player;
                        player.Player = existing;
                    }
                    else
                        _logger.Info($"Not updating player as the server instance is more up to date for {player.Address}");
                    existing.SetPlayerCoordinator(client);

                    JoinedPlayers.Add(player);
                    Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        ShowHosting();
                    });
                };
                _coordServer.OnPlayerLeave += (client, player) =>
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

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            this.MapMenu(new (string, string)[]
            {
                ("menuGame", "interfaceGame"),
                ("menuSettings", "interfaceSettings"),
                ("menuAbout", "interfaceAbout")
            }, 
            (btn) => btn.Background = _menuBrushActive,
            (btn) => btn.Background = _menuBrushInactive,
            (name, btn) =>
            {
                if (name == "menuGame")
                    _menuBrushActive = btn.Background;
                if (name == "menuSettings")
                    _menuBrushInactive = btn.Background;
            });

            //Settings
            this.MapMenu(new (string, string)[]
            {
                ("menuSettingsVideo", "interfaceSettingsVideo"),
                ("menuSettingsAudio", "interfaceSettingsAudio"),
                ("menuSettingsGame", "interfaceSettingsGame")
            });

            //Game
            _interfaceChoice = this.Find<StackPanel>("interfaceChoice");
            _interfaceJoin = this.Find<StackPanel>("interfaceJoin");
            _interfaceHost = this.Find<StackPanel>("interfaceHost");
            _interfaceHosting = this.Find<StackPanel>("interfaceHosting");
            _interfaceJoined = this.Find<StackPanel>("interfaceJoined");
            _interfaceReconnecting = this.Find<StackPanel>("interfaceReconnecting");
            _buttonPlay = this.Find<Button>("buttonPlay");

            _inputName = this.Find<TextBox>("inputName");

            _battleforgeSearchTimer = new Timer((_) =>
            {
                Started = Process.GetProcessesByName("Battleforge").Length > 0;
            }, null, 0, 1000);
        }


        #region GameInterface
        public async Task JoinServer()
        {
            var address = InputJoinAddress;
            if (_coordClient != null)
            {
                return;
            }

            _logger.Info($"Attempting to join server {address}..");

            try
            {
                _coordClient = await CoordinatorClient.Connect(address, _coordServer.Port, CancellationToken.None);

                var me = Player.GetLocalAccount();

                var resp = await _coordClient.Register(me);
                if (resp == null || !resp.Success)
                {
                    throw new Exception("Failed to register");
                }

                TargetServer = address;
                ShowJoined();

                _logger.Info($"Joined server {address}.");

                _coordClient.OnDisconnected += OnCoordinatorDisconnected;
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
            TargetServer = "127.0.0.1";
            ShowJoin();

            _logger.Info("Disconnected from server.");
        }
        public void ReconnectServer()
        {
            string address = InputJoinAddress;
            if (!IsReconnecting)
            {
                IsReconnecting = true;
                Dispatcher.UIThread.InvokeAsync(() => ShowReconnecting());
                _reconnectionThread = new Thread(async () =>
                {
                    ReconnectStatus = "Reconnecting...";
                    _reconnectionCancel = new CancellationTokenSource();
                    while (IsReconnecting)
                    {
                        try
                        {
                            ReconnectStatus = "Attempting to reconnect...";
                            CoordinatorClient client = await CoordinatorClient.Connect(address, _coordServer.Port, _reconnectionCancel.Token);

                            var me = Player.GetLocalAccount();

                            var resp = await client.Register(me);
                            if (resp == null || !resp.Success)
                                throw new Exception("Failed to register");

                            _coordClient = client;

                            TargetServer = address;
                            await Dispatcher.UIThread.InvokeAsync(() => ShowJoined());

                            _logger.Info($"Reconnected server {address}.");

                            _coordClient.OnDisconnected += OnCoordinatorDisconnected;
                            break;
                        }
                        catch (Exception ex)
                        {
                            _logger.Error(ex, $"Failed to reconnect server {address}.");
                            _coordClient?.Disconnect();
                            _coordClient = null;
                        }

                        ReconnectStatus = "Reconnecting in 5 seconds...";
                        Thread.Sleep(5000);
                    }
                    IsReconnecting = false;
                    _reconnectionCancel = null;
                });
                _reconnectionThread.Start();
            }
        }
        public void CancelReconnecting()
        {
            IsReconnecting = false;
            _reconnectionCancel?.Cancel();
            ShowJoin();
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

                string path = BattleForgePath;

                if(!File.Exists(path))
                {
                    _logger.Warn("BattleForge path does not exist, not launching");
                    return;
                }

                if(LauncherSettings.Instance.BattleForgePath != path)
                {
                    LauncherSettings.Instance.BattleForgePath = path;
                    LauncherSettings.Instance.Save();
                }

                try
                {
                    IPAddress address;
                    if (!IPAddress.TryParse(TargetServer, out address))
                    {
                        try
                        {
                            var hostEntry = Dns.GetHostEntry(TargetServer);
                            if (hostEntry.AddressList.Length == 0)
                            {
                                var message = $"Failed to parse IP or resolve host name '{TargetServer}'.";
                                _logger.Error(message);
                                throw new Exception(message);
                            }

                            address = hostEntry.AddressList[0];
                        }
                        catch (Exception e)
                        {
                            _logger.Error(e, $"Failed to parse IP or resolve host name '{TargetServer}'.");
                            throw;
                        }
                    }

                    Settings.ServerAddress = address.ToString();
                    Settings.ServerPort = TargetPort;
                    _manager.StartClient(Settings, path);
                    _logger.Info($"Started client on IP {TargetServer}, port {TargetPort}, path {path}.");
                }
                catch (Exception e)
                {
                    _logger.Error(e, "Failed to start client.");
                }
            }
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

                string old = BattleForgePath;
                BattleForgePath = results[0];
                RaisePropertyChanged(BattleForgePathProperty, old, BattleForgePath);
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
            _interfaceJoin.IsVisible = false;
            _interfaceHosting.IsVisible = true;
            _interfaceJoined.IsVisible = false;
            _interfaceReconnecting.IsVisible = false;
            UpdateInputNameEnabledState();
        }
        public void ShowJoin()
        {
            _interfaceJoin.IsVisible = true;
            _interfaceHosting.IsVisible = false;
            _interfaceJoined.IsVisible = false;
            _interfaceReconnecting.IsVisible = false;
            UpdateInputNameEnabledState();
        }
        public void ShowJoined()
        {
            _interfaceJoin.IsVisible = false;
            _interfaceHosting.IsVisible = false;
            _interfaceJoined.IsVisible = true;
            _interfaceReconnecting.IsVisible = false;
            UpdateInputNameEnabledState();
        }
        public void ShowReconnecting()
        {
            _interfaceJoin.IsVisible = false;
            _interfaceHosting.IsVisible = false;
            _interfaceJoined.IsVisible = false;
            _interfaceReconnecting.IsVisible = true;
            UpdateInputNameEnabledState();
        }

        public void Patreon()
        {
            _logger.Info("Patreon clicked.");

            Process.Start(new ProcessStartInfo("https://www.patreon.com/openforge")
            {
                UseShellExecute = true
            });
        }
        public void Github()
        {
            _logger.Info("Github clicked.");

            Process.Start(new ProcessStartInfo("https://github.com/OpenForgeBf/OpenForge")
            {
                UseShellExecute = true
            });
        }

        private void UpdateInputNameEnabledState() => _inputName.IsEnabled = !Started && _interfaceJoin.IsVisible;
        #endregion

        #region Event Handlers
        public void OnCoordinatorDisconnected(PlayerDescriptor descriptor)
        {
            if(_coordClient != null)
            {
                _coordClient.OnDisconnected -= OnCoordinatorDisconnected;
                _coordClient = null;
                ReconnectServer();
            }
        }
        #endregion


        public void Close()
        {
            base.Close();
        }
        public void Minimize()
        {
            base.WindowState = WindowState.Minimized;
        }
    }
}
