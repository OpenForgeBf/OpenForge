// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketHandlers;

namespace OpenForge.Server.PacketStructures.Matchmaking
{
    [InterfaceType(InterfaceType.Matchmaking)]
    public class CNetCustomGameUpdatedNotification
    {
        public CNetDataHeader Header { get; set; }
        public ulong IdPreMatch { get; set; }
        public ulong IdMap { get; set; }
        public bool PvP { get; set; }
        public CNetMatchPlayerVO[] Team1 { get; set; }
        public CNetMatchPlayerVO[] Team2 { get; set; }

        public CNetCustomGameUpdatedNotification(bool defaultHeader)
        {
            Header = new CNetDataHeader(InterfaceType.Matchmaking, (int)MatchmakingMessageType.CNetCustomGameUpdatedNotification, false);
            IdPreMatch = default(ulong);
            IdMap = default(ulong);
            PvP = default(bool);
            Team1 = default(CNetMatchPlayerVO[]);
            Team2 = default(CNetMatchPlayerVO[]);
        }
    }
}
