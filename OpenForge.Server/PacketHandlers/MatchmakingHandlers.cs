// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using NLog;
using OpenForge.Server.Database.Memory;
using OpenForge.Server.PacketStructures;
using OpenForge.Server.PacketStructures.Matchmaking;

namespace OpenForge.Server.PacketHandlers
{
    public static class MatchmakingHandlers
    {
        public static Logger Logger = LogManager.GetCurrentClassLogger();

        public static void AcceptRegisterGroupForPvPAutomatchFunDuelAction(Session session, CNetAcceptRegisterGroupForPvPAutomatchFunDuelAction data)
        {
        }

        public static CNetCancelCustomGameRMR CancelCustomGameRMC(Session session, CNetCancelCustomGameRMC data)
        {
            var group = Group.GetGroup(data.IdPreMatch);

            group.StopLobby();

            if (!group.HasMembers || group.Leader == session.Player)
            {
                group.Disband();
            }

            return new CNetCancelCustomGameRMR(true)
            {
                Status = 0
            };
        }

        public static CNetCancelGroupPvPAutomatchRMR CancelGroupPvPAutomatchRMC(Session session, CNetCancelGroupPvPAutomatchRMC data)
        {
            if (data.IsLeader)
            {
                return new CNetCancelGroupPvPAutomatchRMR(true)
                {
                    Status = 0
                };
            }
            else
            {
                return new CNetCancelGroupPvPAutomatchRMR(true)
                {
                    Status = 1
                };
            }
        }

        public static CNetCancelRegisterCharacterForPvPAutomatchFunDuelRMR CancelRegisterCharacterForPvPAutomatchFunDuelRMC(Session session, CNetCancelRegisterCharacterForPvPAutomatchFunDuelRMC data)
        {
            return new CNetCancelRegisterCharacterForPvPAutomatchFunDuelRMR(true)
            {
                Status = 0
            };
        }

        //Others
        public static CNetChangeCustomGameOpenForOtherPlayersStateRMR ChangeCustomGameOpenForOtherPlayersStateRMC(Session session, CNetChangeCustomGameOpenForOtherPlayersStateRMC data)
        {
            return new CNetChangeCustomGameOpenForOtherPlayersStateRMR(true)
            {
                Status = 0
            };
        }

        public static CNetChangeCustomGameSlotRMR ChangeCustomGameSlotRMC(Session session, CNetChangeCustomGameSlotRMC data)
        {
            var group = Group.GetGroup(data.IdPreMatch);

            group.ChangeGameSlot(session.Player, data.Slot, data.IdDeck);
            group.NotifyLobbyChanges();

            return new CNetChangeCustomGameSlotRMR(true)
            {
                Status = 0
            };
        }

        public static CNetCreateCustomGameRMR CreateCustomGameRMC(Session session, CNetCreateCustomGameRMC data)
        {
            var group = Group.GetOrCreate(session.Player);
            session.Player.GroupID = group.ID;

            group.NotifyGroupUpdate();
            group.StartLobby(data);

            var groupPlayers = group.Players;
            for (var i = 0; i < groupPlayers.Count; i++)
            {
                var player = groupPlayers[i];
                group.ChangeGameSlot(player, i, player.ActiveDeck);
            }

            group.NotifyLobbyChanges();

            return new CNetCreateCustomGameRMR(true)
            {
                Status = 0,
                IdLeader = session.Player.ID,
                IdPreMatch = group.Lobby.ID,
                Pvp = data.Pvp
            };
        }

        public static void DeclineRegisterGroupForPvPAutomatchFunDuelAction(Session session, CNetDeclineRegisterGroupForPvPAutomatchFunDuelAction data)
        {
        }

        public static CNetGetAllOpenCustomGamesRMR GetAllOpenCustomGamesRMC(Session session, CNetGetAllOpenCustomGamesRMC data)
        {
            return new CNetGetAllOpenCustomGamesRMR(true)
            {
                Status = 0,
                CustomGames = Group
                    .Where(x => x.Lobby != null && x.OngoingMatch == null && x.Lobby.IsPVP == data.PvP && (data.PvP || x.Lobby.Map.ID == data.IdMap))
                    .Select(x => x.GetCustomGameOverview()).ToArray()
            };
        }

