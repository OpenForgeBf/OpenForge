// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

namespace OpenForge.Server.Enumerations
{
    public enum ChatMessageType : byte
    {
        CNetChatChannelDestroyedNotification = 0x4,
        CNetChatMuteAccountByCharacterAction = 0x1,
        CNetChatMuteUserRMC = 0x13,
        CNetChatMuteUserRMR = 0x19,
        CNetCreateGameChannelSMC = 0x8,
        CNetCreateGroupChannelSMC = 0x6,
        CNetDeleteGameChannelSMC = 0x7,
        CNetDeleteGroupChannelAction = 0x5,
        CNetGetAllGeneralChatChannelsRMC = 0x3,
        CNetGetAllGeneralChatChannelsRMR = 0x18,
        CNetJoinChatChannelRMC = 0x16,
        CNetJoinChatChannelRMR = 0x1C,
        CNetJoinChatRegionChannelNotification = 0x11,
        CNetJoinChatRegionRMC = 0x15,
        CNetJoinChatRegionRMR = 0x1B,
        CNetKickAction = 0xA,
        CNetKickNotification = 0x9,
        CNetLeaveChatChannelRMC = 0x14,
        CNetLeaveChatChannelRMR = 0x1A,
        CNetLeaveChatRegionChannelNotification = 0xF,
        CNetRollDiceAction = 0x12,
        CNetRollDiceNotification = 0x10,
        CNetSayAction = 0xE,
        CNetSayNotification = 0xD,
        CNetSystemAction = 0xC,
        CNetSystemNotification = 0xB,
        CNetUserInChatChannelRMC = 0x2,
        CNetUserInChatChannelRMR = 0x17,
    }
}
