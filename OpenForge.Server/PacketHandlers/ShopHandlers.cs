// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using NLog;
using OpenForge.Server.PacketStructures;
using OpenForge.Server.PacketStructures.Shop;

namespace OpenForge.Server.PacketHandlers
{
    public static class ShopHandlers
    {
        public static Logger Logger = LogManager.GetCurrentClassLogger();

        public static CNetAddAuctionToWatchListRMR AddAuctionToWatchListRMC(Session session, CNetAddAuctionToWatchListRMC data)
        {
            return new CNetAddAuctionToWatchListRMR(true)
            {
                Status = 1
            };
        }

        public static CNetBidAuctionRMR BidAuctionRMC(Session session, CNetBidAuctionRMC data)
        {
            return new CNetBidAuctionRMR(true)
            {
                Status = 1
            };
        }

        public static CNetBuyoutAuctionRMR BuyoutAuctionRMC(Session session, CNetBuyoutAuctionRMC data)
        {
            return new CNetBuyoutAuctionRMR(true)
            {
                Status = 1
            };
        }

        public static CNetBuyProductRMR BuyProductRMC(Session session, CNetBuyProductRMC data)
        {
            return new CNetBuyProductRMR(true)
            {
                Status = 1,
                Price = 0,
            };
        }

        public static CNetCancelAuctionRMR CancelAuctionRMC(Session session, CNetCancelAuctionRMC data)
        {
            return new CNetCancelAuctionRMR(true)
            {
                Status = 1
            };
        }

        public static CNetCreateAuctionRMR CreateAuctionRMC(Session session, CNetCreateAuctionRMC data)
        {
            return new CNetCreateAuctionRMR(true)
            {
                Status = 1
            };
        }

        public static CNetGetAllAuctionsRMR GetAllAuctionsRMC(Session session, CNetGetAllAuctionsRMC data)
        {
            return new CNetGetAllAuctionsRMR(true)
            {
                Status = 0,
                Auctions = new CNetAuctionVO[0],
                OverallSearchResultCount = 0,
            };
        }

        public static CNetGetAllBoosterRMR GetAllBoosterRMC(Session session, CNetGetAllBoosterRMC data)
        {
            return new CNetGetAllBoosterRMR(true)
            {
                Status = 0,
                Boosters = new CNetBoosterVO[0]
            };
        }

        public static CNetGetAllMyAuctionsRMR GetAllMyAuctionsRMC(Session session, CNetGetAllMyAuctionsRMC data)
        {
            return new CNetGetAllMyAuctionsRMR(true)
            {
                Status = 0,
                Auctions = new CNetAuctionVO[0]
            };
        }

        public static CNetGetAllProductsRMR GetAllProductsRMC(Session session, CNetGetAllProductsRMC data)
        {
            return new CNetGetAllProductsRMR(true)
            {
                Status = 0,
                IsEligibleForFreeBooster = false,
                Products = new CNetProductVO[0]
            };
        }

        public static CNetGetAuthenticatedLockboxURLRMR GetAuthenticatedLockboxURLRMC(Session session, CNetGetAuthenticatedLockboxURLRMC data)
        {
            return new CNetGetAuthenticatedLockboxURLRMR(true)
            {
                Status = 0,
                AuthenticatedLockboxURL = null
            };
        }

        public static CNetGetAuthenticatedManagedOffersURLRMR GetAuthenticatedManagedOffersURLRMC(Session session, CNetGetAuthenticatedManagedOffersURLRMC data)
        {
            return new CNetGetAuthenticatedManagedOffersURLRMR(true)
            {
                Status = 0,
                AuthenticatedManagedOffersURL = null
            };
        }

        public static CNetGetMyWatchListRMR GetMyWatchListRMC(Session session, CNetGetMyWatchListRMC data)
        {
            return new CNetGetMyWatchListRMR(true)
            {
                Status = 0,
                Auctions = new CNetAuctionVO[0]
            };
        }

        public static CNetRemoveAuctionFromWatchListRMR RemoveAuctionFromWatchListRMC(Session session, CNetRemoveAuctionFromWatchListRMC data)
        {
            return new CNetRemoveAuctionFromWatchListRMR(true)
            {
                Status = 1
            };
        }

        public static CNetRetrieveAvatarDisponibilityRMR RetrieveAvatarDisponibilityRMC(Session session, CNetRetrieveAvatarDisponibilityRMC data)
        {
            return new CNetRetrieveAvatarDisponibilityRMR(true)
            {
                Status = 0,
                IsAvatarAvailable = true,
                Price = 0
            };
        }
    }
}
