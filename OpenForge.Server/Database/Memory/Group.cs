// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketStructures;
using OpenForge.Server.PacketStructures.Matchmaking;
using OpenForge.Server.PacketStructures.World;

namespace OpenForge.Server.Database.Memory
{
    /// <summary>
    ///
    /// </summary>
    public class Group : DBObject<Group>
    {
        private static readonly IndexManager _index = new IndexManager(() => Select(x => x.ID));

        public ulong ID { get; set; }
        public Player Leader { get; set; }
        public GameLobby Lobby { get; set; }
        public override bool MemoryOnly => true;
        public Match OngoingMatch { get; set; }
        public List<Player> Members { get; set; } = new List<Player>();
        public List<Player> Players
        {
            get
            {
                lock (Members)
                {
                    return new Player[] { Leader }.Concat(Members).ToList();
                }
            }
        }

        public ChatChannel GroupChannel { get; init; }
        public ChatChannel MatchChannel { get; init; }
        public ChatChannel[] TeamChannels { get; set; }
        public ulong VersionID { get; set; } = 0;

        public bool HasMembers
        {
            get
            {
                lock (Members)
                {
                    return Members.Any();
                }
            }
        }

        public static Group Create(params Player[] players)
        {
            var group = new Group()
            {
                ID = _index.NewIndex(),
                VersionID = 0,
                Leader = players[0],
                Members = players.Skip(1).ToList(),
                GroupChannel = ChatChannel.Create(ChatChannelType.Group, players.ToArray()),
                MatchChannel = ChatChannel.Create(ChatChannelType.Group, players.ToArray()),
            };
            group.Update();
            return group;
        }

        public static Group GetGroup(ulong id)
        {
            return FirstOrDefault(x => x.ID == id);
        }

        public static Group GetOrCreate(Player leader)
        {
            var group = FirstOrDefault(x => x.Leader == leader);

            if (group == null)
            {
                group = Create(leader);
            }

            leader.SetActiveGroup(group);

            return group;
        }

        public void AddMember(Player player)
        {
            if (Players.Contains(player))
            {
                return;
            }

            Members.Add(player);
            VersionID++;
            NotifyGroupUpdate();
            player.SetActiveGroup(this);
        }

        public void ChangeGameSlot(Player player, int slot, ulong deckId)
        {
            if (Lobby == null)
            {
                return;
            }

            var deck = player.GetDeck(deckId);

            Lobby.ChangeSlot(player, deck, slot);
        }

        public void ChangeReadyStatus(Player player, bool ready)
        {
            if (Lobby == null)
            {
                return;
            }

            Lobby.ChangeReady(player, ready);
        }

        public void Disband()
        {
            Delete();
            VersionID++;

            foreach (var player in Players)
            {
                player.SetActiveGroup(null);
                player.LeftGroupNotify(ID);
            }

            if (TeamChannels != null)
            {
                foreach (var channel in TeamChannels)
                {
                    ChatChannel.Remove(channel.Id);
                }
            }

            ChatChannel.Remove(GroupChannel.Id);
            ChatChannel.Remove(MatchChannel.Id);
        }

        public int GetAvailableSlotIndex()
        {
            if (Lobby == null)
            {
                return -1;
            }

            var players = Lobby.GetPlayers().ToList();
            foreach (var player in players)
            {
                if (player.Player != null)
                {
                    continue;
                }

                return players.IndexOf(player);
            }

            return -1;
        }

        public int GetAvailableSlots()
        {
            return Lobby?.GetPlayers().Where(x => x.Player == null).Count() ?? 0;
        }

        public CNetCustomGameOverviewVO GetCustomGameOverview()
        {
            if (Lobby == null)
            {
                return null;
            }

            return new CNetCustomGameOverviewVO()
            {
                Id = Lobby.ID,
                CombinedChecksum = Lobby.CombinedChecksum,
                MapChecksum = Lobby.MapChecksum,
                CountOpenSlots = 1,
                Difficulty = Lobby.Difficulty,
                GroupLeader = Leader.GetWorldPlayer(),
                IdMap = Lobby.Map.ID,
                Limited = Lobby.Limited,
                MapNames = Lobby.MapNames,
                Speedrun = Lobby.Speedrun,
                RewardMode = Lobby.RewardMode,
                MapOfTheWeek = Lobby.MapOfTheWeek
            };
        }

        public CNetCustomGameCreatedNotification GetLobbyCreatedNotification()
        {
            return new CNetCustomGameCreatedNotification(true)
            {
                IdLeader = Leader.ID,
                IdPreMatch = Lobby.ID,
                IdMap = Lobby.Map.ID,
                Difficulty = Lobby.Difficulty,
                IsOpenCustomGame = true,
                Limited = Lobby.Limited,
                MapChecksum = Lobby.MapChecksum,
                CombinedChecksum = Lobby.CombinedChecksum,
                Pvp = Lobby.IsPVP
            };
        }

