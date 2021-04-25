// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketHandlers;

namespace OpenForge.Server.PacketStructures.Matchmaking
{
    [InterfaceType(InterfaceType.Matchmaking)]
    public class CNetMatchLostAction
    {
        public CNetMatchStatisticsVO MatchStatistics { get; set; }
        public long MatchId { get; set; }
        public long CharacterIdLoser { get; set; }
        public bool Desync { get; set; }
        public bool CrcFail { get; set; }
        public int GoldLootedFromChests { get; set; }
        public int XPGainedFromGoals { get; set; }
        public bool SuspiciousGame { get; set; }
        public int GdStep { get; set; }
        public long ActionTimestamp { get; set; }
        public long[] PlayersIngame { get; set; }
    }
}
