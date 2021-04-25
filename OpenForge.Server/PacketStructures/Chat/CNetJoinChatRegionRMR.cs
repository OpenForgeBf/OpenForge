// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketHandlers;

namespace OpenForge.Server.PacketStructures.Chat
{
    [InterfaceType(InterfaceType.Chat)]
    public class CNetJoinChatRegionRMR
    {
        public CNetDataHeader Header { get; set; }
        public int Status { get; set; }
        public long ChannelId { get; set; }
        public int ChannelNumber { get; set; }
        public int ChatServerId { get; set; }
        public CNetWorldPlayerVO[] Players { get; set; }

        public CNetJoinChatRegionRMR(bool defaultHeader)
        {
            Header = new CNetDataHeader(InterfaceType.Chat, (int)ChatMessageType.CNetJoinChatRegionRMR, true);
            Status = default(int);
            ChannelId = default(long);
            ChannelNumber = default(int);
            ChatServerId = default(int);
            Players = default(CNetWorldPlayerVO[]);
        }
    }
}
