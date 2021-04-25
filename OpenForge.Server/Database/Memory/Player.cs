// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using NLog;
using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketStructures;
using OpenForge.Server.PacketStructures.Chat;
using OpenForge.Server.PacketStructures.World;

namespace OpenForge.Server.Database.Memory
{
    public class Player : DBObject<Player>
    {
        private static readonly IndexManager s_index = new IndexManager(() => GetMaxPlayerIndex());

        private IPlayerCoordinator _coordinator = null;
        private Group _group = null;
        private Session _session = null;

        public Player()
        {
            GroupID = 0;
        }

        public ulong ActiveDeck { get; set; } = 0;
        public string Address { get; set; }
        public long BannerID { get; set; }
        public int BattlePoints { get; set; }
        public int BFP { get; set; }
        public List<CNetCardVO> Cards { get; set; } = new List<CNetCardVO>();
        public List<CNetDeckVO> Decks { get; set; } = new List<CNetDeckVO>();
        public int Elo { get; set; }
        public int Experience { get; set; }
        public int Gold { get; set; }
        public ulong GroupID { get; set; } = 0;
        public int HonorPoints { get; set; }
        public ulong ID { get; set; }
        public bool Male { get; set; } = true;
        public string Name { get; set; }
        public int VictoryPoints { get; set; }

        [IgnoreDataMember]
        public bool HasCoordinator => _coordinator != null;

        [IgnoreDataMember]
        public bool IsLocal { get; set; }

        [IgnoreDataMember]
        public bool IsOnline => _session != null;

        [IgnoreDataMember]
        public LocationType Location { get; set; } = LocationType.Sandbox;

        [IgnoreDataMember]
        public ulong Map { get; set; } = 0;

        [IgnoreDataMember]
        public PlayerState State { get; set; } = PlayerState.Online;

        private static Logger Logger { get; } = LogManager.GetCurrentClassLogger();

        public static void Broadcast(object packet)
        {
            var ps = GetOnline();
            foreach (var player in ps)
            {
                try
                {
                    player._session?.Send(packet);
                }
                catch (Exception ex)
                {
                    Logger.Warn(ex, "Failed to broadcast to a player.");
                }
            }
        }

        public static Player CreatePlayer(Player player)
        {
            player.ID = s_index.NewIndex();
            player.Update();
            return player;
        }

        public static Player GetLocalAccount()
        {
            return GetPlayerByAddressOrCreate("127.0.0.1");
        }

        public static List<Player> GetOnline()
        {
            return Where(x =>
                x.IsOnline);
        }

        public static Player GetPlayerByAddress(string address)
        {
            return Access(db => db.FirstOrDefault(x => x.Address == address));
        }

        public static Player GetPlayerByAddressOrCreate(string address, PlayerData data)
            => GetPlayerByAddressOrCreate(address, () => data);

        public static Player GetPlayerByAddressOrCreate(string address, Func<PlayerData> creation = null)
        {
            var p = GetPlayerByAddress(address);

            if (p == null)
            {
                var data = creation != null ? creation() : PlayerData.CreateDefault();
                return CreatePlayer(new Player()
                {
                    Name = "Skylord",
                    Address = address,
                    Decks = data.Decks.ToList(),
                    Cards = data.Cards.ToList(),
                    ActiveDeck = data.Decks.First().Index
                });
            }
            else
            {
                return p;
            }
        }

        public static Player GetPlayerByID(ulong id)
        {
            return Access(db => db.FirstOrDefault(x => x.ID == id));
        }

        public static Player GetPlayerByName(string name)
        {
            return Access(db => db.FirstOrDefault(x => x.Name.ToLower() == name.ToLower()));
        }

        public static Player Login(Session session, Func<PlayerData> creation = null)
        {
            var address = session.Address;
            if (address == null)
            {
                return null;
            }

            var player = GetPlayerByAddressOrCreate(address, creation);

            player._session = session;
            session.Player = player;

            return player;
        }

        public static void PlayerJoinedChannel(Player player)
        {
            Broadcast(new CNetJoinChatRegionChannelNotification(true)
            {
                ChannelId = 1,
                Player = player.GetWorldPlayer(),
                ChatServerId = 1
            });
        }

        public static void PlayerLeftChannel(Player player)
        {
            Broadcast(new CNetLeaveChatRegionChannelNotification(true)
            {
                ChannelId = 1,
                ChatServerId = 1,
                PlayerId = player.ID
            });
        }

