// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketHandlers;

namespace OpenForge.Server.PacketStructures.Matchmaking
{
    [InterfaceType(InterfaceType.Matchmaking)]
    public class CNetRegisterCharacterForPvPAutomatchFunDuelRMC
    {
        public long IdDeck { get; set; }
        public long IdPlayerAmount { get; set; }
        public bool Limited { get; set; }
    }
}
