// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

namespace OpenForge.Server.PacketStructures
{
    public class CNetCharacterRewardVO
    {
        public long Id { get; set; }
        public long IdCharacter { get; set; }
        public long IdRewardCard { get; set; }
        public int Upgradelevel { get; set; }
        public int LootCategory { get; set; }
        public CNetCharacterTokenRewardVO[] DisenchantGoldPerPlayer { get; set; }
        public int IsBoosted { get; set; }
        public int Multiplier { get; set; }
        public CNetCharacterTokenRewardVO TokenRewardAfterConversion { get; set; }
    }
}
