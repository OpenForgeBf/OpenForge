// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketHandlers;

namespace OpenForge.Server.PacketStructures.Observer
{
    [InterfaceType(InterfaceType.Observer)]
    public class CNetServerReportsInvalidStateAction
    {
        public int ServerId { get; set; }
        public int ServerType { get; set; }
        public int ErrorLevel { get; set; }
    }
}
