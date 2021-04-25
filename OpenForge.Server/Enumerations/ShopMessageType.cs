// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

namespace OpenForge.Server.Enumerations
{
    public enum ShopMessageType : byte
    {
        CNetAddAuctionToWatchListRMC = 0xA,
        CNetAddAuctionToWatchListRMR = 0x1C,
        CNetBidAuctionRMC = 0xC,
        CNetBidAuctionRMR = 0x1E,
        CNetBlockIPRMC = 0x7,
        CNetBlockIPRMR = 0x19,
        CNetBuyBoosterAmountRMC = 0x14,
        CNetBuyBoosterAmountRMR = 0x26,
        CNetBuyoutAuctionRMC = 0xD,
        CNetBuyoutAuctionRMR = 0x1F,
        CNetBuyProductRMC = 0x12,
        CNetBuyProductRMR = 0x24,
        CNetBuyTomeRMC = 0x13,
        CNetBuyTomeRMR = 0x25,
        CNetCancelAuctionRMC = 0x8,
        CNetCancelAuctionRMR = 0x1A,
        CNetCreateAuctionRMC = 0x10,
        CNetCreateAuctionRMR = 0x22,
        CNetGetAllAuctionsRMC = 0xF,
        CNetGetAllAuctionsRMR = 0x21,
        CNetGetAllBoosterRMC = 0x15,
        CNetGetAllBoosterRMR = 0x27,
        CNetGetAllMyAuctionsRMC = 0xE,
        CNetGetAllMyAuctionsRMR = 0x20,
        CNetGetAllProductsRMC = 0x11,
        CNetGetAllProductsRMR = 0x23,
        CNetGetAuthenticatedLockboxURLRMC = 0x6,
        CNetGetAuthenticatedLockboxURLRMR = 0x18,
        CNetGetAuthenticatedManagedOffersURLRMC = 0x5,
        CNetGetAuthenticatedManagedOffersURLRMR = 0x17,
        CNetGetMyWatchListRMC = 0xB,
        CNetGetMyWatchListRMR = 0x1D,
        CNetGetPlayerCredentialsSMR = 0x3,
        CNetMailNotification = 0x2,
        CNetMuteAccountAction = 0x1,
        CNetRemoveAuctionFromWatchListRMC = 0x9,
        CNetRemoveAuctionFromWatchListRMR = 0x1B,
        CNetRetrieveAvatarDisponibilityRMC = 0x4,
        CNetRetrieveAvatarDisponibilityRMR = 0x16,
    }
}
