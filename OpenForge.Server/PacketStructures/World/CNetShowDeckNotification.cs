﻿// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketHandlers;

namespace OpenForge.Server.PacketStructures.World
{
    [InterfaceType(InterfaceType.World)]
    public class CNetShowDeckNotification
    {
        public CNetDataHeader Header { get; set; }
        public CNetWorldPlayerVO DeckOwner { get; set; }
        public string DeckName { get; set; }
        public CardPool IdCardPool { get; set; }
        public int DeckLevel { get; set; }
        public CNetCardVO CoverCard { get; set; }
        public CNetCardVO[] Cards { get; set; }

        public CNetShowDeckNotification(bool defaultHeader)
        {
            Header = new CNetDataHeader(InterfaceType.World, (int)WorldMessageType.CNetShowDeckNotification, false);
            DeckOwner = default(CNetWorldPlayerVO);
            DeckName = default(string);
            IdCardPool = default(CardPool);
            DeckLevel = default(int);
            CoverCard = default(CNetCardVO);
            Cards = default(CNetCardVO[]);
        }
    }
}
