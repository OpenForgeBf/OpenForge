// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

namespace OpenForge.Server.PacketStructures
{
    public class CNetRankPvPVO
    {
        public long Id { get; set; }
        public long TeamID { get; set; }
        public int Rank { get; set; }
        public CNetWorldPlayerVO[] Players { get; set; }
        public CNetBattleForgeDateTimeVO TeamDeletionDate { get; set; }
        public int MatchCountLast24h { get; set; }
        public int Victories { get; set; }
        public int Losses { get; set; }
        public long EloScore { get; set; }
        public long Rating { get; set; }
        public int Activity { get; set; }
        public int Bonus { get; set; }
        public int EloRatingUnlockPercentage { get; set; }
    }
}
