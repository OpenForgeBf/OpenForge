// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;

namespace OpenForge.Server.PacketStructures
{
    public class CNetPoolCardCollectionVO
    {
        public ulong Id { get; set; }
        public CNetPoolCardVO[] Cards { get; set; }

        public static CNetPoolCardCollectionVO FromCards(IEnumerable<CNetCardVO> cards)
        {
            long index = 0;

            var poolCards = cards.GroupBy(c => c.CardIndex).Select(g =>
            {
                var masterCard = g.First(c => c.IsMasterCard);

                return new CNetPoolCardVO()
                {
                    Index = index++,
                    IdLimitedPool = masterCard.IdLimitedPool,
                    CardAndDuplicates = g.Select(c => c.Index).ToArray(),
                    CardTypeId = masterCard.CardIndex,
                    ChargeAmount = masterCard.ChargeAmount,
                    IsPromoCard = masterCard.IsPromoCard,
                    MasterCardId = masterCard.Index,
                    NonTradeableCards = g.Where(c => !c.IsTradeable).Select(c => c.Index).ToArray(),
                    Upgrades = masterCard.UpgradeLevel
                };
            }).ToArray();

            return new CNetPoolCardCollectionVO()
            {
                Id = 0,
                Cards = poolCards
            };
        }
    }
}
