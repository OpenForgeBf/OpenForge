// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

namespace OpenForge.Server.Enumerations
{
    public enum BorderlineMessageType : byte
    {
        CNetAddServerConnectionSessionsAction = 0x3,
        CNetClientForceDisconnectNotification = 0x1,
        CNetClientTimeoutNotification = 0xA,
        CNetCreateAccountRMC = 0x12,
        CNetCreateAccountRMR = 0x1C,
        CNetEnterSerialCodeRMC = 0x10,
        CNetEnterSerialCodeRMR = 0x1A,
        CNetGetServerFarmVersionInfoRMC = 0xF,
        CNetGetServerFarmVersionInfoRMR = 0x15,
        CNetGetWalletBalanceRMC = 0x13,
        CNetGetWalletBalanceRMR = 0x1D,
        CNetKeepAliveAction = 0x7,
        CNetKeepAliveNotification = 0x6,
        CNetLoginAccountRMC = 0x14,
        CNetLoginAccountRMR = 0x1E,
        CNetLoginCharacterRMC = 0xE,
        CNetLoginCharacterRMR = 0x19,
        CNetLoginNucleusAccountRMC = 0x11,
        CNetLoginNucleusAccountRMR = 0x1B,
        CNetLogoutAction = 0x9,
        CNetLogoutCurrentCharacterRMC = 0xD,
        CNetLogoutCurrentCharacterRMR = 0x18,
        CNetLogoutNotification = 0x8,
        CNetReferUserRMC = 0xB,
        CNetReferUserRMR = 0x16,
        CNetRegisterServerConnectionAction = 0x4,
        CNetRegisterTrialVersionRMC = 0xC,
        CNetRegisterTrialVersionRMR = 0x17,
        CNetRemoveServerConnectionSessionsAction = 0x2,
        CNetServerFarmShutdownSchedueledNotification = 0x5,
    }
}
