// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OpenForge.Coordinator.Packets
{
    public enum CoordinatorPackets : ushort
    {
        GetPlayersRequest = 90,
        GetPlayersResponse = 91,
        PushPlayerDataPacket = 92,

        RegisterPlayerRequest = 100,
        RegisterPlayerResponse = 101,

        SyncPlayerRequest = 110,
        SyncPlayerResponse = 111,
    }

    public class CoordinatorPacket
    {
        private static readonly Dictionary<Type, ushort> s_coordPacketIDs = typeof(CoordinatorPacket).Assembly
            .GetTypes()
            .Where(x => x.GetCustomAttribute<CoordinatorPacketAttribute>() != null)
            .ToDictionary(y => y, x => x.GetCustomAttribute<CoordinatorPacketAttribute>().ID);

        private static readonly Dictionary<ushort, Type> s_coordPacketTypes = typeof(CoordinatorPacket).Assembly
                    .GetTypes()
            .Where(x => x.GetCustomAttribute<CoordinatorPacketAttribute>() != null)
            .ToDictionary(x => x.GetCustomAttribute<CoordinatorPacketAttribute>().ID, y => y);

        public static ushort GetPacketID(Type t)
        {
            if (!s_coordPacketIDs.ContainsKey(t))
            {
                throw new NotImplementedException($"Unknown PacketType {t.Name}, missing ID?");
            }

            return s_coordPacketIDs[t];
        }

        public static ushort GetPacketID(CoordinatorPacket packet)
        {
            return GetPacketID(packet.GetType());
        }

        public static Type GetPacketType(ushort id)
        {
            if (!s_coordPacketTypes.ContainsKey(id))
            {
                throw new NotImplementedException($"Unknown PacketID {id}");
            }

            return s_coordPacketTypes[id];
        }

        public ushort GetPacketID()
        {
            return GetPacketID(this);
        }
    }

    public class CoordinatorPacketAttribute : Attribute
    {
        public CoordinatorPacketAttribute(ushort id)
        {
            ID = id;
        }

        public ushort ID { get; set; }
    }

    public class CoordinatorPacketHeader
    {
        public ushort PacketID { get; set; }
        public int RequestID { get; set; }
        public int ResponseID { get; set; }
    }
}
