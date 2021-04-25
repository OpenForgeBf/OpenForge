// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

namespace OpenForge.Server.PacketStructures
{
    public class CNetRankPvEVO
    {
        public long Id { get; set; }
        public long TeamID { get; set; }
        public int Rank { get; set; }
        public int InnerRank { get; set; }
        public int Difficulty { get; set; }
        public CNetWorldPlayerVO[] Players { get; set; }
        public long MatchTime { get; set; }
        public long RecordGuid { get; set; }
        public long[] HiddenCharDecks { get; set; }
    }
}
