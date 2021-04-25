// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketHandlers;

namespace OpenForge.Server.PacketStructures.PreGame
{
    [InterfaceType(InterfaceType.PreGame)]
    public class CNetSaveDeckForCharacterRMR
    {
        public CNetDataHeader Header { get; set; }
        public int Status { get; set; }
        public bool Success { get; set; }
        public ulong NewDeckId { get; set; }
        public int NewDeckLevel { get; set; }

        public CNetSaveDeckForCharacterRMR(bool defaultHeader)
        {
            Header = new CNetDataHeader(InterfaceType.PreGame, (int)PreGameMessageType.CNetSaveDeckForCharacterRMR, true);
            Status = default(int);
            Success = default(bool);
            NewDeckId = default(ulong);
            NewDeckLevel = default(int);
        }
    }
}
