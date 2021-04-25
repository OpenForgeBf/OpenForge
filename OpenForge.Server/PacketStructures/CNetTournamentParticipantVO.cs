// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

namespace OpenForge.Server.PacketStructures
{
    public class CNetTournamentParticipantVO
    {
        //public float WinningPercent { get; set; }
        public long Id { get; set; }

        public string Name { get; set; }
        public int Rank { get; set; }
        public int WinningPoints { get; set; }
        public int Unknown5 { get; set; }
        public int PlayedMatches { get; set; }
        public int WonMatches { get; set; }
        public int LoosedMatches { get; set; }
        public bool Disconnected { get; set; }
        public bool Leaved { get; set; }
    }
}