        public static CNetGetFinishedMapsForCharacterRMR GetFinishedMapsForCharacterRMC(Session session, CNetGetFinishedMapsForCharacterRMC data)
        {
            var maps = new long[27] { 67, 10, 45, 8, 57, 22, 9, 18, 29, 26, 20, 24, 44, 32, 60, 21, 37, 19, 25, 74, 98, 100, 35, 56, 88, 84, 99 };
            var mapIdGenerator = 1;

            return new CNetGetFinishedMapsForCharacterRMR(true)
            {
                Status = 0,
                Maps = maps.Select(m => new CNetMapVO() { Id = mapIdGenerator++, MapID = m, Difficulty = 3 }).ToArray(),
            };
        }

        public static CNetJoinOpenCustomGameRMR JoinOpenCustomGameRMC(Session session, CNetJoinOpenCustomGameRMC data)
        {
            var group = Group.GetGroup(data.IdPreMatch);

            if (group == null)
            {
                return new CNetJoinOpenCustomGameRMR(true)
                {
                    Status = 1
                };
            }

            group.AddMember(session.Player);
            group.ChangeGameSlot(session.Player, group.GetAvailableSlotIndex(), data.IdDeck);
            group.NotifyLobbyChanges();

            session.Player.Send(group.GetLobbyCreatedNotification());

            return new CNetJoinOpenCustomGameRMR(true)
            {
                Status = 0,
            };
        }

        public static CNetPlayerReadyStatusChangedRMR PlayerReadyStatusChangedRMC(Session session, CNetPlayerReadyStatusChangedRMC data)
        {
            var group = Group.GetGroup(data.IdPreMatch);

            group.ChangeReadyStatus(session.Player, data.Ready);
            group.NotifyLobbyChanges();

            return new CNetPlayerReadyStatusChangedRMR(true)
            {
                Status = 0
            };
        }

        public static CNetRegisterCharacterForPvPAutomatchFunDuelRMR RegisterCharacterForPvPAutomatchFunDuelRMC(Session session, CNetRegisterCharacterForPvPAutomatchFunDuelRMC data)
        {
            return new CNetRegisterCharacterForPvPAutomatchFunDuelRMR(true)
            {
                Status = 1
            };
        }

        public static CNetRegisterGroupForPvPAutomatchFunDuelRMR RegisterGroupForPvPAutomatchFunDuelRMC(Session session, CNetRegisterGroupForPvPAutomatchFunDuelRMC data)
        {
            return new CNetRegisterGroupForPvPAutomatchFunDuelRMR(true)
            {
                Status = 1
            };
        }

        public static CNetSetSpeedrunRMR SetSpeedrunRMC(Session session, CNetSetSpeedrunRMC data)
        {
            return new CNetSetSpeedrunRMR(true)
            {
                Status = 0
            };
        }

        public static CNetStartCustomGameRMR StartCustomGameRMC(Session session, CNetStartCustomGameRMC data)
        {
            var group = Group.GetGroup(data.IdPreMatch);

            if (!group.StartGame())
            {
                return new CNetStartCustomGameRMR(true)
                {
                    Status = 1
                };
            }
            else
            {
                return new CNetStartCustomGameRMR(true)
                {
                    Status = 0 //55->StartMatchError, 69->NotAllPlayersReady
                };
            }
        }

        private static CNetAssignLootRewardItemToCharacterRMR AssignLootRewardItemToCharacterRMC(Session session, CNetAssignLootRewardItemToCharacterRMC data)
        {
            return new CNetAssignLootRewardItemToCharacterRMR(true)
            {
                Status = 0
            };
        }

        private static CNetNeedOrGreedVoteForRewardItemRMR NeedOrGreedVoteForRewardItemRMC(Session session, CNetNeedOrGreedVoteForRewardItemRMC data)
        {
            return new CNetNeedOrGreedVoteForRewardItemRMR(true)
            {
                Status = 0
            };
        }
    }
}
