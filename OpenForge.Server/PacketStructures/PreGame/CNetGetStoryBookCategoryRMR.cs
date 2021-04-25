// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketHandlers;

namespace OpenForge.Server.PacketStructures.PreGame
{
    [InterfaceType(InterfaceType.PreGame)]
    public class CNetGetStoryBookCategoryRMR
    {
        public CNetDataHeader Header { get; set; }
        public int Status { get; set; }
        public long[] CategoryEntrys { get; set; }

        public CNetGetStoryBookCategoryRMR(bool defaultHeader)
        {
            Header = new CNetDataHeader(InterfaceType.PreGame, (int)PreGameMessageType.CNetGetStoryBookCategoryRMR, true);
            Status = default(int);
            CategoryEntrys = default(long[]);
        }
    }
}
