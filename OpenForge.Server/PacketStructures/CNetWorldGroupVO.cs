// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

namespace OpenForge.Server.PacketStructures
{
    public class CNetWorldGroupVO
    {
        public ulong Id { get; set; }
        public ulong VersionId { get; set; }
        public ulong IdGroup { get; set; }
        public int IdChatServer { get; set; }
        public int IdChatChannel { get; set; }
        public CNetWorldPlayerVO Leader { get; set; }
        public CNetWorldPlayerVO[] Players { get; set; }
    }
}
