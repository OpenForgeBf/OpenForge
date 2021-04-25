// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

namespace OpenForge.Server.PacketStructures
{
    public class CNetBoosterVO
    {
        public ulong Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public long IdEdition { get; set; }
    }
}
