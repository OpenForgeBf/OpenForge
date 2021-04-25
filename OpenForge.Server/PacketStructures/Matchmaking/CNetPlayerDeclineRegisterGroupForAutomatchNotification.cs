// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketHandlers;

namespace OpenForge.Server.PacketStructures.Matchmaking
{
    [InterfaceType(InterfaceType.Matchmaking)]
    public class CNetPlayerDeclineRegisterGroupForAutomatchNotification
    {
        public CNetDataHeader Header { get; set; }
        public long IdLeader { get; set; }
        public string DeclineName { get; set; }

        public CNetPlayerDeclineRegisterGroupForAutomatchNotification(bool defaultHeader)
        {
            Header = new CNetDataHeader(InterfaceType.Matchmaking, (int)MatchmakingMessageType.CNetPlayerDeclineRegisterGroupForAutomatchNotification, false);
            IdLeader = default(long);
            DeclineName = default(string);
        }
    }
}
