// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using System;
using NLog;
using OpenForge.Server.Database.Memory;
using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketStructures;
using OpenForge.Server.PacketStructures.World;

namespace OpenForge.Server.PacketHandlers
{
    public static class WorldHandlers
    {
        public static Logger Logger = LogManager.GetCurrentClassLogger();

        public static CNetAcceptDirectTradeRMR AcceptDirectTradeRMC(Session session, CNetAcceptDirectTradeRMC data)
        {
            return new CNetAcceptDirectTradeRMR(true)
            {
                Status = 1,
                IdDirectTrade = 0,
                TradePartner = default
            };
        }

        public static CNetAcceptGroupInviteRMR AcceptGroupInviteRMC(Session session, CNetAcceptGroupInviteRMC data)
        {
            var leader = Player.GetPlayerByID((ulong)data.IdGroupLeader);

            var group = Group.GetOrCreate(leader);
            group.AddMember(session.Player);

            leader.GetSession().Send(new CNetPlayerAcceptGroupInviteNotification(true)
            {
                IdGroupLeader = (long)leader.ID,
                Player = session.Player.GetWorldPlayer()
            });
            return new CNetAcceptGroupInviteRMR(true)
            {
                Status = 1,
                Group = group.GetWorldGroup()
            };
        }

        public static CNetAcceptRequestDeckRMR AcceptRequestDeckRMC(Session session, CNetAcceptRequestDeckRMC data)
        {
            return new CNetAcceptRequestDeckRMR(true)
            {
                Status = 1
            };
        }

        public static CNetAcceptShowDeckRMR AcceptShowDeckRMC(Session session, CNetAcceptShowDeckRMC data)
        {
            return new CNetAcceptShowDeckRMR(true)
            {
                Status = 1
            };
        }

        public static CNetAddBoosterToDirectTradeRMR AddBoosterToDirectTradeRMC(Session session, CNetAddBoosterToDirectTradeRMC data)
        {
            return new CNetAddBoosterToDirectTradeRMR(true)
            {
                Status = 1
            };
        }

        public static CNetAddCardToDirectTradeRMR AddCardToDirectTradeRMC(Session session, CNetAddCardToDirectTradeRMC data)
        {
            return new CNetAddCardToDirectTradeRMR(true)
            {
                Status = 1
            };
        }

        public static CNetAddFriendRMR AddFriendRMC(Session session, CNetAddFriendRMC data)
        {
            return new CNetAddFriendRMR(true)
            {
                Status = 1,
                FriendList = new CNetWorldPlayerVO[0]
            };
        }

        public static CNetAddIgnoreCharacterRMR AddIgnoreCharacterRMC(Session session, CNetAddIgnoreCharacterRMC data)
        {
            return new CNetAddIgnoreCharacterRMR(true)
            {
                Status = 1,
                IgnoreList = new CNetWorldPlayerVO[0]
            };
        }

        public static CNetBeginnDirectTradeRMR BeginnDirectTradeRMC(Session session, CNetBeginnDirectTradeRMC data)
        {
            return new CNetBeginnDirectTradeRMR(true)
            {
                Status = 1
            };
        }

        public static CNetCancelDirectTradeRMR CancelDirectTradeRMC(Session session, CNetCancelDirectTradeRMC data)
        {
            return new CNetCancelDirectTradeRMR(true)
            {
                Status = 1,
            };
        }

        public static CNetChangeCharacterLocationStateRMR ChangeCharacterLocationStateRMC(Session session, CNetChangeCharacterLocationStateRMC data)
        {
            /*
            Group group = session.Player.GetActiveGroup();
            if (group == null)
                throw new NullReferenceException("Player game without group?");
            Group.Match match = group.OngoingMatch;
            if (group == null)
                throw new NullReferenceException("Group without match?"); */

            /*
            if (data.Location != LocationType.Ingame && data.Location != LocationType.LoadingScreen)
            {
                //match.CurrentMatch = null;

                /*
                session.Send(new CNetGroupDestroyedNotification(true)
                {
                    IdGroup = match.CurrentGroupIndex,
                    VersionId = 1,
                });
            }*/

            var lastLocation = session.Player.Location;
            session.Player.Location = data.Location;
            session.Player.State = data.State;
            session.Player.Map = data.Map;
            var group = session.Player.GetActiveGroup();

            if (group != null)
            {
                Logger.Info($"{session.Player.Name}: {Enum.GetName(typeof(LocationType), session.Player.Location)}, {session.Player.Map}");
                group.NotifyLocation(session.Player);

                if (lastLocation == LocationType.PostgameStatistics && group.Players.Count == 0)
                {
                    group.Disband();
                }
            }

            return new CNetChangeCharacterLocationStateRMR(true)
            {
                Status = 0
            };
        }

