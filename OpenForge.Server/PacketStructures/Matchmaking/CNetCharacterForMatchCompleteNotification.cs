// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketHandlers;

namespace OpenForge.Server.PacketStructures.Matchmaking
{
    [InterfaceType(InterfaceType.Matchmaking)]
    public class CNetCharacterForMatchCompleteNotification
    {
        public CNetDataHeader Header { get; set; }
        public ulong IdMatch { get; set; }
        public ulong IdMap { get; set; }
        public int IdGameServer { get; set; }
        public int IdChatServer { get; set; }
        public int IdGeneralChatChannel { get; set; }
        public int IdTeamChatChannel { get; set; }
        public short RandomSeed { get; set; }
        public CNetPlayerVO[] Players { get; set; }
        public long IdTeamStatistic { get; set; }
        public long IdTeamEloRating { get; set; }

        public CNetCharacterForMatchCompleteNotification(bool defaultHeader)
        {
            Header = new CNetDataHeader(InterfaceType.Matchmaking, (int)MatchmakingMessageType.CNetCharacterForMatchCompleteNotification, false);
            IdMatch = default(ulong);
            IdMap = default(ulong);
            IdGameServer = default(int);
            IdChatServer = default(int);
            IdGeneralChatChannel = default(int);
            IdTeamChatChannel = default(int);
            RandomSeed = default(short);
            Players = default(CNetPlayerVO[]);
            IdTeamStatistic = default(long);
            IdTeamEloRating = default(long);
        }
    }
}
