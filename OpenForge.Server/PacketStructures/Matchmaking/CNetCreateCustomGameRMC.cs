﻿// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketHandlers;

namespace OpenForge.Server.PacketStructures.Matchmaking
{
    [InterfaceType(InterfaceType.Matchmaking)]
    public class CNetCreateCustomGameRMC
    {
        public ulong IdDeck { get; set; }
        public ulong IdMap { get; set; }
        public int Difficulty { get; set; }
        public int LootType { get; set; }
        public CNetWorldGroupVO Group { get; set; }
        public bool Pvp { get; set; }
        public bool Limited { get; set; }
        public bool TrialUser { get; set; }
        public int MapChecksum { get; set; }
        public long CombinedChecksum { get; set; }
        public string[] MapNames { get; set; }
        public bool MapMonth { get; set; }
        public bool Speedrun { get; set; }
    }
}
