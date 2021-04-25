// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

namespace OpenForge.Server.PacketStructures
{
    public class CNetCardUpgradeVO
    {
        public long Id { get; set; }
        public long CardId { get; set; }
        public int UpgradeLevel { get; set; }
        public bool Used { get; set; }
    }
}
