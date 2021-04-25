// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketHandlers;

namespace OpenForge.Server.PacketStructures.PreGame
{
    [InterfaceType(InterfaceType.PreGame)]
    public class CNetActivateScratchCardForCharacterRMR
    {
        public CNetDataHeader Header { get; set; }
        public int Status { get; set; }
        public long[] Boosters { get; set; }
        public int BFPoints { get; set; }
        public long[] PromoCards { get; set; }
        public long[] DefaultDecks { get; set; }
        public long DefaultRandomDeck { get; set; }
        public bool BFGameAcces { get; set; }
        public long TomeID { get; set; }
        public int VictoryTokens { get; set; }
        public int BattleTokens { get; set; }
        public int HonorTokens { get; set; }
        public int Gold { get; set; }
        public int ElementOfCreationTimeInDays { get; set; }
        public int ElementOfConversionTimeInDays { get; set; }
        public int ElementOfConcealmentTimeInDays { get; set; }

        public CNetActivateScratchCardForCharacterRMR(bool defaultHeader)
        {
            Header = new CNetDataHeader(InterfaceType.PreGame, (int)PreGameMessageType.CNetActivateScratchCardForCharacterRMR, true);
            Status = default(int);
            Boosters = default(long[]);
            BFPoints = default(int);
            PromoCards = default(long[]);
            DefaultDecks = default(long[]);
            DefaultRandomDeck = default(long);
            BFGameAcces = default(bool);
            TomeID = default(long);
            VictoryTokens = default(int);
            BattleTokens = default(int);
            HonorTokens = default(int);
            Gold = default(int);
            ElementOfCreationTimeInDays = default(int);
            ElementOfConversionTimeInDays = default(int);
            ElementOfConcealmentTimeInDays = default(int);
        }
    }
}
