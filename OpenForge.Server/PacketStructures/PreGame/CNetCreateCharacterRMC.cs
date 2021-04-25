// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketHandlers;

namespace OpenForge.Server.PacketStructures.PreGame
{
    [InterfaceType(InterfaceType.PreGame)]
    public class CNetCreateCharacterRMC
    {
        public string Name { get; set; }
        public int BodyColor { get; set; }
        public int DetailColor { get; set; }
        public int SkinColor { get; set; }
        public int HairColor { get; set; }
        public ulong IdBackground { get; set; }
        public long IdBanner { get; set; }
        public bool Male { get; set; }
        public bool TrialUser { get; set; }
        public bool HadOriginPersona { get; set; }
    }
}
