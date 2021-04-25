// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketHandlers;

namespace OpenForge.Server.PacketStructures.World
{
    [InterfaceType(InterfaceType.World)]
    public class CNetGetRankingPvP2vs2TopTeamByCharacterRMR
    {
        public CNetDataHeader Header { get; set; }
        public int Status { get; set; }
        public CNetRankPvPVO Rank { get; set; }

        public CNetGetRankingPvP2vs2TopTeamByCharacterRMR(bool defaultHeader)
        {
            Header = new CNetDataHeader(InterfaceType.World, (int)WorldMessageType.CNetGetRankingPvP2vs2TopTeamByCharacterRMR, true);
            Status = default(int);
            Rank = default(CNetRankPvPVO);
        }
    }
}
