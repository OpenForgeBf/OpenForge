// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

namespace OpenForge.Server.PacketStructures
{
    public class CNetGamePlayerVO
    {
        public ulong Id { get; set; }
        public CNetWorldPlayerVO WorldPlayer { get; set; }
        public byte Team { get; set; }
        public CNetCardVO[] Deck { get; set; }
        public int Slot { get; set; }
    }
}
