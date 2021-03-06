// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketHandlers;

namespace OpenForge.Server.PacketStructures.Matchmaking
{
    [InterfaceType(InterfaceType.Matchmaking)]
    public class CNetGetAllOpenCustomGamesRMR
    {
        public CNetDataHeader Header { get; set; }
        public int Status { get; set; }
        public CNetCustomGameOverviewVO[] CustomGames { get; set; }

        public CNetGetAllOpenCustomGamesRMR(bool defaultHeader)
        {
            Header = new CNetDataHeader(InterfaceType.Matchmaking, (int)MatchmakingMessageType.CNetGetAllOpenCustomGamesRMR, true);
            Status = default(int);
            CustomGames = default(CNetCustomGameOverviewVO[]);
        }
    }
}
