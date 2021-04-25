// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using OpenForge.Server.Database.Memory;

namespace OpenForge.Coordinator
{
    public class PlayerDescriptor : INotifyPropertyChanged
    {
        private Player _player = null;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Address { get; set; }
        public string ID { get; set; } = Guid.NewGuid().ToString();
        public string Name => _player?.Name;

        public Player Player
        {
            get
            {
                return _player;
            }
            set
            {
                _player = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Player)));
            }
        }
    }
}
