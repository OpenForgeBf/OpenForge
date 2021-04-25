// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

namespace OpenForge.Server.PacketStructures
{
    public class CNetPlayerVO
    {
        public ulong Index { get; set; }
        public string Name { get; set; }
        public byte Team { get; set; }
        public byte Color { get; set; }
        public CNetDeckVO Deck { get; set; }
        public CNetWorldPlayerVO WorldPlayer { get; set; }
    }
}
