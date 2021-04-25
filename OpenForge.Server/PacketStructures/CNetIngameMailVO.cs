// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

namespace OpenForge.Server.PacketStructures
{
    public class CNetIngameMailVO
    {
        public ulong Index { get; set; }
        public string From { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public ulong[] CardsIndex { get; set; }
        public int MailType { get; set; }
        public long[] Boosters { get; set; }
        public CNetBattleForgeDateTimeVO SendDate { get; set; }
        public int Gold { get; set; }
        public long Type { get; set; }
        public int BFP { get; set; }
        public bool Collected { get; set; }
        public ulong[] AuctionCards { get; set; }
        public long[] AuctionBoosters { get; set; }
        public int AuctionInfoBFPoints { get; set; }
        public string ReferrerName { get; set; }
        public int RewardBattleTokens { get; set; }
        public int RewardVictoryTokens { get; set; }
        public int RewardHonorTokens { get; set; }
        public int AuctionFee { get; set; }
        public byte IsOfficial { get; set; }
    }
}
