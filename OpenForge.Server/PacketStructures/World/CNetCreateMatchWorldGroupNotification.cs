// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketHandlers;

namespace OpenForge.Server.PacketStructures.World
{
    [InterfaceType(InterfaceType.World)]
    public class CNetCreateMatchWorldGroupNotification
    {
        public CNetDataHeader Header { get; set; }
        public int IdGroup { get; set; }
        public CNetWorldPlayerVO GroupLeader { get; set; }
        public CNetWorldPlayerVO[] Players { get; set; }

        public CNetCreateMatchWorldGroupNotification(bool defaultHeader)
        {
            Header = new CNetDataHeader(InterfaceType.World, (int)WorldMessageType.CNetCreateMatchWorldGroupNotification, false);
            IdGroup = default(int);
            GroupLeader = default(CNetWorldPlayerVO);
            Players = default(CNetWorldPlayerVO[]);
        }
    }
}
