// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

namespace OpenForge.Server.PacketStructures
{
    public class CNetUserGeneratedMapVO
    {
        public long Id { get; set; }
        public string[] MapNames { get; set; }
        public bool IsPvE { get; set; }
        public int PlayerAmount { get; set; }
        public long CheckSum { get; set; }
        public long FileSize { get; set; }
        public long UncompressedFileSize { get; set; }
        public bool IsPublic { get; set; }
        public string OriginalFileName { get; set; }
    }
}
