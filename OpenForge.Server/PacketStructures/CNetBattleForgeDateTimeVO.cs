// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using System;

namespace OpenForge.Server.PacketStructures
{
    public class CNetBattleForgeDateTimeVO
    {
        public long Index { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Days { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }

        public CNetBattleForgeDateTimeVO(DateTime dateTime)
        {
            Index = default(long);
            Year = dateTime.Year;
            Month = dateTime.Month;
            Days = dateTime.Day;
            Hours = dateTime.Hour;
            Minutes = dateTime.Minute;
            Seconds = dateTime.Second;
        }
    }
}
