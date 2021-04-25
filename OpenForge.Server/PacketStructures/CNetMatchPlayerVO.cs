// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

namespace OpenForge.Server.PacketStructures
{
    public class CNetMatchPlayerVO
    {
        public ulong Id { get; set; }
        public ulong CharacterIndex { get; set; }
        public string Name { get; set; }
        public string DeckName { get; set; }
        public ulong CoverCardID { get; set; }
        public bool IsReady { get; set; }
        public long PvELevel { get; set; }
        public long PvPLevel { get; set; }
        public int DeckLevel { get; set; }
    }
}
