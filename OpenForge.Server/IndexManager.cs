// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenForge.Server
{
    public class IndexManager
    {
        private readonly Func<IEnumerable<ulong>> _allIds = null;
        private readonly object _counterLock = new object();
        private readonly Func<ulong> _maxId = null;
        private ulong _counter = 0;

        public IndexManager()
        {
        }

        public IndexManager(Func<IEnumerable<ulong>> allIds)
        {
            _allIds = allIds;
        }

        public IndexManager(Func<ulong> maxId)
        {
            _maxId = maxId;
        }

        public ulong NewIndex()
        {
            lock (_counterLock)
            {
                if (_counter <= 0)
                {
                    if (_allIds == null && _maxId == null)
                    {
                        _counter = 1;
                    }
                    else
                    {
                        if (_maxId != null)
                        {
                            _counter = Math.Max(1, _maxId()) + 1;
                        }
                        else
                        {
                            var allIds = _allIds();
                            if (allIds.Count() == 0)
                            {
                                _counter = 1;
                            }
                            else
                            {
                                _counter = Math.Max(1, allIds.Max()) + 1;
                            }
                        }
                    }
                }
                return _counter++;
            }
        }
    }
}
