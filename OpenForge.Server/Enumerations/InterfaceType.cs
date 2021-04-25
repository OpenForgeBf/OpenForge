// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

namespace OpenForge.Server.Enumerations
{
    public enum InterfaceType : int
    {
        Observer = 0x2,
        Shop = 0x3,
        World = 0x4,
        Game = 0x5,
        Chat = 0x6,
        PreGame = 0x7,
        Matchmaking = 0x8,
        Borderline = 0xA
    }
}
