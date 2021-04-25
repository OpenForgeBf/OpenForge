// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketHandlers;

namespace OpenForge.Server.PacketStructures.Matchmaking
{
    [InterfaceType(InterfaceType.Matchmaking)]
    public class CNetCharacterJoinedGroupNotification
    {
        public CNetDataHeader Header { get; set; }
        public CNetWorldPlayerVO WorldPlayer { get; set; }
        public long IdGroupLeader { get; set; }

        public CNetCharacterJoinedGroupNotification(bool defaultHeader)
        {
            Header = new CNetDataHeader(InterfaceType.Matchmaking, (int)MatchmakingMessageType.CNetCharacterJoinedGroupNotification, false);
            WorldPlayer = default(CNetWorldPlayerVO);
            IdGroupLeader = default(long);
        }
    }
}
