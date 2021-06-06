// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OpenForge.Launcher
{
    public class BattleForgeSettings
    {
        private static (BattleForgeSettingAttribute, PropertyInfo)[] _settings = typeof(BattleForgeSettings)
            .GetProperties()
            .Select(x => (x.GetCustomAttribute<BattleForgeSettingAttribute>(), x))
            .Where(x => x.Item1 != null)
            .ToArray();

        //Server
        public string ServerAddress { get; set; } = "127.0.0.1";
        public int ServerPort { get; set; } = 7399;

        [BattleForgeSetting("network", "serveruri")]
        public string ServerURI => $"{ServerAddress}:{ServerPort}";

        //Visual
        [BattleForgeSetting("application", "screenwidth")]
        public int ScreenWidth { get; set; } = 1280;
        [BattleForgeSetting("application", "screenheight")]
        public int ScreenHeight { get; set; } = 720;
        [BattleForgeSetting("application", "fullscreen")]
        public bool Fullscreen { get; set; } = false;

        public string ScreenResolution
        {
            get
            {
                return $"{ScreenWidth}x{ScreenHeight}";
            }
            set
            {
                if (value == null || !value.Contains("x"))
                    throw new ArgumentException("Invalid Resolution format");
                string[] parts = value.Split('x');
                ScreenWidth = int.Parse(parts[0]);
                ScreenHeight = int.Parse(parts[1]);
            }
        }

        //Utility
        [BattleForgeSetting("gui", "skip_intro_movies")]
        public bool SkipIntro { get; set; } = false;

        [BattleForgeSetting("gui", "ea_movie_watched")]
        public bool SkipEA { get; set; } = false;
        //Audio
        [BattleForgeSetting("application", "globalvolume")]
        public float VolumeGlobal { get; set; } = 0.7f;

        [BattleForgeSetting("application", "fx3dvolume")]
        public float VolumeFX3D { get; set; } = 0.7f;
        
        [BattleForgeSetting("application", "musicvolume")]
        public float VolumeMusic { get; set; } = 0.7f;

        [BattleForgeSetting("application", "speechvolume")]
        public float VolumeSpeech { get; set; } = 0.7f;

        [BattleForgeSetting("application", "ambientvolume")]
        public float VolumeAmbient { get; set; } = 0.7f;

        [BattleForgeSetting("application", "uivolume")]
        public float VolumeUI { get; set; } = 0.7f;



        public string UpdateConfig(string config)
        {
            //Not optimal, but works.
            foreach (var val in GetBattleForgeValues())
                config = ModifyProperty(config, val.Item1.Category, val.Item1.Property, val.Item2);
            return config;
        }

        public void LoadValuesFromConfig(string xml)
        {
            foreach (var setting in _settings)
            {
                BattleForgeSettingAttribute attr = setting.Item1;
                PropertyInfo prop = setting.Item2;

                if (prop.SetMethod == null)
                    continue;

                string value = GetProperty(xml, attr.Category, attr.Property);

                if (value != null)
                {
                    if (prop.PropertyType == typeof(bool))
                        switch (value)
                        {
                            case "0":
                                prop.SetValue(this, false);
                                break;
                            case "1":
                                prop.SetValue(this, true);
                                break;
                            default:
                                prop.SetValue(this, Convert.ToBoolean(value));
                                break;
                        }
                    else if (prop.PropertyType == typeof(int))
                        prop.SetValue(this, Convert.ToInt32(value));
                    else if (prop.PropertyType == typeof(float))
                        prop.SetValue(this, Convert.ToSingle(value));
                    else if (prop.PropertyType == typeof(string))
                        prop.SetValue(this, value);
                }
            }
        }

        private List<(BattleForgeSettingAttribute, string)> GetBattleForgeValues()
        {
            List<(BattleForgeSettingAttribute, string)> values = new List<(BattleForgeSettingAttribute, string)>();

            foreach (var setting in _settings)
            {
                BattleForgeSettingAttribute attr = setting.Item1;
                PropertyInfo prop = setting.Item2;

                object val = prop.GetValue(this);

                if (val != null)
                {
                    if (prop.PropertyType == typeof(bool))
                        values.Add((attr, ((bool)val) ? "1" : "0"));
                    else if (prop.PropertyType.IsPrimitive || prop.PropertyType == typeof(string))
                        values.Add((attr, val.ToString()));
                }
            }

            return values;
        }

        private static string GetProperty(string configXml, string category, string property)
        {
            Regex regex = new Regex($"<{category}.*{property}=\"(.*?)\"");

            var match = regex.Match(configXml);

            if (!match.Success)
                throw new ArgumentException("Failed to find value in configuration.");

            return match.Groups[1].Value;
        }
        private static string ModifyProperty(string configXml, string category, string property, string value)
        {
            Regex regex = new Regex($"<{category}.*{property}=\"(.*?)\"");

            var match = regex.Match(configXml);

            if (!match.Success)
                throw new ArgumentException("Failed to find value in configuration.");

            var prefix = configXml.Substring(0, match.Groups[1].Index);
            var offset = match.Groups[1].Index + match.Groups[1].Length;
            var suffix = configXml[offset..];

            configXml = $"{prefix}{value}{suffix}";

            return configXml;
        }
    }
    public class BattleForgeSettingAttribute : Attribute
    {

        public string Category { get; set; }
        public string Property { get; set; }


        public BattleForgeSettingAttribute(string cat, string property)
        {
            Category = cat;
            Property = property;
        }
    }
}
