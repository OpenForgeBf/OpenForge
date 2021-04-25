// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using OpenForge.Server.Enumerations;

namespace OpenForge.Server.PacketStructures
{
    // This packet signals the client that the match is over.
    // The group leader is able to create a new game after he received this packet

    public class CNetGameClosedNotification
    {
        public CNetDataHeader Header { get; set; }
        public byte Reason { get; set; }

        public CNetGameClosedNotification(bool defaultHeader)
        {
            Header = new CNetDataHeader(InterfaceType.Game, (int)GameMessageType.CNetGameClosedNotification, false);
            Reason = default(byte);
        }
    }
}
