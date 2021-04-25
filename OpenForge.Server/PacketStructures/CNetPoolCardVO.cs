// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

namespace OpenForge.Server.PacketStructures
{
    public class CNetPoolCardVO
    {
        public long Index { get; set; }
        public ulong CardTypeId { get; set; }
        public bool IsPromoCard { get; set; }
        public int Upgrades { get; set; }
        public int ChargeAmount { get; set; }
        public ulong MasterCardId { get; set; }
        public ulong[] CardAndDuplicates { get; set; }
        public ulong[] NonTradeableCards { get; set; }
        public ulong IdLimitedPool { get; set; }
    }
}
