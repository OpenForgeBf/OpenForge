// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using NLog;
using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketStructures;
using OpenForge.Server.PacketStructures.PreGame;

namespace OpenForge.Server.PacketHandlers
{
    public static class PreGameHandlers
    {
        public static Logger Logger = LogManager.GetCurrentClassLogger();

        public static CNetActivateScratchCardForCharacterRMR ActivateScratchCardForCharacterRMC(Session session, CNetActivateScratchCardForCharacterRMC data)
        {
            return new CNetActivateScratchCardForCharacterRMR(true)
            {
                Status = 0,
                Boosters = null,
                BFPoints = 0,
                PromoCards = null,
                DefaultDecks = null,
                DefaultRandomDeck = 0,
                BFGameAcces = false,
                TomeID = 0,
                VictoryTokens = 0,
                BattleTokens = 0,
                HonorTokens = 0,
                Gold = 0,
                ElementOfCreationTimeInDays = 0,
                ElementOfConversionTimeInDays = 0,
                ElementOfConcealmentTimeInDays = 0,
            };
        }

        public static CNetBuyUpgradeRMR BuyUpgradeRMC(Session session, CNetBuyUpgradeRMC data)
        {
            return new CNetBuyUpgradeRMR(true)
            {
                Status = 0
            };
        }

        public static CNetCombineCardUpgradesRMR CombineCardUpgradesRMC(Session session, CNetCombineCardUpgradesRMC data)
        {
            return new CNetCombineCardUpgradesRMR(true)
            {
                Status = 0
            };
        }

        public static CNetCreateCharacterRMR CreateCharacterRMC(Session session, CNetCreateCharacterRMC data)
        {
            return new CNetCreateCharacterRMR(true)
            {
                Status = 0,
                Success = true,
                WorldCharacter = new CNetWorldPlayerVO(),
                AutostartTutorialMap = false
            };
        }

        public static CNetCreateNewLimitedPoolRMR CreateNewLimitedPoolRMC(Session session, CNetCreateNewLimitedPoolRMC data)
        {
            return new CNetCreateNewLimitedPoolRMR(true)
            {
                Status = 0,
                IdNewLimitedPool = 0,
            };
        }

        public static CNetDeleteDeckForCharacterRMR DeleteDeckForCharacterRMC(Session session, CNetDeleteDeckForCharacterRMC data)
        {
            session.Player.RemoveDeck(data.IdDeck);

            return new CNetDeleteDeckForCharacterRMR(true)
            {
                Status = 0,
                Success = true
            };
        }

        public static CNetDeleteIngameMailRMR DeleteIngameMailRMC(Session session, CNetDeleteIngameMailRMC data)
        {
            return new CNetDeleteIngameMailRMR(true)
            {
                Status = 0,
                BFPCollected = 1
            };
        }

        public static CNetDisbandLimitedPoolRMR DisbandLimitedPoolRMC(Session session, CNetDisbandLimitedPoolRMC data)
        {
            return new CNetDisbandLimitedPoolRMR(true)
            {
                Status = 0,
                IsLegacyTomePool = false
            };
        }

        public static CNetDoesServerKnowUsergeneratedMapRMR DoesServerKnowUsergeneratedMapRMC(Session session, CNetDoesServerKnowUsergeneratedMapRMC data)
        {
            return new CNetDoesServerKnowUsergeneratedMapRMR(true)
            {
                Status = 0,
                ServerKnows = true,
                AllowedToUpload = false,
            };
        }

        public static CNetDownloadUserGeneratedMapFileChunkRMR DownloadUserGeneratedMapFileChunkRMC(Session session, CNetDownloadUserGeneratedMapFileChunkRMC data)
        {
            return new CNetDownloadUserGeneratedMapFileChunkRMR(true)
            {
                Status = 1,
                ChunkBytes = default
            };
        }

        public static CNetGetAllActiveBoostItemsByCharacterRMR GetAllActiveBoostItemsByCharacterRMC(Session session, CNetGetAllActiveBoostItemsByCharacterRMC data)
        {
            return new CNetGetAllActiveBoostItemsByCharacterRMR(true)
            {
                Status = 0,
                BoostItems = null,
            };
        }

        public static CNetGetAllBoostersForCharacterRMR GetAllBoostersForCharacterRMC(Session session, CNetGetAllBoostersForCharacterRMC data)
        {
            return new CNetGetAllBoostersForCharacterRMR(true)
            {
                Status = 0,
                Boosters = new CNetBoosterVO[0]
            };
        }

