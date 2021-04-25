// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketHandlers;

namespace OpenForge.Server.PacketStructures.PreGame
{
    [InterfaceType(InterfaceType.PreGame)]
    public class CNetCreateCharacterRMR
    {
        public CNetDataHeader Header { get; set; }
        public int Status { get; set; }
        public bool Success { get; set; }
        public CNetWorldPlayerVO WorldCharacter { get; set; }
        public bool AutostartTutorialMap { get; set; }

        public CNetCreateCharacterRMR(bool defaultHeader)
        {
            Header = new CNetDataHeader(InterfaceType.PreGame, (int)PreGameMessageType.CNetCreateCharacterRMR, true);
            Status = default(int);
            Success = default(bool);
            WorldCharacter = default(CNetWorldPlayerVO);
            AutostartTutorialMap = default(bool);
        }
    }
}
