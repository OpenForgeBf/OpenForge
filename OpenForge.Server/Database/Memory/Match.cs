// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using OpenForge.Server.Enumerations;
using OpenForge.Server.Messages;
using OpenForge.Server.PacketHandlers;
using OpenForge.Server.PacketStructures;
using OpenForge.Server.PacketStructures.Game;

namespace OpenForge.Server.Database.Memory
{
    public class Match
    {
        public long CurrentGroupIndex = 0;
        public int CurrentStepGenerator = 1;
        public int SequenceNumberGenerator = 1;
        public Mutex StepMutex = new Mutex();
        private static readonly IndexManager s_index = new IndexManager();

        public Match(ulong id, Group group, GameLobby lobby, List<Player> players)
        {
            ID = id;
            Group = group;
            Lobby = lobby;
            ToLoad = players;
        }

        public bool Active { get; set; } = false;
        public Group Group { get; set; }
        public ulong ID { get; set; } = s_index.NewIndex();
        public GameLobby Lobby { get; set; }
        public Stopwatch MatchTimer { get; } = new Stopwatch();
        public Dictionary<uint, uint> StepCrcs { get; } = new Dictionary<uint, uint>();
        public List<Player> ToLoad { get; set; } = new List<Player>();
        private static Logger Logger { get; } = LogManager.GetCurrentClassLogger();

        public bool CheckSynchronization(uint step, uint crc) => !StepCrcs.TryGetValue(step, out var existingCrc) || existingCrc == crc;

        public void FinishedLoading(Player player)
        {
            var start = false;
            lock (ToLoad)
            {
                ToLoad.Remove(player);
                if (ToLoad.Count == 0)
                {
                    start = true;
                }
            }
            if (start)
            {
                StartGameLoop();
            }
        }

        public void PlayerLeft(Player player)
        {
            var tp = Lobby.GetTeamPlayer(player);
            tp.Disconnected = true;
        }

        public void SendCommand(CommandType commandType, byte[] blob)
        {
            Group.Send(player => new CNetAnnounceCommandNotification(true)
            {
                Header = new CNetDataHeader()
                {
                    ClientIds = default,
                    Interface = InterfaceType.Game,
                    MessageId = (int)GameMessageType.CNetAnnounceCommandNotification,
                    Broadcast = false,
                    RemoteMethod = false,
                    DestinationServerId = 0,
                    SourceServerId = 0,
                    SequenceNumber = Lobby.GetTeamPlayer(player).SequenceNumberUpdate(),
                    RequestId = 0,
                    CharacterId = player.ID,
                    Channel = ID,
                },
                Blob = blob,
                GdCommand = commandType,
                Step = CurrentStepGenerator + 1
            });
        }

        public void StartGameLoop()
        {
            Active = true;
            foreach (var player in Lobby.GetPlayers())
            {
                if (player.Player == null)
                {
                    continue;
                }

                player.SequenceNumberReset();
                player.Player.Send(new CNetGameStartNotification(false)
                {
                    Header = new CNetDataHeader()
                    {
                        ClientIds = default,
                        Interface = InterfaceType.Game,
                        MessageId = (int)GameMessageType.CNetGameStartNotification,
                        Broadcast = false,
                        RemoteMethod = false,
                        DestinationServerId = 0,
                        SourceServerId = 0,
                        SequenceNumber = player.SequenceNumberUpdate(),
                        RequestId = 0,
                        CharacterId = player.Player.ID,
                        Channel = ID,
                    }
                });
            }

            new Thread(() =>
            {
                Logger.Info("Match thread prepared.");
                Thread.Sleep(5000);
                Logger.Info("Match thread started.");

                CurrentStepGenerator = 0;

                MatchTimer.Restart();

                try
                {
                    while (Active)
                    {
                        GameStep();
                    }

                    //Continue for 1 more second
                    var active = true;
                    //Make this actual thread to prevent suspension
                    Task.Delay(2000).ContinueWith((x) => active = false);

                    while (active)
                    {
                        GameStep();
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e, "Error in match thread loop.");
                }

                Logger.Info("Match thread stopped.");
            }).Start();
        }

        public void StopGameLoop()
        {
            Active = false;
        }

        public bool StopIfDisconnected()
        {
            if (!Lobby.GetPlayers().Any(player => player.Player != null && !player.Disconnected))
            {
                StopGameLoop();
                return true;
            }
            return false;
        }

        private void GameStep()
        {
            var timeUntilNextStep = (int)Math.Round(((CurrentStepGenerator + 1) * 100) - MatchTimer.Elapsed.TotalMilliseconds);
            if (timeUntilNextStep > 0)
                Thread.Sleep(timeUntilNextStep);

            StepMutex.WaitOne();

            try
            {
                var step = ++CurrentStepGenerator;

                Group.Send(player =>
                    new CNetExecuteToStepNotification(false)
                    {
                        Header = new CNetDataHeader()
                        {
                            ClientIds = default,
                            Interface = InterfaceType.Game,
                            MessageId = (int)GameMessageType.CNetExecuteToStepNotification,
                            Broadcast = false,
                            RemoteMethod = false,
                            DestinationServerId = 0,
                            SourceServerId = 0,
                            SequenceNumber = Lobby.GetTeamPlayer(player).SequenceNumberUpdate(),
                            RequestId = 0,
                            CharacterId = player.ID,
                            Channel = ID,
                        },
                        Step = step
                    });
            }
            finally
            {
                StepMutex.ReleaseMutex();
            }
        }
    }
}
