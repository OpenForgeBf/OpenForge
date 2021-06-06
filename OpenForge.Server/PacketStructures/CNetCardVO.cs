// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using OpenForge.Server.Database.Memory;
using OpenForge.Server.Enumerations;

namespace OpenForge.Server.PacketStructures
{
    public class CNetCardVO
    {
        private static readonly IndexManager _index = new IndexManager(() => Player.GetMaxCardIndex());

        public ulong Index { get; set; }
        public ulong CardIndex { get; set; }
        public bool IsMasterCard { get; set; }
        public bool IsPromoCard { get; set; }
        public int UpgradeLevel { get; set; }
        public int ChargeAmount { get; set; }
        public string CardName { get; set; }
        public ulong IdCardPool { get; set; }
        public ulong IdLimitedPool { get; set; }
        public bool IsTradeable { get; set; }

        public CNetCardVO()
        {
        }

        public CNetCardVO(bool newId)
        {
            if (newId)
            {
                Index = _index.NewIndex();
            }
        }

        public void ReIndex()
        {
            Index = _index.NewIndex();
        }

        public static CNetCardVO FromTemplate(CardTemplate template)
        {
            return new CNetCardVO()
            {
                Index = _index.NewIndex(),
                CardIndex = (ulong)template.CardIndex,
                CardName = template.Name,
                ChargeAmount = template.IsPromo ? 0 : 3,
                UpgradeLevel = template.IsPromo ? 0 : 3,
                IsTradeable = true,
                IsPromoCard = template.IsPromo,
                IsMasterCard = true,
                IdCardPool = (ulong)CardPool.Collection,
                IdLimitedPool = (ulong)CardPool.Collection,
            };
        }
    }
}
