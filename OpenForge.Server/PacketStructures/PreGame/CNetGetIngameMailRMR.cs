// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketHandlers;

namespace OpenForge.Server.PacketStructures.PreGame
{
    [InterfaceType(InterfaceType.PreGame)]
    public class CNetGetIngameMailRMR
    {
        public CNetDataHeader Header { get; set; }
        public int Status { get; set; }
        public CNetIngameMailVO[] Mail { get; set; }

        public CNetGetIngameMailRMR(bool defaultHeader)
        {
            Header = new CNetDataHeader(InterfaceType.PreGame, (int)PreGameMessageType.CNetGetIngameMailRMR, true);
            Status = default(int);
            Mail = default(CNetIngameMailVO[]);
        }
    }
}