        public static CNetGetAllCardsForCharacterCompressedRMR GetAllCardsForCharacterCompressedRMC(Session session, CNetGetAllCardsForCharacterCompressedRMC data)
        {
            return new CNetGetAllCardsForCharacterCompressedRMR(true)
            {
                Status = 0,
                PoolCards = new CNetPoolCardCollectionVO[]
                {
                    CNetPoolCardCollectionVO.FromCards(session.Player.Cards.Where(c => (CardPool)c.IdCardPool == CardPool.Collection)),
                    CNetPoolCardCollectionVO.FromCards(session.Player.Cards.Where(c => (CardPool)c.IdCardPool == CardPool.Tutorial))
                }
            };
        }

        public static CNetGetAllCardsForCharacterRMR GetAllCardsForCharacterRMC(Session session, CNetGetAllCardsForCharacterRMC data)
        {
            return new CNetGetAllCardsForCharacterRMR(true)
            {
                Status = 0,
                Cards = session.Player.GetCards()
            };
        }

        public static CNetGetAllCardUpgradesForCharacterCompressedRMR GetAllCardUpgradesForCharacterCompressedRMC(Session session, CNetGetAllCardUpgradesForCharacterCompressedRMC data)
        {
            return new CNetGetAllCardUpgradesForCharacterCompressedRMR(true)
            {
                Status = 0,
                Upgrades = new CNetCardUpgradeCompressedCollectionVO()
                {
                    Index = 1,
                    CardUpgrades = new CNetCardUpgradeCompressedVO[0]
                }
            };
        }

        public static CNetGetAllCardUpgradesForCharacterRMR GetAllCardUpgradesForCharacterRMC(Session session, CNetGetAllCardUpgradesForCharacterRMC data)
        {
            return new CNetGetAllCardUpgradesForCharacterRMR(true)
            {
                Status = 0,
                Upgrades = new CNetCardUpgradeVO[0],
            };
        }

        public static CNetGetAllDecksForCharacterRMR GetAllDecksForCharacterRMC(Session session, CNetGetAllDecksForCharacterRMC data)
        {
            return new CNetGetAllDecksForCharacterRMR(true)
            {
                Status = 0,
                Decks = session.Player.GetDecks()
            };
        }

        public static CNetGetAlllLimitedPoolsForCharacterRMR GetAlllLimitedPoolsForCharacterRMC(Session session, CNetGetAlllLimitedPoolsForCharacterRMC data)
        {
            return new CNetGetAlllLimitedPoolsForCharacterRMR(true)
            {
                Status = 0,
                LimitedPools = null,
            };
        }

        public static CNetGetCharacterStatisticRMR GetCharacterStatisticRMC(Session session, CNetGetCharacterStatisticRMC data)
        {
            return new CNetGetCharacterStatisticRMR(true)
            {
                Status = 0,
                Pve1PlayerWonStandard = 0,
                Pve1PlayerWonAdvanced = 0,
                Pve1PlayerWonExpert = 0,
                Pve2PlayerWonStandard = 0,
                Pve2PlayerWonAdvanced = 0,
                Pve2PlayerWonExpert = 0,
                Pve4PlayerWonStandard = 0,
                Pve4PlayerWonAdvanced = 0,
                Pve4PlayerWonExpert = 0,
                Pve12PlayerWonStandard = 0,
                Pve12PlayerWonAdvanced = 0,
                Pve12PlayerWonExpert = 0,
                Pve1PlayerLooseStandard = 0,
                Pve1PlayerLooseAdvanced = 0,
                Pve1PlayerLooseExpert = 0,
                Pve2PlayerLooseStandard = 0,
                Pve2PlayerLooseAdvanced = 0,
                Pve2PlayerLooseExpert = 0,
                Pve4PlayerLooseStandard = 0,
                Pve4PlayerLooseAdvanced = 0,
                Pve4PlayerLooseExpert = 0,
                Pve12PlayerLooseStandard = 0,
                Pve12PlayerLooseAdvanced = 0,
                Pve12PlayerLooseExpert = 0,
                PvP1vs1Won = 0,
                PvP2vs2Won = 0,
                PvP4vs4Won = 0,
                PvP1vs1Loose = 0,
                PvP2vs2Loose = 0,
                PvP4vs4Loose = 0,
                PvP1vs1LimitedWon = 0,
                PvP2vs2LimitedWon = 0,
                PvP1vs1LimitedLoose = 0,
                PvP2vs2LimitedLoose = 0,
                TotalCards = 0,
                EloRating = 0,
                EloRatingLimited = 0
            };
        }

