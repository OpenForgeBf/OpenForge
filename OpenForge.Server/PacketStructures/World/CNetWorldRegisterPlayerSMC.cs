// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketHandlers;

namespace OpenForge.Server.PacketStructures.World
{
    [InterfaceType(InterfaceType.World)]
    public class CNetWorldRegisterPlayerSMC
    {
        public CNetDataHeader Header { get; set; }
        public bool TrialUser { get; set; }

        public CNetWorldRegisterPlayerSMC(bool defaultHeader)
        {
            Header = new CNetDataHeader(InterfaceType.World, (int)WorldMessageType.CNetWorldRegisterPlayerSMC, false);
            TrialUser = default(bool);
        }
    }
}
