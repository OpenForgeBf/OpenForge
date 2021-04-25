// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketHandlers;

namespace OpenForge.Server.PacketStructures.World
{
    [InterfaceType(InterfaceType.World)]
    public class CNetGetRankingPvP2vs2TeamsByCharacterRMR
    {
        public CNetDataHeader Header { get; set; }
        public int Status { get; set; }
        public CNetRankPvPVO[] Ranks { get; set; }
        public int TotalRanksAmount { get; set; }

        public CNetGetRankingPvP2vs2TeamsByCharacterRMR(bool defaultHeader)
        {
            Header = new CNetDataHeader(InterfaceType.World, (int)WorldMessageType.CNetGetRankingPvP2vs2TeamsByCharacterRMR, true);
            Status = default(int);
            Ranks = default(CNetRankPvPVO[]);
            TotalRanksAmount = default(int);
        }
    }
}
