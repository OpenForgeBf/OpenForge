// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using OpenForge.Server.Enumerations;

namespace OpenForge.Server.PacketStructures
{
    public class CNetDataHeader
    {
        public long[] ClientIds { get; set; }
        public InterfaceType Interface { get; set; }
        public int MessageId { get; set; }
        public bool Broadcast { get; set; }
        public bool RemoteMethod { get; set; }
        public int DestinationServerId { get; set; }
        public int SourceServerId { get; set; }
        public int SequenceNumber { get; set; }
        public int RequestId { get; set; }
        public ulong CharacterId { get; set; }
        public ulong Channel { get; set; }

        public CNetDataHeader()
        {
        }

        public CNetDataHeader(InterfaceType interfaceType, int messageType, bool isRemoteMethod)
        {
            ClientIds = default(long[]);
            Interface = interfaceType;
            MessageId = messageType;
            Broadcast = false;
            RemoteMethod = isRemoteMethod;
            DestinationServerId = 0;
            SourceServerId = 0;
            SequenceNumber = 0;
            RequestId = 0;
            CharacterId = 0;
            Channel = 0;
        }
    }
}
