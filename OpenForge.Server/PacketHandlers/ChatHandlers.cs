// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using NLog;
using OpenForge.Server.Database.Memory;
using OpenForge.Server.PacketStructures.Chat;

namespace OpenForge.Server.PacketHandlers
{
    public static class ChatHandlers
    {
        public static Logger Logger = LogManager.GetCurrentClassLogger();

        public static CNetChatMuteUserRMR ChatMuteUserRMC(Session session, CNetChatMuteUserRMC data)
        {
            return new CNetChatMuteUserRMR(1);
        }

        public static CNetJoinChatChannelRMR JoinChatChannelRMC(Session session, CNetJoinChatChannelRMC data)
        {
            return new CNetJoinChatChannelRMR(true)
            {
                Status = 0
            };
        }

        public static CNetJoinChatRegionRMR JoinChatRegionRMC(Session session, CNetJoinChatRegionRMC data)
        {
            var channelId = 1;//(long)data.Type | (data.MapId << 4);

            Player.PlayerJoinedChannel(session.Player);

            return new CNetJoinChatRegionRMR(true)
            {
                Status = 0,
                ChannelId = channelId,
                ChannelNumber = 0,
                ChatServerId = 0,
                Players = Player.GetOnline().Select(x => x.GetWorldPlayer()).ToArray()
            };
        }

        public static void KickAction(Session session, CNetKickAction data)
        {
        }

        public static CNetLeaveChatChannelRMR LeaveChatChannelRMC(Session session, CNetLeaveChatChannelRMC data)
        {
            Player.PlayerLeftChannel(session.Player);
            return new CNetLeaveChatChannelRMR(true)
            {
                Status = 0
            };
        }

        public static void RollDiceAction(Session session, CNetRollDiceAction data)
        {
            var maxValue = data.MaxValue >= data.MinValue ? data.MaxValue : data.MinValue;
            maxValue = Math.Min(maxValue, int.MaxValue - 1);

            session.Send(new CNetRollDiceNotification(true)
            {
                ChannelId = data.ChannelId,
                Player = session.Player.GetWorldPlayer(),
                MinValue = data.MinValue,
                MaxValue = maxValue,
                RollResult = new Random(Environment.TickCount).Next(data.MinValue, maxValue)
            });
        }

        public static void SayAction(Session session, CNetSayAction data)
        {
            session.Send(new CNetSayNotification(true)
            {
                Player = session.Player.GetWorldPlayer(),
                ChannelId = data.ChannelId,
                ChatServerId = 0,
                Message = data.Message,
                Language = data.Language,
                SentenceBlockedForSeconds = 0
            });
        }

        public static void SystemAction(Session session, CNetSystemAction data)
        {
        }
    }
}
