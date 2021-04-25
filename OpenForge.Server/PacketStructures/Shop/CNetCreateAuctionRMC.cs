// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketHandlers;

namespace OpenForge.Server.PacketStructures.Shop
{
    [InterfaceType(InterfaceType.Shop)]
    public class CNetCreateAuctionRMC
    {
        public int StartBid { get; set; }
        public int BuyoutPrice { get; set; }
        public int BidTime { get; set; }
        public long[] Cards { get; set; }
        public long[] Boosters { get; set; }
    }
}
