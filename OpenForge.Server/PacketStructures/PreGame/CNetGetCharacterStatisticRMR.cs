// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketHandlers;

namespace OpenForge.Server.PacketStructures.PreGame
{
    [InterfaceType(InterfaceType.PreGame)]
    public class CNetGetCharacterStatisticRMR
    {
        public CNetDataHeader Header { get; set; }
        public int Status { get; set; }
        public int Pve1PlayerWonStandard { get; set; }
        public int Pve1PlayerWonAdvanced { get; set; }
        public int Pve1PlayerWonExpert { get; set; }
        public int Pve2PlayerWonStandard { get; set; }
        public int Pve2PlayerWonAdvanced { get; set; }
        public int Pve2PlayerWonExpert { get; set; }
        public int Pve4PlayerWonStandard { get; set; }
        public int Pve4PlayerWonAdvanced { get; set; }
        public int Pve4PlayerWonExpert { get; set; }
        public int Pve12PlayerWonStandard { get; set; }
        public int Pve12PlayerWonAdvanced { get; set; }
        public int Pve12PlayerWonExpert { get; set; }
        public int Pve1PlayerLooseStandard { get; set; }
        public int Pve1PlayerLooseAdvanced { get; set; }
        public int Pve1PlayerLooseExpert { get; set; }
        public int Pve2PlayerLooseStandard { get; set; }
        public int Pve2PlayerLooseAdvanced { get; set; }
        public int Pve2PlayerLooseExpert { get; set; }
        public int Pve4PlayerLooseStandard { get; set; }
        public int Pve4PlayerLooseAdvanced { get; set; }
        public int Pve4PlayerLooseExpert { get; set; }
        public int Pve12PlayerLooseStandard { get; set; }
        public int Pve12PlayerLooseAdvanced { get; set; }
        public int Pve12PlayerLooseExpert { get; set; }
        public int PvP1vs1Won { get; set; }
        public int PvP2vs2Won { get; set; }
        public int PvP4vs4Won { get; set; }
        public int PvP1vs1Loose { get; set; }
        public int PvP2vs2Loose { get; set; }
        public int PvP4vs4Loose { get; set; }
        public int PvP1vs1LimitedWon { get; set; }
        public int PvP2vs2LimitedWon { get; set; }
        public int PvP1vs1LimitedLoose { get; set; }
        public int PvP2vs2LimitedLoose { get; set; }
        public int TotalCards { get; set; }
        public long EloRating { get; set; }
        public long EloRatingLimited { get; set; }

        public CNetGetCharacterStatisticRMR(bool defaultHeader)
        {
            Header = new CNetDataHeader(InterfaceType.PreGame, (int)PreGameMessageType.CNetGetCharacterStatisticRMR, true);
            Status = default(int);

            Pve1PlayerWonStandard = default(int);
            Pve1PlayerWonAdvanced = default(int);
            Pve1PlayerWonExpert = default(int);
            Pve2PlayerWonStandard = default(int);
            Pve2PlayerWonAdvanced = default(int);
            Pve2PlayerWonExpert = default(int);
            Pve4PlayerWonStandard = default(int);
            Pve4PlayerWonAdvanced = default(int);
            Pve4PlayerWonExpert = default(int);
            Pve12PlayerWonStandard = default(int);
            Pve12PlayerWonAdvanced = default(int);
            Pve12PlayerWonExpert = default(int);
            Pve1PlayerLooseStandard = default(int);
            Pve1PlayerLooseAdvanced = default(int);
            Pve1PlayerLooseExpert = default(int);
            Pve2PlayerLooseStandard = default(int);
            Pve2PlayerLooseAdvanced = default(int);
            Pve2PlayerLooseExpert = default(int);
            Pve4PlayerLooseStandard = default(int);
            Pve4PlayerLooseAdvanced = default(int);
            Pve4PlayerLooseExpert = default(int);
            Pve12PlayerLooseStandard = default(int);
            Pve12PlayerLooseAdvanced = default(int);
            Pve12PlayerLooseExpert = default(int);
            PvP1vs1Won = default(int);
            PvP2vs2Won = default(int);
            PvP4vs4Won = default(int);
            PvP1vs1Loose = default(int);
            PvP2vs2Loose = default(int);
            PvP4vs4Loose = default(int);
            PvP1vs1LimitedWon = default(int);
            PvP2vs2LimitedWon = default(int);
            PvP1vs1LimitedLoose = default(int);
            PvP2vs2LimitedLoose = default(int);
            TotalCards = default(int);
            EloRating = default(long);
            EloRatingLimited = default(long);
        }
    }
}
