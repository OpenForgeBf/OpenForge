// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Text;
using NLog;
using OpenForge.Server.Enumerations;
using OpenForge.Server.Extensions;
using OpenForge.Server.Messages;
using OpenForge.Server.PacketStructures;
using OpenForge.Server.PacketStructures.Game;
using OpenForge.Server.PacketStructures.Matchmaking;

namespace OpenForge.Server.PacketHandlers
{
    public static class GameHandlers
    {
        private static Logger Logger { get; } = LogManager.GetCurrentClassLogger();
        private static Logger SyncCheckActionLogger { get; } = LogManager.GetLogger($"{nameof(GameHandlers)}.SyncCheckAction");

        public static void CharacterLooseAction(Session session, CNetCharacterLooseAction data)
        {
            HandlePlayerLeftMatch(session, true);
        }

        public static void CharacterWinAction(Session session, CNetCharacterWinAction data)
        {
            HandlePlayerLeftMatch(session, true);
        }

        public static void CurrentStepAction(Session session, CNetCurrentStepAction data)
        {
            var group = session.Player.GetActiveGroup();
            if (group == null)
            {
                throw new NullReferenceException("Player game without group?");
            }

            var match = group.OngoingMatch;
            if (group == null)
            {
                throw new NullReferenceException("Group without match?");
            }

            var serverStep = match.CurrentStepGenerator;
            var clientStep = data.Step;
            var secondsSinceGameStart = match.MatchTimer.Elapsed.TotalSeconds;
            Logger.Info(() => $"Client current step is {data.Step}, server current step is {serverStep}, average client steps per second {clientStep / secondsSinceGameStart:N2}, average server steps per second {serverStep / secondsSinceGameStart:N2}.");
        }

        public static void MapCompleteLoadedAction(Session session, CNetMapCompleteLoadedAction data)
        {
            var group = session.Player.GetActiveGroup();
            if (group == null)
            {
                throw new NullReferenceException("Player game without group?");
            }

            var match = group.OngoingMatch;
            if (match == null)
            {
                throw new NullReferenceException("Group without match?");
            }

            match.FinishedLoading(session.Player);
        }

        public static void PingAction(Session session, CNetPingAction data)
        {
            var group = session.Player.GetActiveGroup();
            if (group == null)
            {
                throw new NullReferenceException("Player game without group?");
            }

            var match = group.OngoingMatch;
            if (group == null)
            {
                throw new NullReferenceException("Group without match?");
            }

            session.Player?.Send(new CNetPingNotification(false)
            {
                Header = new CNetDataHeader()
                {
                    ClientIds = default,
                    Interface = InterfaceType.Game,
                    MessageId = (int)GameMessageType.CNetPingNotification,
                    Broadcast = false,
                    RemoteMethod = false,
                    DestinationServerId = 0,
                    SourceServerId = 0,
                    SequenceNumber = match.Lobby.GetTeamPlayer(session.Player).SequenceNumberUpdate(),
                    RequestId = 0,
                    CharacterId = session.Player.ID,
                    Channel = match.ID,
                }
            });
        }

        public static void RequestCommandAction(Session session, CNetRequestCommandAction data)
        {
            var group = session.Player.GetActiveGroup();
            if (group == null)
            {
                throw new NullReferenceException("Player game without group?");
            }

            var match = group.OngoingMatch;
            if (group == null)
            {
                throw new NullReferenceException("Group without match?");
            }

            match.StepMutex.WaitOne();

            try
            {
                var blobReader = new MessageReader(new MemoryStream(data.Blob, false));

                switch (data.GdCommand)
                {
                    case CommandType.Desync:
                        var cmdDesync = blobReader.Deserialize<CLogMsgDesync>();
                        Logger.Warn($"Character: {cmdDesync.GlobalPlayer} desynced. Reason: {cmdDesync.Reason}.");
                        break;

                    case CommandType.PlayerLoot:
                        var cmdPlayerLoot = blobReader.Deserialize<CLogMsgPlayerLoot>();

                        using (var stream = new MemoryStream())
                        {
                            using (var writer = new MessageWriter(stream))
                            {
                                writer.Serialize(new CLogMsgPlayerLootResult()
                                {
                                    Looter = cmdPlayerLoot.GlobalPlayer,
                                    LootTarget = cmdPlayerLoot.LootTarget,
                                    GoldAmountPlayers = new GoldAmountPlayer[]
                                    {
                                        new GoldAmountPlayer()
                                        {
                                            GlobalPlayer = (ulong)cmdPlayerLoot.GlobalPlayer,
                                            GoldAmount = new Random(Environment.TickCount).Next(500, 1000)
                                        }
                                    }
                                });
                            }

                            match.SendCommand(CommandType.PlayerLootResult, stream.ToArray());
                        }
                        break;

                    default:
                        match.SendCommand(data.GdCommand, data.Blob);
                        break;
                }
            }
            finally
            {
                match.StepMutex.ReleaseMutex();
            }
        }