        public static CNetChangeTradeMoneyAmountRMR ChangeTradeMoneyAmountRMC(Session session, CNetChangeTradeMoneyAmountRMC data)
        {
            return new CNetChangeTradeMoneyAmountRMR(true)
            {
                Status = 1
            };
        }

        public static CNetDeclineDirectTradeRMR DeclineDirectTradeRMC(Session session, CNetDeclineDirectTradeRMC data)
        {
            return new CNetDeclineDirectTradeRMR(true)
            {
                Status = 1
            };
        }

        public static void DeclineGroupInviteAction(Session session, CNetDeclineGroupInviteAction data)
        {
            var player = Player.GetPlayerByID((ulong)data.IdGroupLeader);

            player.Send(new CNetDeclineGroupInviteNotification(true)
            {
                Decliner = session.Player.GetWorldPlayer()
            });
        }

        public static CNetDeclineRequestDeckRMR DeclineRequestDeckRMC(Session session, CNetDeclineRequestDeckRMC data)
        {
            return new CNetDeclineRequestDeckRMR(true)
            {
                Status = 1
            };
        }

        public static CNetDistributeValueToGroupMembersRMR DistributeValueToGroupMembersRMC(Session session, CNetDistributeValueToGroupMembersRMC data)
        {
            var group = Group.GetGroup(data.IdGroup);

            if (group != null)
            {
                group.SendToMembers(new CNetDistributeValueToGroupMembersNotification(true)
                {
                    IdGroup = group.ID,
                    Key = data.Key,
                    Value = data.Value
                }, session.Player);
            }

            return new CNetDistributeValueToGroupMembersRMR(true)
            {
                Status = 0
            };
        }

        public static CNetGetFriendListRMR GetFriendListRMC(Session session, CNetGetFriendListRMC data)
        {
            return new CNetGetFriendListRMR(true)
            {
                Status = 0,
                FriendList = new CNetWorldPlayerVO[0]
            };
        }

        public static CNetGetIgnoreListRMR GetIgnoreListRMC(Session session, CNetGetIgnoreListRMC data)
        {
            return new CNetGetIgnoreListRMR(true)
            {
                Status = 0,
                IgnoreList = new CNetWorldPlayerVO[0]
            };
        }

        public static CNetGetPlayerInfoRMR GetPlayerInfoRMC(Session session, CNetGetPlayerInfoRMC data)
        {
            //var isCurrentPlayer = data.IdCharacter == (long)session.Player.ID;

            var player = Player.GetPlayerByID((ulong)data.IdCharacter);
            var status = player != null ? 0 : 1;
            var wp = player?.GetWorldPlayer() ?? new CNetWorldPlayerVO();

            return new CNetGetPlayerInfoRMR(true)
            {
                Status = status,
                Player = wp
            };
        }

        public static CNetGetRankingPvEByCharacterRMR GetRankingPvEByCharacterRMC(Session session, CNetGetRankingPvEByCharacterRMC data)
        {
            var player = session.Player;

            return new CNetGetRankingPvEByCharacterRMR(true)
            {
                Rank = new CNetRankPvEVO()
                {
                    Id = (long)player.ID,
                    Difficulty = 0,
                    HiddenCharDecks = new long[0],
                    InnerRank = 0,
                    MatchTime = 0,
                    Rank = 0,
                    RecordGuid = 0,
                    TeamID = 0,
                    Players = new CNetWorldPlayerVO[] { player.GetWorldPlayer() }
                },
                Status = 0
            };
        }

        public static CNetGetRankingPvEByRangeRMR GetRankingPvEByRangeRMC(Session session, CNetGetRankingPvEByRangeRMC data)
        {
            var player = session.Player;

            return new CNetGetRankingPvEByRangeRMR(true)
            {
                Status = 0,
                Ranks = new CNetRankPvEVO[]
                {
                    new CNetRankPvEVO()
                    {
                        Id = (long)player.ID,
                        Difficulty = 0,
                        HiddenCharDecks = new long[0],
                        InnerRank = 0,
                        MatchTime = 0,
                        Rank = 0,
                        RecordGuid = 0,
                        TeamID = 0,
                        Players = new CNetWorldPlayerVO[] { player.GetWorldPlayer() }
                   }
                },
                TotalRanksAmount = 0,
            };
        }