        public static CNetGetIngameMailRMR GetIngameMailRMC(Session session, CNetGetIngameMailRMC data)
        {
            return new CNetGetIngameMailRMR(true)
            {
                Status = 0,
                Mail = new CNetIngameMailVO[0]
            };
        }

        public static CNetGetRankingsRMR GetRankingsRMC(Session session, CNetGetRankingsRMC data)
        {
            return new CNetGetRankingsRMR(true)
            {
                RankingList = new CNetRankedCharacterVO[0]
            };
        }

        public static CNetGetStoryBookCategoryRMR GetStoryBookCategoryRMC(Session session, CNetGetStoryBookCategoryRMC data)
        {
            return new CNetGetStoryBookCategoryRMR(true)
            {
                Status = 0,
                CategoryEntrys = null
            };
        }

        public static CNetGetUserGeneratedMapsAvailableForDownloadRMR GetUserGeneratedMapsAvailableForDownloadRMC(Session session, CNetGetUserGeneratedMapsAvailableForDownloadRMC data)
        {
            return new CNetGetUserGeneratedMapsAvailableForDownloadRMR(true)
            {
                Status = 0,
                UserGeneratedMaps = null
            };
        }

        public static CNetIsEligibleForFreeTomeRMR IsEligibleForFreeTomeRMC(Session session, CNetIsEligibleForFreeTomeRMC data)
        {
            return new CNetIsEligibleForFreeTomeRMR(true)
            {
                Status = 0,
                IsEligible = 0
            };
        }

        public static CNetOpenBoosterForCharacterRMR OpenBoosterForCharacterRMC(Session session, CNetOpenBoosterForCharacterRMC data)
        {
            return new CNetOpenBoosterForCharacterRMR(true)
            {
                Status = 1,
                Cards = new CNetCardVO[0]
            };
        }

        public static CNetOpenIngameMailRMR OpenIngameMailRMC(Session session, CNetOpenIngameMailRMC data)
        {
            return new CNetOpenIngameMailRMR(true)
            {
                Status = 1,
                BFPCollected = 0
            };
        }

        public static CNetRegisterUserGeneratedMapDownloadRMR RegisterUserGeneratedMapDownloadRMC(Session session, CNetRegisterUserGeneratedMapDownloadRMC data)
        {
            return new CNetRegisterUserGeneratedMapDownloadRMR(true)
            {
                Status = 0,
                OriginalFileName = null,
                FileSize = 0,
                UncompressedFileSize = 0
            };
        }

        public static CNetRenameDeckForCharacterRMR RenameDeckForCharacterRMC(Session session, CNetRenameDeckForCharacterRMC data)
        {
            var deck = session.Player.GetDeck(data.IdDeck);

            if (deck == null)
            {
                return new CNetRenameDeckForCharacterRMR(true)
                {
                    Status = 1
                };
            }

            deck.DeckName = data.NewName;
            session.Player.Update();

            return new CNetRenameDeckForCharacterRMR(true)
            {
                Status = 0
            };
        }

        public static CNetSaveDeckForCharacterRMR SaveDeckForCharacterRMC(Session session, CNetSaveDeckForCharacterRMC data)
        {
            var deck = session.Player.CreateOrSave(data.Deck);
            session.Player.Update();

            return new CNetSaveDeckForCharacterRMR(true)
            {
                Status = 0,
                Success = true,
                NewDeckId = deck.Index,
                NewDeckLevel = deck.DeckLevel
            };
        }

        public static CNetSendIngameMailRMR SendIngameMailRMC(Session session, CNetSendIngameMailRMC data)
        {
            return new CNetSendIngameMailRMR(true)
            {
                Status = 1
            };
        }

        public static CNetSetAvatarRMR SetAvatarRMC(Session session, CNetSetAvatarRMC data)
        {
            session.Player.BannerID = data.CardId;
            session.Player.Update();

            return new CNetSetAvatarRMR(true)
            {
                Status = 0
            };
        }
    }
}
