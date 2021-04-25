// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketHandlers;

namespace OpenForge.Server.PacketStructures.World
{
    [InterfaceType(InterfaceType.World)]
    public class CNetGetRankingPvEByRangeRMC
    {
        public int RangeStart { get; set; }
        public int RangeEnd { get; set; }
        public int Mode { get; set; }
        public int MapID { get; set; }
        public int Filter { get; set; }
        public int NumMonthsBack { get; set; }
    }
}
