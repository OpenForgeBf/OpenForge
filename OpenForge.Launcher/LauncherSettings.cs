// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OpenForge.Launcher
{
    public class LauncherSettings
    {
        private const string SETTINGS_FILE = "LAUNCHER_SETTINGS.json";
        public string BattleForgePath { get; set; } = "BattleForge.exe";



        private static LauncherSettings _instance = null;
        public static LauncherSettings Instance
        {
            get
            {
                if (_instance == null)
                    Load();
                return _instance;
            }
        }

        public void Save()
        {
            File.WriteAllText(SETTINGS_FILE, JsonConvert.SerializeObject(this));
        }
        private static void Load()
        {
            if (File.Exists(SETTINGS_FILE))
            {
                try
                {
                    _instance = JsonConvert.DeserializeObject<LauncherSettings>(File.ReadAllText(SETTINGS_FILE));
                }
                catch
                {
                    _instance = new LauncherSettings();
                }
            }
            else
                _instance = new LauncherSettings();
        }
    }
}