        public static void SyncCheckAction(Session session, CNetSyncCheckAction data)
        {
            var group = session.Player.GetActiveGroup();
            if (group == null)
            {
                throw new NullReferenceException("Player game without group?");
            }

            var match = group.OngoingMatch;
            if (match == null)
            {
                throw new NullReferenceException("Group without match?");
            }

            using var stream = new MemoryStream(data.Blob, false);
            using var reader = new MessageReader(stream);
            var syncCheck = reader.Deserialize<CLogMsgSyncCheck>();

            SyncCheckActionLogger.Trace(() =>
            {
                var stringBuilder = new StringBuilder(2048);
                stringBuilder.AppendLine($"Player '{session.Player.Name}' sync action received:");
                stringBuilder.AppendLine($"  Step: {syncCheck.Step}");
                stringBuilder.AppendLine($"  Step interval: {syncCheck.StepInterval}");
                stringBuilder.AppendLine($"  BarrierModuleCrc: {syncCheck.BarrierModuleCrc}");
                stringBuilder.AppendLine($"  BarrierSetCrc: {syncCheck.BarrierSetCrc}");
                stringBuilder.AppendLine($"  BuildingCrc: {syncCheck.BuildingCrc}");
                stringBuilder.AppendLine($"  FigureCrc: {syncCheck.FigureCrc}");
                stringBuilder.AppendLine($"  UnknownCrc: {syncCheck.UnknownCrc}");
                stringBuilder.AppendLine($"  ObjectCrc: {syncCheck.ObjectCrc}");
                stringBuilder.AppendLine($"  PlayerCrc: {syncCheck.PlayerCrc}");
                stringBuilder.AppendLine($"  PowerSlotCrc: {syncCheck.PowerSlotCrc}");
                stringBuilder.AppendLine($"  ProjectileCrc: {syncCheck.ProjectileCrc}");
                stringBuilder.AppendLine($"  ScriptingCrc: {syncCheck.ScriptingCrc}");
                stringBuilder.AppendLine($"  SquadCrc: {syncCheck.SquadCrc}");
                stringBuilder.AppendLine($"  TokenSlotCrc: {syncCheck.TokenSlotCrc}");
                stringBuilder.AppendLine($"  WorldCrc: {syncCheck.WorldCrc}");
                stringBuilder.AppendLine($"  TurretCrc: {syncCheck.TurretCrc}");

                var assetCrcStrings = string.Join(", ", syncCheck.AssetCrcs);
                stringBuilder.Append($"  AssetCRCs: {assetCrcStrings}");
                return stringBuilder.ToString();
            });

            if (reader.BaseStream.Position != reader.BaseStream.Length)
                throw new Exception("Expected end of packet.");

            if (!match.CheckSynchronization(syncCheck.Step, syncCheck.UnknownCrc))
            {
                Logger.Warn($"Player '{session.Player.Name}' has desynced from the match.");

                using var commandStream = new MemoryStream();
                using (var writer = new BinaryWriter(commandStream))
                {
                    writer.Write(data.IdCharacter);
                    writer.Write((ushort)1);
                }

                match.SendCommand(CommandType.Desync, commandStream.ToArray());
            }
        }

        public static void UnRegisterCharacterFromGameAction(Session session, CNetUnRegisterCharacterFromGameAction data)
        {
            HandlePlayerLeftMatch(session, false);
        }

