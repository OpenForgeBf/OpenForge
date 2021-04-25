// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketHandlers;

namespace OpenForge.Server.PacketStructures.Matchmaking
{
    [InterfaceType(InterfaceType.Matchmaking)]
    public class CNetMatchFinishedNotification
    {
        public CNetDataHeader Header { get; set; }
        public ulong IdMatch { get; set; }
        public long IdLootingType { get; set; }
        public bool IsLimited { get; set; }
        public ulong[] WinnerCharacterIds { get; set; }
        public ulong[] LooserCharacterIds { get; set; }
        public CNetCharacterTokenRewardVO[] TokenRewardList { get; set; }
        public CNetCharacterRewardVO[] RewardList { get; set; }
        public CNetCharacterXPVO[] XPList { get; set; }
        public CNetCharacterEloRatingVO[] EloRatingList { get; set; }
        public bool TeamBasedEloRating { get; set; }

        public CNetMatchFinishedNotification(bool defaultHeader)
        {
            Header = new CNetDataHeader(InterfaceType.Matchmaking, (int)MatchmakingMessageType.CNetMatchFinishedNotification, false);
            IdMatch = default(ulong);
            IdLootingType = default(long);
            IsLimited = default(bool);
            WinnerCharacterIds = default(ulong[]);
            LooserCharacterIds = default(ulong[]);
            TokenRewardList = default(CNetCharacterTokenRewardVO[]);
            RewardList = default(CNetCharacterRewardVO[]);
            XPList = default(CNetCharacterXPVO[]);
            EloRatingList = default(CNetCharacterEloRatingVO[]);
            TeamBasedEloRating = default(bool);
        }
    }
}
