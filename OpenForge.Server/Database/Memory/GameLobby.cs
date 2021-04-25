// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenForge.Server.Maps;
using OpenForge.Server.PacketStructures;

namespace OpenForge.Server.Database.Memory
{
    public class GameLobby
    {
        public GameLobby(ulong map)
        {
            Map = MapInfo.GetMap(map);

            if (Map.Type != MapType.Pvp)
            {
                Team1 = new TeamPlayer[Map.Slots];
                Team2 = new TeamPlayer[0];
            }
            else
            {
                Team1 = new TeamPlayer[Map.Slots / 2];
                Team2 = new TeamPlayer[Map.Slots / 2];
            }
            for (var i = 0; i < Team1.Length; i++)
            {
                Team1[i] = new TeamPlayer(1);
            }

            for (var i = 0; i < Team2.Length; i++)
            {
                Team2[i] = new TeamPlayer(2);
            }
        }

        public long CombinedChecksum { get; set; }
        public int Difficulty { get; set; }
        public ulong ID { get; set; }
        public bool IsPVP { get; set; }
        public bool Limited { get; set; }
        public MapInfo Map { get; set; }
        public int MapChecksum { get; set; }
        public bool MapFileAvailableOnServer { get; set; } = false;
        public string[] MapNames { get; set; }
        public int MapOfTheWeek { get; set; }
        public int RewardMode { get; set; }
        public int Slots { get; set; }
        public bool Speedrun { get; set; }
        public TeamPlayer[] Team1 { get; set; }
        public TeamPlayer[] Team2 { get; set; }

        public void ChangeReady(Player player, bool isReady)
        {
            var tp = GetTeamPlayer(player);
            if (tp != null)
            {
                tp.IsReady = isReady;
            }
        }

        public void ChangeSlot(Player player, CNetDeckVO deck, int slot)
        {
            foreach (var tp in GetPlayers())
            {
                if (tp.Player != player)
                {
                    continue;
                }

                tp.Clear();
            }

            if (slot < Team1.Length)
            {
                Team1[slot].Set(player, deck);
            }
            else
            {
                Team2[slot - Team1.Length].Set(player, deck);
            }
        }

        public IEnumerable<TeamPlayer> GetPlayers()
        {
            return Team1.Concat(Team2);
        }

        public TeamPlayer GetTeamPlayer(Player player)
        {
            return Team1.FirstOrDefault(x => x.Player == player) ?? Team2.FirstOrDefault(x => x.Player == player);
        }
    }

    public class TeamPlayer
    {
        public int SequenceNumber = 0;

        public TeamPlayer(byte team)
        {
            Team = team;
        }

        public CNetDeckVO Deck { get; set; }
        public bool Disconnected { get; set; } = false;
        public bool IsReady { get; set; }
        public Player Player { get; set; }
        public byte Team { get; set; } = 1;

        public void Clear()
        {
            Player = null;
            Deck = null;
        }

        public CNetMatchPlayerVO GetMatchPlayer()
        {
            return new CNetMatchPlayerVO()
            {
                Id = Player?.ID ?? 0,
                PvELevel = 1,
                PvPLevel = 1,
                CharacterIndex = Player?.ID ?? 0,
                CoverCardID = Deck?.CoverCard.CardIndex ?? 0,
                DeckName = Deck?.DeckName ?? "",
                DeckLevel = Deck?.DeckLevel ?? 0,
                IsReady = IsReady,
                Name = Player?.Name
            };
        }

        public CNetPlayerVO GetPlayerObject()
        {
            if (Player != null)
            {
                return new CNetPlayerVO()
                {
                    Deck = Deck ?? new CNetDeckVO() { CoverCard = new CNetCardVO() },
                    Color = Team,
                    Index = Player.ID,
                    Name = Player.Name,
                    Team = Team,
                    WorldPlayer = Player.GetWorldPlayer() ?? new CNetWorldPlayerVO()
                };
            }

            return new CNetPlayerVO()
            {
                Deck = new CNetDeckVO() { CoverCard = new CNetCardVO() },
                WorldPlayer = new CNetWorldPlayerVO()
            };
        }

        public int SequenceNumberReset()
        {
            return Interlocked.Exchange(ref SequenceNumber, 0);
        }

        public int SequenceNumberUpdate()
        {
            return Interlocked.Increment(ref SequenceNumber);
        }

        public void Set(Player player, CNetDeckVO deck)
        {
            Player = player;
            Deck = deck;
        }
    }
}
