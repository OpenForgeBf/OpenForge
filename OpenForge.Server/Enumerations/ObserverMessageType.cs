// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

namespace OpenForge.Server.Enumerations
{
    public enum ObserverMessageType : byte
    {
        CNetIamAliveAction = 0x4,
        CNetObserverCommandAction = 0x2,
        CNetRegisterServerAction = 0x3,
        CNetServerReportsInvalidStateAction = 0x1,
    }
}
