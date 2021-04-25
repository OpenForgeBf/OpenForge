// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using NLog;
using OpenForge.Server.Database.Memory;
using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketStructures;
using OpenForge.Server.PacketStructures.Borderline;

namespace OpenForge.Server.PacketHandlers
{
    public static class BorderlineHandlers
    {
        public static Logger Logger = LogManager.GetCurrentClassLogger();

        public static CNetEnterSerialCodeRMR EnterSerialCodeRMC(Session session, CNetEnterSerialCodeRMC data)
        {
            return new CNetEnterSerialCodeRMR(true)
            {
                Status = 0,
                BfPoints = 0
            };
        }

        public static CNetGetWalletBalanceRMR GetWalletBalanceRMC(Session session, CNetGetWalletBalanceRMC data)
        {
            return new CNetGetWalletBalanceRMR(true)
            {
                Status = 0,
                BfPoints = session.Player.BFP
            };
        }

        public static void KeepAliveAction(Session session, CNetKeepAliveAction data)
        {
            session.Send(new CNetKeepAliveNotification(true));
        }

        public static CNetLoginAccountRMR LoginAccountRMC(Session session, CNetLoginAccountRMC data)
        {
            Player.Login(session);

            return new CNetLoginAccountRMR(true)
            {
                Status = (int)LoginResult.Success,
                CharacterList = new CNetWorldPlayerVO[] { session.Player.GetWorldPlayer() },
                AccessMode = 4,
                BfPoints = session.Player.BFP,
                ClientBuildNumber = 0,
                IsBeta = false,
                OriginPersonaName = null
            };
        }

        public static CNetLoginCharacterRMR LoginCharacterRMC(Session session, CNetLoginCharacterRMC data)
        {
            return new CNetLoginCharacterRMR(true)
            {
                Status = 0
            };
        }

        public static void LogoutAction(Session session, CNetLogoutAction data)
        {
            var p = Player.GetPlayerByAddress(session.Address);
            if (p != null)
            {
                p.Logout();
            }
        }

        public static CNetReferUserRMR ReferUserRMC(Session session, CNetReferUserRMC data)
        {
            return new CNetReferUserRMR(true)
            {
                Status = 1
            };
        }

        public static CNetRegisterTrialVersionRMR RegisterTrialVersionRMC(Session session, CNetRegisterTrialVersionRMC data)
        {
            return new CNetRegisterTrialVersionRMR(true)
            {
                Status = 0,
                BfPoints = 0
            };
        }
    }
}
