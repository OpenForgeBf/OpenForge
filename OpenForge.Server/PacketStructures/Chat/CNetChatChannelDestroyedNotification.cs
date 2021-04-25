﻿// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketHandlers;

namespace OpenForge.Server.PacketStructures.Chat
{
    [InterfaceType(InterfaceType.Chat)]
    public class CNetChatChannelDestroyedNotification
    {
        public CNetDataHeader Header { get; set; }
        public long IdChannel { get; set; }
        public int IdChatServer { get; set; }

        public CNetChatChannelDestroyedNotification(bool defaultHeader)
        {
            Header = new CNetDataHeader(InterfaceType.Chat, (int)ChatMessageType.CNetChatChannelDestroyedNotification, false);
            IdChannel = default(long);
            IdChatServer = default(int);
        }
    }
}
