// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketHandlers;

namespace OpenForge.Server.PacketStructures.Matchmaking
{
    [InterfaceType(InterfaceType.Matchmaking)]
    public class CNetAcceptRegisterGroupForPvPAutomatchFunDuelNotification
    {
        public CNetDataHeader Header { get; set; }
        public long IdCoverCard { get; set; }
        public long IdCharacter { get; set; }
        public string Deckname { get; set; }
        public int DeckLevel { get; set; }

        public CNetAcceptRegisterGroupForPvPAutomatchFunDuelNotification(bool defaultHeader)
        {
            Header = new CNetDataHeader(InterfaceType.Matchmaking, (int)MatchmakingMessageType.CNetAcceptRegisterGroupForPvPAutomatchFunDuelNotification, false);
            IdCoverCard = default(long);
            IdCharacter = default(long);
            Deckname = default(string);
            DeckLevel = default(int);
        }
    }
}
