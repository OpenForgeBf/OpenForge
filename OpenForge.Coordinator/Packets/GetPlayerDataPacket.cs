// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using OpenForge.Server.Database.Memory;

namespace OpenForge.Coordinator.Packets
{
    [CoordinatorPacket((ushort)CoordinatorPackets.SyncPlayerRequest)]
    public class GetPlayerDataRequest : CoordinatorPacket
    {
    }

    [CoordinatorPacket((ushort)CoordinatorPackets.SyncPlayerResponse)]
    public class GetPlayerDataResponse : CoordinatorPacket
    {
        public Player Player { get; set; }
    }
}
