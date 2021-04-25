// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

namespace OpenForge.Server.PacketStructures
{
    public class CNetPlayerStatisticsVO
    {
        public long Id { get; set; }
        public long IdCharacter { get; set; }
        public string Name { get; set; }
        public bool ComputerPlayer { get; set; }
        public long PowerGained { get; set; }
        public long PowerSpent { get; set; }
        public long PowerRefundMax { get; set; }
        public long MaxTokenSlots { get; set; }
        public long MaxTokenSlotsBuildState { get; set; }
    }
}
