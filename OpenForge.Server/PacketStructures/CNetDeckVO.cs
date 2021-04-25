// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using OpenForge.Server.Database.Memory;
using OpenForge.Server.Enumerations;

namespace OpenForge.Server.PacketStructures
{
    public class CNetDeckVO
    {
        private static readonly IndexManager _index = new IndexManager(() => Player.GetMaxDeckIndex());

        public ulong Index { get; set; }
        public string DeckName { get; set; }
        public CNetDeckCardVO[] Cards { get; set; }
        public CNetCardVO CoverCard { get; set; }
        public int DeckLevel { get; set; }
        public CardPool IdCardPool { get; set; }
        public CardPool IdLimitedPool { get; set; }

        public int CalculateDeckLevel(List<CNetCardVO> playerCards)
        {
            return CalculateDeckLevel(this, playerCards);
        }

        public static int CalculateDeckLevel(CNetDeckVO deck, List<CNetCardVO> playerCards)
        {
            var level = 0;
            foreach (var deckCard in deck.Cards)
            {
                var playerCard = playerCards.First(pc => pc.Index == deckCard.Index);

                if (playerCard.IsPromoCard)
                {
                    level += 6;
                }
                else
                {
                    level += playerCard.UpgradeLevel;
                    level += playerCard.ChargeAmount;
                }
            }

            return level;
        }

        public static ulong NewIndex() => _index.NewIndex();

        public void ReIndex(List<CNetCardVO> cards)
        {
            var toRemove = new List<CNetDeckCardVO>();
            Index = _index.NewIndex();
            foreach (var card in Cards)
            {
                var c = cards.FirstOrDefault(x => x.CardIndex == card.Card.CardIndex);
                if (c == null)
                {
                    toRemove.Add(card);
                }
                else
                {
                    card.Index = c.Index;
                    card.Card = c;
                }
            }
        }

        public static CNetDeckVO CreateTutorialDeck(List<CNetCardVO> cards)
        {
            ulong cover = 700;
            var cardIndices = new ulong[] { 288, 253, 354, 700, 673, 379, 287 };
            var deck = new CNetDeckVO()
            {
                Index = _index.NewIndex(),
                DeckName = "Tutorial",
                IdCardPool = CardPool.Tutorial,
                IdLimitedPool = CardPool.Tutorial
            };

            deck.Cards = new CNetDeckCardVO[cardIndices.Length];

            for (var i = 0; i < cardIndices.Length; i++)
            {
                var cardIndex = cardIndices[i];
                var card = cards.First(c => c.CardIndex == cardIndex);

                deck.Cards[i] = new CNetDeckCardVO()
                {
                    Index = card.Index,
                    Position = i,
                    Card = card
                };
            }

            deck.CoverCard = cards.First(c => c.CardIndex == cover);
            deck.DeckLevel = deck.CalculateDeckLevel(cards);

            return deck;
        }
    }
}
