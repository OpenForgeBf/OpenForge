// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketHandlers;

namespace OpenForge.Server.PacketStructures.Matchmaking
{
    [InterfaceType(InterfaceType.Matchmaking)]
    public class CNetCreateGroupSMR
    {
        public CNetDataHeader Header { get; set; }
        public long IdGroup { get; set; }
        public long IdGroupLeader { get; set; }
        public CNetWorldPlayerVO Member { get; set; }

        public CNetCreateGroupSMR(bool defaultHeader)
        {
            Header = new CNetDataHeader(InterfaceType.Matchmaking, (int)MatchmakingMessageType.CNetCreateGroupSMR, false);
            IdGroup = default(long);
            IdGroupLeader = default(long);
            Member = default(CNetWorldPlayerVO);
        }
    }
}
