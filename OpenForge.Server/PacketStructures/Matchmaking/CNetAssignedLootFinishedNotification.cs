// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketHandlers;

namespace OpenForge.Server.PacketStructures.Matchmaking
{
    [InterfaceType(InterfaceType.Matchmaking)]
    public class CNetAssignedLootFinishedNotification
    {
        public CNetDataHeader Header { get; set; }
        public long IdAssignedLootSession { get; set; }
        public CNetCharacterRewardVO[] RewardList { get; set; }
        public long IdMatch { get; set; }

        public CNetAssignedLootFinishedNotification(bool defaultHeader)
        {
            Header = new CNetDataHeader(InterfaceType.Matchmaking, (int)MatchmakingMessageType.CNetAssignedLootFinishedNotification, false);
            IdAssignedLootSession = default(long);
            RewardList = default(CNetCharacterRewardVO[]);
            IdMatch = default(long);
        }
    }
}
