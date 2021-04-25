// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketHandlers;

namespace OpenForge.Server.PacketStructures.Borderline
{
    [InterfaceType(InterfaceType.Borderline)]
    public class CNetGetServerFarmVersionInfoRMR
    {
        public CNetDataHeader Header { get; set; }
        public int Status { get; set; }
        public int ClientBuildNumber { get; set; }
        public int BootstrapperBuildNumber { get; set; }
        public int NewsVersion { get; set; }

        public CNetGetServerFarmVersionInfoRMR(bool defaultHeader)
        {
            Header = new CNetDataHeader(InterfaceType.Borderline, (int)BorderlineMessageType.CNetGetServerFarmVersionInfoRMR, true);
            Status = default(int);
            ClientBuildNumber = default(int);
            BootstrapperBuildNumber = default(int);
            NewsVersion = default(int);
        }
    }
}
