// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

namespace OpenForge.Server.PacketStructures
{
    public class CNetCharacterTokenRewardVO
    {
        public long Id { get; set; }
        public long IdCharacter { get; set; }
        public int IsBoosted { get; set; }
        public int HasConvertUpgradeToTokensBoostItem { get; set; }
        public int Multiplier { get; set; }
        public int BattleTokens { get; set; }
        public int VictoryTokens { get; set; }
        public int HonorTokens { get; set; }
        public int Gold { get; set; }
    }
}
