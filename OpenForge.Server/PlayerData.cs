// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketStructures;

namespace OpenForge.Server
{
    public class PlayerData
    {
        public CNetCardVO[] Cards;
        public CNetDeckVO[] Decks;
        public int Version;

        public PlayerData()
        {
        }

        public PlayerData(List<CNetCardVO> cards, List<CNetDeckVO> decks)
        {
            Version = 1;
            Cards = cards.ToArray();
            Decks = decks.ToArray();
        }

        public PlayerData(CNetCardVO[] cards, CNetDeckVO[] decks)
        {
            Version = 1;
            Cards = cards;
            Decks = decks;
        }

        public static PlayerData CreateDefault()
        {
            var cardIndices = new int[] { 288, 253, 354, 700, 673, 379, 287 };

            var cards = new List<CNetCardVO>();
            var decks = new List<CNetDeckVO>();

            foreach (var cardIndex in cardIndices)
            {
                var cardTemplate = CardTemplate.CardTemplates.First(c => c.CardIndex == cardIndex);

                cards.Add(new CNetCardVO(true)
                {
                    CardIndex = (ulong)cardIndex,
                    CardName = cardTemplate.Name,
                    ChargeAmount = 0,
                    UpgradeLevel = 0,
                    IsTradeable = false,
                    IsPromoCard = cardTemplate.IsPromo,
                    IsMasterCard = true,
                    IdCardPool = (ulong)CardPool.Tutorial,
                    IdLimitedPool = (ulong)CardPool.Tutorial,
                });
            }

            foreach (var cardTemplate in CardTemplate.CardTemplates)
            {
                cards.Add(CNetCardVO.FromTemplate(cardTemplate));
            }

            decks.Add(CNetDeckVO.CreateTutorialDeck(cards));

            return new PlayerData(cards, decks);
        }
    }
}
