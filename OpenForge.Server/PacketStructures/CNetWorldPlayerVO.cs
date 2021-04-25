// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using OpenForge.Server.Enumerations;

namespace OpenForge.Server.PacketStructures
{
    public class CNetWorldPlayerVO
    {
        public ulong Id { get; set; }
        public ulong IdAccount { get; set; }
        public ulong IdCharacter { get; set; }
        public int IdBorderline { get; set; }
        public int Gold { get; set; }
        public string CharacterName { get; set; }
        public int BodyColor { get; set; }
        public int SkinColor { get; set; }
        public int HairColor { get; set; }
        public ulong IdBackground { get; set; }
        public long IdBanner { get; set; }
        public bool IsMale { get; set; }
        public PlayerState State { get; set; }
        public LocationType Location { get; set; }
        public int HonorPoints { get; set; }
        public int VictoryPoints { get; set; }
        public int BattlePoints { get; set; }
        public ulong MapID { get; set; }
        public long Experience { get; set; }
        public long Elo { get; set; }
        public long EloLimited { get; set; }
        public int RankCollection { get; set; }
        public int RankLimited { get; set; }
        public int ActivityCollection { get; set; }
        public int ActivityLimited { get; set; }
        public int ActivityBonusPercentageCollection { get; set; }
        public int ActivityBonusPercentageLimited { get; set; }
        public int EloRatingUnlockPercentageCollection { get; set; }
        public int EloRatingUnlockPercentageLimited { get; set; }
        public bool Disabled { get; set; }
        public bool IsDeckHidden { get; set; }
    }
}
