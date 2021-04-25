// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

namespace OpenForge.Server.PacketStructures
{
    public class CNetCustomGameOverviewVO
    {
        public ulong Id { get; set; }
        public CNetWorldPlayerVO GroupLeader { get; set; }
        public ulong IdMap { get; set; }
        public int CountOpenSlots { get; set; }
        public int Difficulty { get; set; }
        public int RewardMode { get; set; }
        public bool Limited { get; set; }
        public int MapChecksum { get; set; }
        public long CombinedChecksum { get; set; }
        public string[] MapNames { get; set; }
        public int MapOfTheWeek { get; set; }
        public bool Speedrun { get; set; }
        public bool MapFileAvailableOnServer { get; set; }
    }
}