        public static CNetGetRankingPvP1vs1ByCharacterRMR GetRankingPvP1vs1ByCharacterRMC(Session session, CNetGetRankingPvP1vs1ByCharacterRMC data)
        {
            var player = session.Player;

            return new CNetGetRankingPvP1vs1ByCharacterRMR(true)
            {
                Status = 0,
                Rank = new CNetRankPvPVO()
                {
                    Id = (long)player.ID,
                    Activity = 0,
                    Bonus = 0,
                    EloRatingUnlockPercentage = 0,
                    EloScore = 0,
                    Players = new CNetWorldPlayerVO[] { player.GetWorldPlayer() },
                    Losses = 0,
                    MatchCountLast24h = 0,
                    Rank = 0,
                    Rating = 0,
                    TeamDeletionDate = new CNetBattleForgeDateTimeVO(DateTime.Now),
                    TeamID = 0,
                    Victories = 0
                }
            };
        }

        public static CNetGetRankingPvP1vs1ByRangeRMR GetRankingPvP1vs1ByRangeRMC(Session session, CNetGetRankingPvP1vs1ByRangeRMC data)
        {
            var player = session.Player;

            return new CNetGetRankingPvP1vs1ByRangeRMR(true)
            {
                Status = 0,
                Ranks = new CNetRankPvPVO[]
                {
                    new CNetRankPvPVO()
                    {
                        Id = (long)player.ID,
                        Activity = 0,
                        Bonus = 0,
                        EloRatingUnlockPercentage = 0,
                        EloScore = 0,
                        Players = new CNetWorldPlayerVO[] { player.GetWorldPlayer() },
                        Losses = 0,
                        MatchCountLast24h = 0,
                        Rank = 0,
                        Rating = 0,
                        TeamDeletionDate = new CNetBattleForgeDateTimeVO(DateTime.Now),
                        TeamID = 0,
                        Victories = 0
                    }
                },
                TotalRanksAmount = 0
            };
        }

        public static CNetGetRankingPvP2vs2ByRangeRMR GetRankingPvP2vs2ByRangeRMC(Session session, CNetGetRankingPvP2vs2ByRangeRMC data)
        {
            var player = session.Player;

            return new CNetGetRankingPvP2vs2ByRangeRMR(true)
            {
                Status = 0,
                Ranks = new CNetRankPvPVO[]
                {
                    new CNetRankPvPVO()
                    {
                        Id = (long)player.ID,
                        Activity = 0,
                        Bonus = 0,
                        EloRatingUnlockPercentage = 0,
                        EloScore = 0,
                        Players = new CNetWorldPlayerVO[] { player.GetWorldPlayer() },
                        Losses = 0,
                        MatchCountLast24h = 0,
                        Rank = 0,
                        Rating = 0,
                        TeamDeletionDate = new CNetBattleForgeDateTimeVO(DateTime.Now),
                        TeamID = 0,
                        Victories = 0
                    }
                },
                TotalRanksAmount = 0
            };
        }

        public static CNetGetRankingPvP2vs2TopTeamByCharacterRMR GetRankingPvP2vs2TopTeamByCharacterRMC(Session session, CNetGetRankingPvP2vs2TopTeamByCharacterRMC data)
        {
            var player = session.Player;

            return new CNetGetRankingPvP2vs2TopTeamByCharacterRMR(true)
            {
                Status = 0,
                Rank = new CNetRankPvPVO()
                {
                    Id = (long)player.ID,
                    Activity = 0,
                    Bonus = 0,
                    EloRatingUnlockPercentage = 0,
                    EloScore = 0,
                    Players = new CNetWorldPlayerVO[] { player.GetWorldPlayer() },
                    Losses = 0,
                    MatchCountLast24h = 0,
                    Rank = 0,
                    Rating = 0,
                    TeamDeletionDate = new CNetBattleForgeDateTimeVO(DateTime.Now),
                    TeamID = 0,
                    Victories = 0
                }
            };
        }

        public static CNetGetTotalPlayerOnlineCountRMR GetTotalPlayerOnlineCountRMC(Session session, CNetGetTotalPlayerOnlineCountRMC data)
        {
            return new CNetGetTotalPlayerOnlineCountRMR(true)
            {
                PlayerCount = Player.GetOnline().Count,
                Status = 0,
                TrialPlayerCount = 0
            };
        }

