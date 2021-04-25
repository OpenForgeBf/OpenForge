// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

namespace OpenForge.Server.PacketStructures
{
    public class CNetProductVO
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public int BEditionId { get; set; }
        public int Cost { get; set; }
        public long[] Data { get; set; }
        public long[] BundledProductIds { get; set; }
        public long[] BundledEditionIds { get; set; }
        public int BoostItemDurationHours { get; set; }
    }
}