        private static void HandlePlayerLeftMatch(Session session, bool win)
        {
            var group = session.Player.GetActiveGroup();
            if (group == null)
            {
                throw new NullReferenceException("Player game without group?");
            }

            var match = group.OngoingMatch;
            if (match == null)
            {
                return;
            }
            //throw new NullReferenceException("Group without match?");

            match.PlayerLeft(session.Player);

            /*only when more than 1 players in match
             * session.Send(new CNetPlayerLeftGameNotification(true)
            {
                Header = new CNetDataHeader()
                {
                    ClientIds = default(long[]),
                    Interface = InterfaceType.Game,
                    MessageId = (int)GameMessageType.CNetPlayerLeftGameNotification,
                    Broadcast = false,
                    RemoteMethod = false,
                    DestinationServerId = 0,
                    SourceServerId = 0,
                    SequenceNumber = Interlocked.Increment(ref session.SequenceNumberGenerator),
                    RequestId = 0,
                    CharacterId = session.Player.Index,
                    Channel = session.CurrentMatch.IdMatch,
                },
                IdCharacter = session.Player.Index
            });*/

            /*Send(new CNetGameClosedNotification(true)
            {
                Header = new CNetDataHeader()
                {
                    ClientIds = default(long[]),
                    Interface = InterfaceType.Game,
                    MessageId = (int)GameMessageType.CNetGameClosedNotification,
                    Broadcast = false,
                    RemoteMethod = false,
                    DestinationServerId = 0,
                    SourceServerId = 0,
                    SequenceNumber = Interlocked.Increment(ref SequenceNumberGenerator),
                    RequestId = 0,
                    CharacterId = Player.Index,
                    Channel = CurrentMatch.IdMatch,
                },
            });*/

            group.Send(player => new CNetMatchFinishedNotification(true)
            {
                IdMatch = match.ID,
                IdLootingType = 1,
                IsLimited = false,
                WinnerCharacterIds = win ? new ulong[] { player.ID } : new ulong[0],
                LooserCharacterIds = !win ? new ulong[] { player.ID } : new ulong[0],
                TokenRewardList = new CNetCharacterTokenRewardVO[0],
                RewardList = new CNetCharacterRewardVO[0],
                XPList = new CNetCharacterXPVO[0],
                EloRatingList = new CNetCharacterEloRatingVO[0]
            });

            if (match.StopIfDisconnected())
            {
                ;
            }
            //    group.OngoingMatch = null;
        }
    }

    public class CLogMsgDesync
    {
        public long GlobalPlayer { get; set; }
        public short Reason { get; set; }
    }

    public class CLogMsgPlayerLoot
    {
        public long GlobalPlayer { get; set; }
        public int LootTarget { get; set; }
        public int ResMapLootId { get; set; }
    }

    public class CLogMsgPlayerLootResult
    {
        public long Looter { get; set; }
        public int LootTarget { get; set; }
        public GoldAmountPlayer[] GoldAmountPlayers { get; set; }
    }

    public class GoldAmountPlayer
    {
        public ulong GlobalPlayer { get; set; }
        public int GoldAmount { get; set; }
    }

    public class CLogMsgSyncCheck
    {
        public uint Step { get; set; }
        public ushort StepInterval { get; set; }
        public uint BarrierModuleCrc { get; set; }
        public uint BarrierSetCrc { get; set; }
        public uint BuildingCrc { get; set; }
        public uint FigureCrc { get; set; }
        public uint UnknownCrc { get; set; }
        public uint ObjectCrc { get; set; }
        public uint PlayerCrc { get; set; }
        public uint PowerSlotCrc { get; set; }
        public uint ProjectileCrc { get; set; }
        public uint ScriptingCrc { get; set; }
        public uint SquadCrc { get; set; }
        public uint TokenSlotCrc { get; set; }
        public uint WorldCrc { get; set; }
        public uint TurretCrc { get; set; }
        public uint[] AssetCrcs { get; set; }
    }

    public class CLogMsgPlayerRemove
    {
        public long GlobalPlayer { get; set; }
    }
}
