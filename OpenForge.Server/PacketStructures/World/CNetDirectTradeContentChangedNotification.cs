// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketHandlers;

namespace OpenForge.Server.PacketStructures.World
{
    [InterfaceType(InterfaceType.World)]
    public class CNetDirectTradeContentChangedNotification
    {
        public CNetDataHeader Header { get; set; }
        public ulong IdDirectTrade { get; set; }
        public ulong LeftIdCharacter { get; set; }
        public int LeftMoneyAmount { get; set; }
        public long[] LeftCards { get; set; }
        public long[] LeftBoosters { get; set; }
        public bool LeftAccepted { get; set; }
        public ulong RightIdCharacter { get; set; }
        public int RightMoneyAmount { get; set; }
        public long[] RightCards { get; set; }
        public long[] RightBoosters { get; set; }
        public bool RightAccepted { get; set; }

        public CNetDirectTradeContentChangedNotification(bool defaultHeader)
        {
            Header = new CNetDataHeader(InterfaceType.World, (int)WorldMessageType.CNetDirectTradeContentChangedNotification, false);
            IdDirectTrade = default(ulong);
            LeftIdCharacter = default(ulong);
            LeftMoneyAmount = default(int);
            LeftCards = default(long[]);
            LeftBoosters = default(long[]);
            LeftAccepted = default(bool);
            RightIdCharacter = default(ulong);
            RightMoneyAmount = default(int);
            RightCards = default(long[]);
            RightBoosters = default(long[]);
            RightAccepted = default(bool);
        }
    }
}
