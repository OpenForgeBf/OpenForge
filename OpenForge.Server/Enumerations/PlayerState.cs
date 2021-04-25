// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

namespace OpenForge.Server.Enumerations
{
    public enum PlayerState : int
    {
        Offline = 0,
        Online = 1,
        Ingame = 2,
        AFK = 3,
        DND = 4,
        LFG = 5
    }
}
