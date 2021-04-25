// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

namespace OpenForge.Server.PacketStructures
{
    public class CNetLimitedPoolVO
    {
        public long Id { get; set; }
        public CNetBattleForgeDateTimeVO CreateDateTime { get; set; }
        public CNetBattleForgeDateTimeVO CloseDateTime { get; set; }
        public int Status { get; set; }
    }
}