        public CNetDeckVO AddDeck(CNetDeckVO deck)
        {
            deck.Index = CNetDeckVO.NewIndex();
            lock (Decks)
            {
                Decks.Add(deck);
            }
            return deck;
        }

        public CNetDeckVO CreateOrSave(CNetDeckVO deckToSave)
        {
            CNetDeckVO deck;

            lock (Decks)
            {
                deck = Decks.FirstOrDefault(d => deckToSave.Index == d.Index);
            }

            if (deck == null)
            {
                deck = AddDeck(deckToSave);
            }
            else
            {
                deck.Cards = deckToSave.Cards;
                deck.DeckName = deckToSave.DeckName;
                deck.IdCardPool = deckToSave.IdCardPool;
                deck.IdLimitedPool = deckToSave.IdLimitedPool;
            }

            deck.DeckLevel = deck.CalculateDeckLevel(Cards);
            deck.CoverCard = Cards.FirstOrDefault(c => c.Index == deckToSave.CoverCard.Index) ?? new CNetCardVO();
            return deck;
        }

        public Group GetActiveGroup()
        {
            return _group;
        }

        public CNetCardVO GetCard(ulong id)
        {
            lock (Cards)
            {
                return Cards.FirstOrDefault(x => x.Index == id);
            }
        }

        public CNetCardVO[] GetCards()
        {
            lock (Cards)
            {
                return Cards.ToArray();
            }
        }

        public CNetDeckVO GetDeck(ulong id)
        {
            lock (Decks)
            {
                return Decks.FirstOrDefault(x => x.Index == id);
            }
        }

        public CNetDeckVO[] GetDecks()
        {
            lock (Decks)
            {
                return Decks.ToArray();
            }
        }

        public Session GetSession()
        {
            return _session;
        }

        public CNetWorldPlayerVO GetWorldPlayer()
        {
            return new CNetWorldPlayerVO()
            {
                Id = ID,
                IdAccount = ID,
                IdCharacter = ID,
                IdBorderline = 1,
                Gold = Gold,
                CharacterName = Name,
                BodyColor = 0,
                SkinColor = 0,
                HairColor = 0,
                IdBackground = 0,
                IdBanner = BannerID,
                IsMale = Male,
                State = State,
                Location = Location,
                HonorPoints = HonorPoints,
                VictoryPoints = VictoryPoints,
                BattlePoints = BattlePoints,
                MapID = Map,
                Experience = 7200000,
                Elo = 173000,
                EloLimited = 1,
                RankCollection = 0,
                RankLimited = 0,
                ActivityCollection = 1,
                ActivityLimited = 1,
                ActivityBonusPercentageCollection = 1,
                ActivityBonusPercentageLimited = 1,
                EloRatingUnlockPercentageCollection = 1,
                EloRatingUnlockPercentageLimited = 1,
                Disabled = false,
                IsDeckHidden = false,
            };
        }

        public void LeftGroupNotify(ulong groupId, ulong versionId = 99999)
        {
            Send(new CNetLeaveGroupNotification(true)
            {
                GroupId = groupId,
                VersionId = versionId
            });
        }

        public void Logout()
        {
            _session = null;
        }

        public void ReIndex()
        {
            ID = s_index.NewIndex();
            foreach (var card in Cards)
            {
                card.ReIndex();
            }

            foreach (var deck in Decks)
            {
                deck.ReIndex(Cards);
            }
        }

        public void RemoveDeck(ulong id)
        {
            lock (Decks)
            {
                Decks.RemoveAll(deck => deck.Index == id);
            }
        }

        public void Send(object obj)
        {
            _session?.Send(obj);
        }

        public void SetActiveGroup(Group group)
        {
            _group = group;
        }

        public void SetPlayerCoordinator(IPlayerCoordinator coordinator)
        {
            _coordinator = coordinator;
        }

        public override void Update()
        {
            base.Update();

            //Pushes changes back to local database
            _coordinator?.PushPlayerData(this);
        }

        internal static ulong GetMaxCardIndex() => Count() == 0 ? 1 : Access(db => db.Max(x => x.Cards.Count > 0 ? x.Cards.Max(y => y.Index) : 1));
        internal static ulong GetMaxDeckIndex() => Count() == 0 ? 1 : Access(db => db.Max(x => x.Decks.Count > 0 ? x.Decks.Max(y => y.Index) : 1));
        internal static ulong GetMaxPlayerIndex() => Count() > 0 ? Access(x => x.Max(x => x.ID)) : 1;
    }
}
