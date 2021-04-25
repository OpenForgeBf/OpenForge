// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketHandlers;

namespace OpenForge.Server.PacketStructures.World
{
    [InterfaceType(InterfaceType.World)]
    public class CNetPlayerAcceptGroupInviteNotification
    {
        public CNetDataHeader Header { get; set; }
        public CNetWorldPlayerVO Player { get; set; }
        public long IdGroupLeader { get; set; }

        public CNetPlayerAcceptGroupInviteNotification(bool defaultHeader)
        {
            Header = new CNetDataHeader(InterfaceType.World, (int)WorldMessageType.CNetPlayerAcceptGroupInviteNotification, false);
            Player = default(CNetWorldPlayerVO);
            IdGroupLeader = default(long);
        }
    }
}
