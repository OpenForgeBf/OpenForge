// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketHandlers;

namespace OpenForge.Server.PacketStructures.World
{
    [InterfaceType(InterfaceType.World)]
    public class CNetAddIgnoreCharacterRMR
    {
        public CNetDataHeader Header { get; set; }
        public int Status { get; set; }
        public CNetWorldPlayerVO[] IgnoreList { get; set; }

        public CNetAddIgnoreCharacterRMR(bool defaultHeader)
        {
            Header = new CNetDataHeader(InterfaceType.World, (int)WorldMessageType.CNetAddIgnoreCharacterRMR, true);
            Status = default(int);
            IgnoreList = default(CNetWorldPlayerVO[]);
        }
    }
}
