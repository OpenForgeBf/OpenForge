// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using System;
using OpenForge.Server.Enumerations;

namespace OpenForge.Server.PacketHandlers
{
    public class InterfaceTypeAttribute : Attribute
    {
        public InterfaceTypeAttribute(InterfaceType interfaceType)
        {
            InterfaceType = interfaceType;
        }

        public InterfaceType InterfaceType { get; }
    }
}
