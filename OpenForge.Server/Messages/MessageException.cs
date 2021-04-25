// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using System;

namespace OpenForge.Server.Messages
{
    public class MessageException : Exception
    {
        public MessageException(string message) : base(message) { }
    }
}
