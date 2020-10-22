using Natmc.Logging;
using Natmc.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc
{
    public static class Settings
    {
        private static readonly LogScope Log = new LogScope("Settings");

        public static string SelectedLanguageCode { get; set; }
        public static Language SelectedLanguage
        {
            get => ResourceManager.Languages.Find(x => x.Code == SelectedLanguageCode);
            set => SelectedLanguageCode = value.Code;
        }

        public static void Load()
        {
            SelectedLanguageCode = "en_us";
            Log.Warn("Settings loading not implemented; using default values");
        }

        public static void Save()
        {
            throw new NotImplementedException();
        }
    }
}
