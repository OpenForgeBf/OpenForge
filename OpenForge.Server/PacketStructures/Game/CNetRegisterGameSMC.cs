// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketHandlers;

namespace OpenForge.Server.PacketStructures.Game
{
    [InterfaceType(InterfaceType.Game)]
    public class CNetRegisterGameSMC
    {
        public CNetDataHeader Header { get; set; }
        public long GameID { get; set; }
        public long MapID { get; set; }
        public byte GameDifficulty { get; set; }
        public byte GameMode { get; set; }
        public bool IsLimited { get; set; }
        public bool CRCEnabled { get; set; }
        public bool Is12PlayerMatch { get; set; }
        public short RandomSeed { get; set; }
        public CNetGamePlayerVO[] Players { get; set; }
        public byte DefaultMapPlayerCount { get; set; }

        public CNetRegisterGameSMC(bool defaultHeader)
        {
            Header = new CNetDataHeader(InterfaceType.Game, (int)GameMessageType.CNetRegisterGameSMC, false);
            GameID = default(long);
            MapID = default(long);
            GameDifficulty = default(byte);
            GameMode = default(byte);
            IsLimited = default(bool);
            CRCEnabled = default(bool);
            Is12PlayerMatch = default(bool);
            RandomSeed = default(short);
            Players = default(CNetGamePlayerVO[]);
            DefaultMapPlayerCount = default(byte);
        }
    }
}
