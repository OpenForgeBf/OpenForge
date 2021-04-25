// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketHandlers;

namespace OpenForge.Server.PacketStructures.Chat
{
    [InterfaceType(InterfaceType.Chat)]
    public class CNetRollDiceNotification
    {
        public CNetDataHeader Header { get; set; }
        public long ChannelId { get; set; }
        public CNetWorldPlayerVO Player { get; set; }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
        public int RollResult { get; set; }

        public CNetRollDiceNotification(bool defaultHeader)
        {
            Header = new CNetDataHeader(InterfaceType.Chat, (int)ChatMessageType.CNetRollDiceNotification, false);
            ChannelId = default(long);
            Player = default(CNetWorldPlayerVO);
            MinValue = default(int);
            MaxValue = default(int);
            RollResult = default(int);
        }
    }
}
