// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NLog;
using OpenForge.Server.Database.Memory;
using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketStructures.Chat;

namespace OpenForge.Server
{
    public class ChatChannel
    {
        private static readonly ConcurrentDictionary<ulong, ChatChannel> s_chatChannels = new();
        private static readonly IndexManager[] s_chatChannelIndexManagers = new IndexManager[(int)ChatChannelType.Max];

        private readonly List<Player> _members = new();

        static ChatChannel()
        {
            for (var i = 0; i < s_chatChannelIndexManagers.Length; i++)
            {
                s_chatChannelIndexManagers[i] = new IndexManager();
            }
        }

        public ChatChannel(ulong id)
        {
            Id = id;
        }

        public ReadOnlyCollection<Player> Members
        {
            get
            {
                lock (_members)
                {
                    return _members.ToList().AsReadOnly();
                }
            }
        }

        public ChatChannelType Type => (ChatChannelType)(Id & 0xF);
        public ulong Id { get; }

        private static Logger Logger { get; } = LogManager.GetCurrentClassLogger();

        public static ChatChannel CreateOrJoin(ulong id, Player player)
        {
            var chatChannel = GetOrCreate(id);
            chatChannel.Add(player);
            return chatChannel;
        }

        public static ChatChannel CreateOrJoin(ChatChannelType type, ulong index, Player player)
        {
            var typeAsByte = (byte)type;

            if (typeAsByte > 0xF)
            {
                throw new ArgumentOutOfRangeException($"Type index is limited between 0 and 15 (0xF)");
            }

            var id = typeAsByte | index << 4;
            return CreateOrJoin(id, player);
        }

        public static ChatChannel Create(ChatChannelType type)
        {
            var typeAsByte = (byte)type;

            if (typeAsByte > 0xF)
            {
                throw new ArgumentOutOfRangeException($"Type index is limited between 0 and 15 (0xF)");
            }

            var index = s_chatChannelIndexManagers[typeAsByte].NewIndex();
            var id = typeAsByte | index << 4;

            return GetOrCreate(id);
        }

        public static ChatChannel Create(ChatChannelType type, params Player[] players)
        {
            var chatChannel = Create(type);

            foreach (var player in players)
            {
                chatChannel.Add(player);
            }

            return chatChannel;
        }

        public static void RemovePlayerFromAllChannels(Player player)
        {
            var channelsWithPlayer = s_chatChannels.Values.Where(chatChannel => chatChannel.Members.Contains(player)).ToArray();

            foreach (var channelWithPlayer in channelsWithPlayer)
            {
                channelWithPlayer.Remove(player);
            }
        }

        public static void Remove(ulong id)
        {
            if (!s_chatChannels.TryGetValue(id, out var chatChannel))
            {
                return;
            }

            foreach (var member in chatChannel.Members)
            {
                try
                {
                    chatChannel.Remove(member);
                }
                catch (Exception e)
                {
                    Logger.Warn(e, $"Failed to remove member '{member.Name}' from channel {id}.");
                }
            }

            s_chatChannels.TryRemove(id, out var _);
            Logger.Info($"Removed channel {id}.");
        }

        public static void Leave(ulong id, Player player)
        {
            if (!s_chatChannels.TryGetValue(id, out var chatChannel))
            {
                Logger.Warn($"Failed to send leave channel {id} because the channel does not exist.");
                return;
            }

            chatChannel.Remove(player);
        }

        public static void Say(ulong id, Player sender, string message, string language)
        {
            if (!s_chatChannels.TryGetValue(id, out var chatChannel))
            {
                Logger.Warn($"Failed to send '[{id}] {sender.Name}: {message}' because the channel does not exist.");
                return;
            }

            chatChannel.Say(sender, message, language);
        }

        public static void Roll(ulong id, Player sender, int minValue, int maxValue)
        {
            if (!s_chatChannels.TryGetValue(id, out var chatChannel))
            {
                Logger.Warn($"Failed to send roll because the channel does not exist.");
                return;
            }

            chatChannel.Roll(sender, minValue, maxValue);
        }

        public void Say(Player sender, string message, string language)
        {
            Logger.Info($"[{Id}] {sender.Name}: {message}");

            Send(new CNetSayNotification(true)
            {
                Player = sender.GetWorldPlayer(),
                ChannelId = Id,
                ChatServerId = 1,
                Message = message,
                Language = language,
                SentenceBlockedForSeconds = 0
            });
        }

        public void Roll(Player sender, int minValue, int maxValue)
        {
            var rollResult = new Random(Environment.TickCount).Next(minValue, maxValue);
            Logger.Info($"[{Id}] {sender.Name}: Roll {rollResult} ({minValue}/{maxValue})");

            Send(new CNetRollDiceNotification(true)
            {
                ChannelId = Id,
                Player = sender.GetWorldPlayer(),
                MinValue = minValue,
                MaxValue = maxValue,
                RollResult = rollResult
            });
        }

        public void Add(Player player)
        {
            bool added;

            lock (_members)
            {
                added = !_members.Contains(player);
                if (added)
                {
                    _members.Add(player);
                }
            }

            if (added)
            {
                Logger.Info($"{player.Name} joined channel '{Id}'.");

                if (Type == ChatChannelType.Region)
                {
                    Send(new CNetJoinChatRegionChannelNotification(true)
                    {
                        ChannelId = Id,
                        Player = player.GetWorldPlayer(),
                        ChatServerId = 1
                    });
                }
            }
        }

        public void Remove(Player player)
        {
            bool removed;

            lock (_members)
            {
                removed = _members.Remove(player);
            }

            if (removed)
            {
                Logger.Info($"{player.Name} left channel '{Id}'.");

                if (Type == ChatChannelType.Region)
                {
                    Send(new CNetLeaveChatRegionChannelNotification(true)
                    {
                        ChannelId = Id,
                        ChatServerId = 1,
                        PlayerId = player.ID
                    });
                }
            }
        }

        private static ChatChannel GetOrCreate(ulong id)
        {
            if (!s_chatChannels.TryGetValue(id, out var chatChannel))
            {
                Logger.Info($"Created new channel {id} because the channel does not exist.");

                chatChannel = new ChatChannel(id);
                s_chatChannels[id] = chatChannel;
            }

            return chatChannel;
        }

        private void Send(object obj)
        {
            foreach (var member in Members)
            {
                try
                {
                    Logger.Trace($"Sending message in channel {Id} to '{member.Name}'.");
                    member.Send(obj);
                }
                catch (Exception e)
                {
                    Logger.Error(e, $"Failed to send channel message to '{member.Name}'.");
                }
            }
        }
    }
}
