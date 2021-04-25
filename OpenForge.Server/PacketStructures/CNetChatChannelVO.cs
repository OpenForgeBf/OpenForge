// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

namespace OpenForge.Server.PacketStructures
{
    public class CNetChatChannelVO
    {
        public long Id { get; set; }
        public int IdChannel { get; set; }
        public int Type { get; set; }
        public string ChannelName { get; set; }
        public int IdServer { get; set; }
    }
}
