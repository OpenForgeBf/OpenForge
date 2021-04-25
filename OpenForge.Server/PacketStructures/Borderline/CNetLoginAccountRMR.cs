// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketHandlers;

namespace OpenForge.Server.PacketStructures.Borderline
{
    [InterfaceType(InterfaceType.Borderline)]
    public class CNetLoginAccountRMR
    {
        public CNetDataHeader Header { get; set; }
        public int Status { get; set; }
        public CNetWorldPlayerVO[] CharacterList { get; set; }
        public int AccessMode { get; set; }
        public int BfPoints { get; set; }
        public int ClientBuildNumber { get; set; }
        public bool IsBeta { get; set; }
        public string OriginPersonaName { get; set; }

        public CNetLoginAccountRMR(bool defaultHeader)
        {
            Header = new CNetDataHeader(InterfaceType.Borderline, (int)BorderlineMessageType.CNetLoginAccountRMR, true);
            Status = default(int);
            CharacterList = default(CNetWorldPlayerVO[]);
            AccessMode = default(int);
            BfPoints = default(int);
            ClientBuildNumber = default(int);
            IsBeta = default(bool);
            OriginPersonaName = default(string);
        }
    }
}
