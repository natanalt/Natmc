using Natmc.Logging;
using Natmc.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Settings
{
    public static class SettingsManager
    {
        private static readonly LogScope Log = new LogScope("Settings");

        public static string SelectedLanguageCode;
        public static Dictionary<string, string> Keybinds;

        public static void Init()
        {
            SelectedLanguageCode = "en_US";
            Keybinds = new Dictionary<string, string>
            {
                ["key.attack"] = "key.mouse.left",
                ["key.use"] = "key.mouse.right",
            };
            Log.Warn("Settings loading not implemented; going with the defaults");
        }
    }
}