        public Player GetPlayerByID(ulong id)
        {
            if (Leader.ID == id)
            {
                return Leader;
            }

            return Players.FirstOrDefault(x => x.ID == id);
        }

        public CNetWorldGroupVO GetWorldGroup()
        {
            lock (Members)
            {
                return new CNetWorldGroupVO()
                {
                    Id = ID,
                    IdGroup = ID,
                    Leader = Leader.GetWorldPlayer(),
                    Players = Members.Select(x => x.GetWorldPlayer()).ToArray(),
                    VersionId = VersionID,
                    IdChatChannel = (int)GroupChannel.Id,
                    IdChatServer = 1
                };
            }
        }

        public void NotifyGroupUpdate()
        {
            Send(new CNetGroupListUpdatedNotification(true)
            {
                Group = GetWorldGroup()
            });
        }

        public void NotifyLobbyChanges()
        {
            if (Lobby == null)
            {
                return;
            }

            Send(new CNetCustomGameUpdatedNotification(true)
            {
                IdPreMatch = Lobby.ID,
                IdMap = Lobby.Map.ID,
                PvP = Lobby.IsPVP,
                Team1 = Lobby.Team1.Select(x => x.GetMatchPlayer()).ToArray(),
                Team2 = Lobby.Team2.Select(x => x.GetMatchPlayer()).ToArray(),
            });
        }

        public void NotifyLocation(Player player)
        {
            Send(new CNetGroupPlayerChangedLocationNotification(true)
            {
                Player = player.GetWorldPlayer()
            }, player);
        }

        public void RemoveMember(Player player)
        {
            lock (Members)
            {
                Members.RemoveAll(x => x == player);
            }

            VersionID++;
            player.SetActiveGroup(null);

            NotifyGroupUpdate();

            if (Lobby == null && (OngoingMatch == null || !OngoingMatch.Active) && (!HasMembers || player == Leader))
            {
                Disband();
                OngoingMatch?.StopGameLoop();
            }
            else if (Lobby != null)
            {
                Lobby.ChangeSlot(player, null, -1);
                NotifyLobbyChanges();
            }
        }

        public void Send(object obj, Player exclude = null)
        {
            Send(x => obj, exclude);
        }

        public void Send(Func<Player, object> resolve, Player exclude = null)
        {
            foreach (var player in Players)
            {
                if (player != exclude)
                {
                    player.Send(resolve(player));
                }
            }
        }

        public bool StartGame()
        {
            if (Lobby == null)
            {
                return false;
            }

            var seed = (short)new Random(Environment.TickCount).Next(short.MinValue, short.MaxValue + 1);

            OngoingMatch = new Match(Lobby.ID, this, Lobby, Players);

            if (TeamChannels != null)
            {
                foreach (var channel in TeamChannels)
                {
                    ChatChannel.Remove(channel.Id);
                }
            }

            TeamChannels = new ChatChannel[]
            {
                ChatChannel.Create(ChatChannelType.Team, Lobby.Team1.Where(t => t.Player != null).Select(t => t.Player).ToArray()),
                ChatChannel.Create(ChatChannelType.Team, Lobby.Team2.Where(t => t.Player != null).Select(t => t.Player).ToArray())
            };

            foreach (var player in Lobby.GetPlayers())
            {
                if (player.Player == null)
                {
                    continue;
                }

                player.Player.Send(new CNetCharacterForMatchCompleteNotification(true)
                {
                    IdMatch = Lobby.ID,
                    IdMap = Lobby.Map.ID,
                    IdGameServer = 1,
                    IdChatServer = 1,
                    IdGeneralChatChannel = (int)MatchChannel.Id,
                    IdTeamChatChannel = (int)TeamChannels[player.Team - 1].Id,
                    RandomSeed = seed,
                    Players = Lobby.GetPlayers().Select(x => x.GetPlayerObject()).ToArray(),
                    IdTeamStatistic = player.Team - 1,
                    IdTeamEloRating = 0,
                });
            }

            Lobby = null;

            return true;
        }

        public void StartLobby(CNetCreateCustomGameRMC descriptor)
        {
            Lobby = new GameLobby(descriptor.IdMap)
            {
                ID = ID,
                IsPVP = descriptor.Pvp,
                CombinedChecksum = descriptor.CombinedChecksum,
                MapChecksum = descriptor.MapChecksum,
                Difficulty = descriptor.Difficulty,
                Limited = descriptor.Limited,
                Speedrun = descriptor.Speedrun
            };

            Send(GetLobbyCreatedNotification());
        }

        public void StopLobby()
        {
            Lobby = null;
            Send(new CNetCustomGameDestroyedNotification(true)
            {
                IdPreMatch = ID
            });
        }
    }
}
