// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketHandlers;

namespace OpenForge.Server.PacketStructures.Matchmaking
{
    [InterfaceType(InterfaceType.Matchmaking)]
    public class CNetGetAllCriteriasRMR
    {
        public CNetDataHeader Header { get; set; }
        public int Status { get; set; }
        public long[] MapIds { get; set; }
        public long[] GameTypeIds { get; set; }
        public long[] FormatIds { get; set; }
        public long[] LevelIds { get; set; }

        public CNetGetAllCriteriasRMR(bool defaultHeader)
        {
            Header = new CNetDataHeader(InterfaceType.Matchmaking, (int)MatchmakingMessageType.CNetGetAllCriteriasRMR, true);
            Status = default(int);
            MapIds = default(long[]);
            GameTypeIds = default(long[]);
            FormatIds = default(long[]);
            LevelIds = default(long[]);
        }
    }
}
