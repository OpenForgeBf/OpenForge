// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;

namespace OpenForge.Server.Maps
{
    public enum MapType
    {
        Unknown = 0,
        Campaign = 1,
        Pvp = 2,
        Community = 3
    }

    public class CampaignMap : MapInfo
    {
        public CampaignMap(ulong id, int slots, string name) : base(id, slots, name)
        {
        }

        public override MapType Type => MapType.Campaign;
    }

    public class MapInfo
    {
        private static readonly List<MapInfo> _maps = new List<MapInfo>()
        {
            //Campaign (1)
            new CampaignMap(67, 1, "Introduction"),
            new CampaignMap(45, 1, "EncountersWithTwilight"),
            new CampaignMap(8, 1, "SiegeOfHope"),
            new CampaignMap(57, 1, "DefendingHope"),
            new CampaignMap(22, 1, "TheSoulTree"),
            new CampaignMap(20, 1, "TheTreasureFleet"),
            new CampaignMap(32, 1, "BehindEnemyLines"),
            new CampaignMap(88, 1, "Mo"),
            new CampaignMap(84, 1, "Ocean"),
            new CampaignMap(99, 1, "Oracle"),
            new CampaignMap(101, 1, "SinglePlayer"),

            //Campaign (2)
            new CampaignMap(9, 2, "Crusade"),
            new CampaignMap(18, 2, "Sunbridge"),
            new CampaignMap(24, 2, "NightmareShard"),
            new CampaignMap(44, 2, "NightmaresEnd"),
            new CampaignMap(21, 2, "TheInsaneGod"),
            new CampaignMap(35, 2, "SlaveMaster"),
            new CampaignMap(56, 2, "Convoy"),
            new CampaignMap(102, 2, "DualPlayer"),

            //Campaign (4)
            new CampaignMap(10, 4, "BadHarvest"),
            new CampaignMap(19, 4, "TheDwarvenRiddle"),
            new CampaignMap(26, 4, "KingOfTheGiants"),
            new CampaignMap(29, 4, "Titans"),
            new CampaignMap(74, 4, "Blight"),
            new CampaignMap(98, 4, "RavensEnd"),
            new CampaignMap(100, 4, "Empire"),
            new CampaignMap(25, 4, "TheGunsOfLyr"),
            new CampaignMap(103, 4, "QuadPlayer"),

            //Campaign (12)
            new CampaignMap(60, 2, "PassageToDarkness"),
            new CampaignMap(37, 12, "Ascension"),

            //Pvp (2)
            new PvpMap(7, 2, "Simai"),
            new PvpMap(15, 2, "Haladur"),
            new PvpMap(17, 2, "Elyon"),
            new PvpMap(27, 2, "Wazhai"),
            new PvpMap(30, 2, "Lajesh"),
            new PvpMap(33, 2, "Uro"),
            new PvpMap(71, 2, "Yrmia"),
            new PvpMap(96, 2, "Generated1v1"),

            //Pvp (4)
            new PvpMap(6, 4, "Turan"),
            new PvpMap(14, 4, "Danduil"),
            new PvpMap(16, 4, "Fyre"),
            new PvpMap(31, 4, "Yshia"),
            new PvpMap(34, 4, "Nadai"),
            new PvpMap(78, 4, "Koshan"),
            new PvpMap(79, 4, "Zahadune"),
            new PvpMap(97, 4, "Generated2v2"),

            //Pvp (6)
            new PvpMap(28, 2, "Gorgash"),
            new PvpMap(109, 6, "Generated3v3"),
        };

        public MapInfo(ulong id, int slots, string name)
        {
            ID = id;
            Slots = slots;
            Name = name;
        }

        public ulong ID { get; set; }
        public string Name { get; set; }
        public int Slots { get; set; }
        public virtual MapType Type => MapType.Unknown;

        public static MapInfo GetMap(ulong id)
        {
            return _maps.FirstOrDefault(x => x.ID == id);
        }
    }

    public class PvpMap : MapInfo
    {
        public PvpMap(ulong id, int slots, string name) : base(id, slots, name)
        {
        }

        public override MapType Type => MapType.Pvp;
    }
}
