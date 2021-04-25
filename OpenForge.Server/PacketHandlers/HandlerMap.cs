// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NLog;
using OpenForge.Server.Enumerations;
using OpenForge.Server.Extensions;
using OpenForge.Server.Messages;
using OpenForge.Server.PacketStructures;

namespace OpenForge.Server.PacketHandlers
{
    public static class HandlerMap
    {
        private static readonly Dictionary<InterfaceType, Dictionary<int, PacketHandler>> s_handlerMap = new Dictionary<InterfaceType, Dictionary<int, PacketHandler>>();

        private delegate void PacketHandler(Session session, MessageReader reader);

        private static Logger Logger { get; } = LogManager.GetCurrentClassLogger();

        public static void HandlePacket(Session session, CNetDataHeader head, MessageReader reader)
        {
            if (!s_handlerMap.TryGetValue(head.Interface, out var handlerMap))
            {
                Logger.Warn($"Handler for interface {head.Interface} does not exist.");
                return;
            }

            if (!handlerMap.TryGetValue(head.MessageId, out var packetHandler))
            {
                Logger.Warn($"A handler for interface {head.Interface} message id {head.MessageId} does not exist.");
                return;
            }

            packetHandler(session, reader);
        }

        public static void MapHandlers(Type type)
        {
            foreach (var m in type.GetMethods(BindingFlags.Public | BindingFlags.Static))
            {
                var parameters = m.GetParameters();
                if (parameters.Length < 2)
                {
                    continue;
                }

                var messageType = parameters[1].ParameterType;
                if (messageType == null)
                {
                    Logger.Warn($"Failed to find packet structure type {m.Name}.");
                    continue;
                }

                var isRMC = messageType.Name.EndsWith("RMC");
                var isAction = messageType.Name.EndsWith("Action");
                if (!isRMC && !isAction)
                {
                    Logger.Warn($"Only RMC and Action types are expected to be handlers on {m.Name}, skipping handler registration.");
                    continue;
                }

                if (isRMC && m.ReturnType == typeof(void))
                {
                    Logger.Warn($"RMC must return a RMR {m.Name}, skipping handler registration.");
                    continue;
                }

                var interfaceTypeAttribute = messageType.GetCustomAttribute<InterfaceTypeAttribute>();
                if (interfaceTypeAttribute == null)
                {
                    Logger.Warn($"Expected interface type attribute on {m.Name}, skipping handler registration.");
                    continue;
                }

                var messageTypeEnumType = GetInterfaceMessageTypeEnum(interfaceTypeAttribute.InterfaceType);
                if (messageTypeEnumType == null)
                {
                    Logger.Warn($"Message type enum does not exist for type {interfaceTypeAttribute.InterfaceType} on {m.Name}, skipping handler registration.");
                    continue;
                }

                if (!Enum.TryParse(messageTypeEnumType, messageType.Name, out var messageIdObject))
                {
                    Logger.Warn($"Failed to find message id on {m.Name}, skipping handler registration.");
                    continue;
                }

                var messageId = Convert.ToInt32(messageIdObject);

                Dictionary<int, PacketHandler> d;
                if (s_handlerMap.ContainsKey(interfaceTypeAttribute.InterfaceType))
                {
                    d = s_handlerMap[interfaceTypeAttribute.InterfaceType];
                }
                else
                {
                    d = new Dictionary<int, PacketHandler>();
                    s_handlerMap.Add(interfaceTypeAttribute.InterfaceType, d);
                }

                if (d.ContainsKey(messageId))
                {
                    Logger.Warn($"Multiple handlers exist for type {messageId}.");
                }

                if (isRMC)
                {
                    var rmrConstructor = m.ReturnType.GetConstructor(new Type[] { typeof(bool) });
                    var rmrStatusField = m.ReturnType.GetField("Status");

                    d[messageId] = (session, reader) =>
                    {
                        var data = reader.Deserialize(messageType);

                        try
                        {
                            var response = m.Invoke(null, new object[] { session, data });
                            session.Send(response);
                        }
                        catch (Exception e)
                        {
                            if (rmrStatusField != null && rmrConstructor != null)
                            {
                                Logger.Warn(e, $"Failed to handle using {m.Name}, attempting to send default error response.");

                                try
                                {
                                    var rmr = rmrConstructor.Invoke(new object[] { true });
                                    rmrStatusField.SetValue(rmr, 1);
                                    session.Send(rmr);
                                    Logger.Info($"Sent default error response for handler {m.Name}.");
                                }
                                catch (Exception ex)
                                {
                                    Logger.Warn(ex, $"Failed to send default error response {m.ReturnType.Name}.");
                                }
                            }
                            else
                            {
                                Logger.Warn(e, $"Failed to handle using {m.Name}, no default error response available.");
                            }
                        }
                    };
                }
                else if (isAction)
                {
                    d[messageId] = (session, reader) =>
                    {
                        var data = reader.Deserialize(messageType);
                        m.Invoke(null, new object[] { session, data });
                    };
                }
            }
        }

        private static Type GetInterfaceMessageTypeEnum(InterfaceType interfaceType)
        {
            return interfaceType switch
            {
                InterfaceType.Borderline => typeof(BorderlineMessageType),
                InterfaceType.Chat => typeof(ChatMessageType),
                InterfaceType.Game => typeof(GameMessageType),
                InterfaceType.Matchmaking => typeof(MatchmakingMessageType),
                InterfaceType.Observer => typeof(ObserverMessageType),
                InterfaceType.PreGame => typeof(PreGameMessageType),
                InterfaceType.Shop => typeof(ShopMessageType),
                InterfaceType.World => typeof(WorldMessageType),
                _ => null
            };
        }
    }
}
