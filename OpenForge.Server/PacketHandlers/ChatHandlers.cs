// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketStructures.Chat;

namespace OpenForge.Server.PacketHandlers
{
    public static class ChatHandlers
    {
        public static CNetChatMuteUserRMR ChatMuteUserRMC(Session session, CNetChatMuteUserRMC data)
        {
            return new CNetChatMuteUserRMR(1);
        }

        public static CNetJoinChatChannelRMR JoinChatChannelRMC(Session session, CNetJoinChatChannelRMC data)
        {
            ChatChannel.CreateOrJoin(data.ChannelId, session.Player);

            return new CNetJoinChatChannelRMR(true)
            {
                Status = 0
            };
        }

        public static CNetJoinChatRegionRMR JoinChatRegionRMC(Session session, CNetJoinChatRegionRMC data)
        {
            var chatChannel = ChatChannel.CreateOrJoin((ChatChannelType)data.Type, (ulong)data.MapId, session.Player);

            return new CNetJoinChatRegionRMR(true)
            {
                Status = 0,
                ChannelId = chatChannel.Id,
                ChannelNumber = 1,
                ChatServerId = 1,
                Players = chatChannel.Members.Select(x => x.GetWorldPlayer()).ToArray()
            };
        }

        public static void KickAction(Session session, CNetKickAction data)
        {
        }

        public static CNetLeaveChatChannelRMR LeaveChatChannelRMC(Session session, CNetLeaveChatChannelRMC data)
        {
            ChatChannel.Leave(data.ChannelId, session.Player);

            return new CNetLeaveChatChannelRMR(true)
            {
                Status = 0
            };
        }

        public static void RollDiceAction(Session session, CNetRollDiceAction data)
        {
            var maxValue = data.MaxValue >= data.MinValue ? data.MaxValue : data.MinValue;
            maxValue = Math.Min(maxValue, int.MaxValue - 1);
            ChatChannel.Roll(data.ChannelId, session.Player, data.MinValue, maxValue);
        }

        public static void SayAction(Session session, CNetSayAction data) => ChatChannel.Say(data.ChannelId, session.Player, data.Message, data.Language);

        public static void SystemAction(Session session, CNetSystemAction data)
        {
        }
    }
}
