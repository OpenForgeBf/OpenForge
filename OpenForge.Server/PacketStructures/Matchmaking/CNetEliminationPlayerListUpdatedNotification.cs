// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketHandlers;

namespace OpenForge.Server.PacketStructures.Matchmaking
{
    [InterfaceType(InterfaceType.Matchmaking)]
    public class CNetEliminationPlayerListUpdatedNotification
    {
        public CNetDataHeader Header { get; set; }
        public int RoundNumber { get; set; }
        public long IdElimination { get; set; }
        public string[] Playernames { get; set; }

        public CNetEliminationPlayerListUpdatedNotification(bool defaultHeader)
        {
            Header = new CNetDataHeader(InterfaceType.Matchmaking, (int)MatchmakingMessageType.CNetEliminationPlayerListUpdatedNotification, false);
            RoundNumber = default(int);
            IdElimination = default(long);
            Playernames = default(string[]);
        }
    }
}