        public static CNetInvitePlayerToGroupRMR InvitePlayerToGroupRMC(Session session, CNetInvitePlayerToGroupRMC data)
        {
            var target = Player.GetPlayerByID(data.PlayerId);

            target.Send(new CNetInviteToGroupNotification(true)
            {
                Leader = session.Player.GetWorldPlayer()//target.GetWorldPlayer()
            });

            return new CNetInvitePlayerToGroupRMR(true)
            {
                Status = 1
            };
        }

        public static CNetIsReportPlayerEnabledRMR IsReportPlayerEnabledRMC(Session session, CNetIsReportPlayerEnabledRMC data)
        {
            return new CNetIsReportPlayerEnabledRMR(true)
            {
                Status = 0
            };
        }

        public static CNetKickCharacterFromGroupRMR KickCharacterFromGroupRMC(Session session, CNetKickCharacterFromGroupRMC data)
        {
            var group = Group.GetGroup(data.IdGroup);

            var toRemove = Player.GetPlayerByID(data.IdCharacter);
            group.RemoveMember(toRemove);
            toRemove.LeftGroupNotify(group.ID);

            return new CNetKickCharacterFromGroupRMR(true)
            {
                Status = 0
            };
        }

        public static CNetLeaveGroupRMR LeaveGroupRMC(Session session, CNetLeaveGroupRMC data)
        {
            var group = Group.GetGroup(data.IdGroup);

            var toRemove = session.Player;

            group.RemoveMember(toRemove);

            return new CNetLeaveGroupRMR(true)
            {
                Status = 0,
                GroupId = data.IdGroup,
                VersionId = long.MaxValue
            };
        }

        public static CNetRemoveBoosterFromDirectTradeRMR RemoveBoosterFromDirectTradeRMC(Session session, CNetRemoveBoosterFromDirectTradeRMC data)
        {
            return new CNetRemoveBoosterFromDirectTradeRMR(true)
            {
                Status = 1
            };
        }

        public static CNetRemoveCardFromDirectTradeRMR RemoveCardFromDirectTradeRMC(Session session, CNetRemoveCardFromDirectTradeRMC data)
        {
            return new CNetRemoveCardFromDirectTradeRMR(true)
            {
                Status = 1
            };
        }

        public static CNetRemoveFriendRMR RemoveFriendRMC(Session session, CNetRemoveFriendRMC data)
        {
            return new CNetRemoveFriendRMR(true)
            {
                Status = 1,
                FriendList = new CNetWorldPlayerVO[0]
            };
        }

        public static CNetRemoveIgnoreCharacterRMR RemoveIgnoreCharacterRMC(Session session, CNetRemoveIgnoreCharacterRMC data)
        {
            return new CNetRemoveIgnoreCharacterRMR(true)
            {
                Status = 1,
                IgnoreList = new CNetWorldPlayerVO[0]
            };
        }

        public static CNetReportPlayerRMR ReportPlayerRMC(Session session, CNetReportPlayerRMC data)
        {
            return new CNetReportPlayerRMR(true)
            {
                Status = 1
            };
        }

        public static CNetRequestDeckRMR RequestDeckRMC(Session session, CNetRequestDeckRMC data)
        {
            return new CNetRequestDeckRMR(true)
            {
                Status = 1
            };
        }

        public static CNetSearchPlayerRMR SearchPlayerRMC(Session session, CNetSearchPlayerRMC data)
        {
            return new CNetSearchPlayerRMR(true)
            {
                Status = 1,
                Players = new CNetWorldPlayerVO[0]
            };
        }

        public static CNetShowDeckMOTWRMR ShowDeckMOTWRMC(Session session, CNetShowDeckMOTWRMC data)
        {
            return new CNetShowDeckMOTWRMR(true)
            {
                Status = 1
            };
        }

        public static CNetShowDeckRMR ShowDeckRMC(Session session, CNetShowDeckRMC data)
        {
            return new CNetShowDeckRMR(true)
            {
                Status = 1
            };
        }

        public static CNetToggleDirectTradeAcceptStatusRMR ToggleDirectTradeAcceptStatusRMC(Session session, CNetToggleDirectTradeAcceptStatusRMC data)
        {
            return new CNetToggleDirectTradeAcceptStatusRMR(true)
            {
                Status = 1
            };
        }

        public static CNetWhisperMuteUserRMR WhisperMuteUserRMC(Session session, CNetWhisperMuteUserRMC data)
        {
            return new CNetWhisperMuteUserRMR(1);
        }

        public static CNetWhisperRMR WhisperRMC(Session session, CNetWhisperRMC data)
        {
            return new CNetWhisperRMR(true)
            {
                Status = 0
            };
        }
    }
}
