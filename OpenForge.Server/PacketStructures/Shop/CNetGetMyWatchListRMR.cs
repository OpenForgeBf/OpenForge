// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketHandlers;

namespace OpenForge.Server.PacketStructures.Shop
{
    [InterfaceType(InterfaceType.Shop)]
    public class CNetGetMyWatchListRMR
    {
        public CNetDataHeader Header { get; set; }
        public int Status { get; set; }
        public CNetAuctionVO[] Auctions { get; set; }

        public CNetGetMyWatchListRMR(bool defaultHeader)
        {
            Header = new CNetDataHeader(InterfaceType.Shop, (int)ShopMessageType.CNetGetMyWatchListRMR, true);
            Status = default(int);
            Auctions = default(CNetAuctionVO[]);
        }
    }
}
