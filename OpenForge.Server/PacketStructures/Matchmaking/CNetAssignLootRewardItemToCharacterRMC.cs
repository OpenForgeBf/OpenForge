// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketHandlers;

namespace OpenForge.Server.PacketStructures.Matchmaking
{
    [InterfaceType(InterfaceType.Matchmaking)]
    public class CNetAssignLootRewardItemToCharacterRMC
    {
        public long IdLeaderAssignedLootSession { get; set; }
        public long IdReward { get; set; }
        public bool Disenchant { get; set; }
        public long IdTargetCharacter { get; set; }
    }
}
