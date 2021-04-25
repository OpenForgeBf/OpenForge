// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

namespace OpenForge.Server.Enumerations
{
    public enum GameMessageType : byte
    {
        CNetAnnounceCommandNotification = 0xB,
        CNetCharacterLooseAction = 0xD,
        CNetCharacterWinAction = 0xC,
        CNetCreateGameAction = 0x11,
        CNetCurrentStepAction = 0x5,
        CNetExecuteToStepNotification = 0x9,
        CNetGameClosedNotification = 0x2,
        CNetGameStartNotification = 0x4,
        CNetLost12PlayerMapNotification = 0x1,
        CNetMapCompleteLoadedAction = 0xE,
        CNetPingAction = 0x10,
        CNetPingNotification = 0xF,
        CNetPlayerLeftGameNotification = 0x7,
        CNetRegisterGameSMC = 0x12,
        CNetRequestCommandAction = 0xA,
        CNetSyncCheckAction = 0x3,
        CNetUnRegisterCharacterFromGameAction = 0x6,
    }
}
