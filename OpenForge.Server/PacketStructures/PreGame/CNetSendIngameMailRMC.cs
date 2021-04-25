// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketHandlers;

namespace OpenForge.Server.PacketStructures.PreGame
{
    [InterfaceType(InterfaceType.PreGame)]
    public class CNetSendIngameMailRMC
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public int Gold { get; set; }
        public int BFPoints { get; set; }
        public long[] CardList { get; set; }
        public long[] BoostersList { get; set; }
        public bool OverrideScamCheck { get; set; }
    }
}
