// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketHandlers;

namespace OpenForge.Server.PacketStructures.Matchmaking
{
    [InterfaceType(InterfaceType.Matchmaking)]
    public class CNetCustomGameCreatedNotification
    {
        public CNetDataHeader Header { get; set; }
        public ulong IdLeader { get; set; }
        public ulong IdMap { get; set; }
        public int Difficulty { get; set; }
        public ulong IdPreMatch { get; set; }
        public bool Pvp { get; set; }
        public bool IsOpenCustomGame { get; set; }
        public bool Limited { get; set; }
        public int MapChecksum { get; set; }
        public long CombinedChecksum { get; set; }

        public CNetCustomGameCreatedNotification(bool defaultHeader)
        {
            Header = new CNetDataHeader(InterfaceType.Matchmaking, (int)MatchmakingMessageType.CNetCustomGameCreatedNotification, false);
            IdLeader = default(ulong);
            IdMap = default(ulong);
            Difficulty = default(int);
            IdPreMatch = default(ulong);
            Pvp = default(bool);
            IsOpenCustomGame = default(bool);
            Limited = default(bool);
            MapChecksum = default(int);
            CombinedChecksum = default(long);
        }
    }
}
