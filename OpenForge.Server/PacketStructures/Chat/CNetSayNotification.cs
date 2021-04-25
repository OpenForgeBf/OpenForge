// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketHandlers;

namespace OpenForge.Server.PacketStructures.Chat
{
    [InterfaceType(InterfaceType.Chat)]
    public class CNetSayNotification
    {
        public CNetDataHeader Header { get; set; }
        public CNetWorldPlayerVO Player { get; set; }
        public long ChannelId { get; set; }
        public int ChatServerId { get; set; }
        public string Message { get; set; }
        public string Language { get; set; }
        public int SentenceBlockedForSeconds { get; set; }

        public CNetSayNotification(bool defaultHeader)
        {
            Header = new CNetDataHeader(InterfaceType.Chat, (int)ChatMessageType.CNetSayNotification, false);
            Player = default(CNetWorldPlayerVO);
            ChannelId = default(long);
            ChatServerId = default(int);
            Message = default(string);
            Language = default(string);
            SentenceBlockedForSeconds = default(int);
        }
    }
}
