// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace OpenForge.Coordinator.Packets
{
    [CoordinatorPacket((ushort)CoordinatorPackets.GetPlayersRequest)]
    public class GetPlayersRequest : CoordinatorPacket
    {
    }

    [CoordinatorPacket((ushort)CoordinatorPackets.GetPlayersResponse)]
    public class GetPlayersResponse : CoordinatorPacket
    {
        public List<string> Players { get; set; }
    }
}
