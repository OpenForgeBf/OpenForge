// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;

namespace OpenForge.Launcher
{
    public static class Extensions
    {
        //TODO: Create a neat structure for this
        public static void MapMenu(this Window window, (string, string)[] menuGroupMap, Action<Button> onActivate = null, Action<Button> onDeactivate = null, Action<string, Button> onButton = null)
        {
            List<Button> allMenuButtons = new List<Button>();
            List<Control> allViews = new List<Control>();

            foreach(var menuGroup in menuGroupMap)
            {
                Button b = window.Find<Button>(menuGroup.Item1);
                if (b == null)
                    throw new ArgumentException($"Expected button with name {menuGroup.Item1}");
                allMenuButtons.Add(b);
                onButton?.Invoke(menuGroup.Item1, b);

                Control c = window.Find<Control>(menuGroup.Item2);
                if (c == null)
                    throw new ArgumentException($"Expected view with name {menuGroup.Item2}");
                allViews.Add(c);

                b.Click += (sender, args) =>
                {
                    foreach (Control v in allViews)
                        v.IsVisible = false;

                    c.IsVisible = true;

                    foreach (Button m in allMenuButtons)
                        onDeactivate?.Invoke(m);

                    onActivate?.Invoke(b);
                };
            }
        }
    }
}
