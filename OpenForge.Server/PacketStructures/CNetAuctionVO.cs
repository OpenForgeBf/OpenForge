// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

namespace OpenForge.Server.PacketStructures
{
    public class CNetAuctionVO
    {
        public ulong Id { get; set; }
        public int StartBid { get; set; }
        public int PriceBuyout { get; set; }
        public int CurrentBid { get; set; }
        public ulong IdCurrentBidder { get; set; }
        public int TimeLeft { get; set; }
        public string Seller { get; set; }
        public long[] Cards { get; set; }
        public long[] Boosters { get; set; }
    }
}
