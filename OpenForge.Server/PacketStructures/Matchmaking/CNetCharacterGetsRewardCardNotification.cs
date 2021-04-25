// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketHandlers;

namespace OpenForge.Server.PacketStructures.Matchmaking
{
    [InterfaceType(InterfaceType.Matchmaking)]
    public class CNetCharacterGetsRewardCardNotification
    {
        public CNetDataHeader Header { get; set; }
        public long MatchId { get; set; }
        public CNetCardVO RewardCard { get; set; }

        public CNetCharacterGetsRewardCardNotification(bool defaultHeader)
        {
            Header = new CNetDataHeader(InterfaceType.Matchmaking, (int)MatchmakingMessageType.CNetCharacterGetsRewardCardNotification, false);
            MatchId = default(long);
            RewardCard = default(CNetCardVO);
        }
    }
}
