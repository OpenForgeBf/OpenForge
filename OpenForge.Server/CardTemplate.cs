// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace OpenForge.Server
{
    public class CardTemplate
    {
        private static List<CardTemplate> _cardTemplates = null;

        public static List<CardTemplate> CardTemplates
        {
            get
            {
                if (_cardTemplates == null)
                {
                    LoadCardTemplates();
                }

                return _cardTemplates;
            }
        }

        public int CardIndex { get; set; }
        public int Expansion { get; set; }
        public bool IsPromo { get; set; }
        public string Name { get; set; }
        public int Rarity { get; set; }

        public static void LoadCardTemplates()
        {
            var cardTemplatesJson = File.ReadAllText("card_template.json");
            _cardTemplates = JsonConvert.DeserializeObject<List<CardTemplate>>(cardTemplatesJson);
        }
    }
}
