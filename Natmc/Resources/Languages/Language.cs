using Natmc.Logging;
using Natmc.Settings;
using Natmc.Ui.Text;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Resources.Languages
{
    public class Language
    {
        private static readonly LogScope Log = new LogScope("LanguageManager");

        public static Language Current => ResourceManager.Language.FindByCode(SettingsManager.SelectedLanguageCode);

        public string Code;
        public string Name;
        public string Region;
        public bool IsBidirectional;

        public string FormattedName => $"{Name} ({Region}, {Code})";

        public bool LoadedStrings { get; protected set; }
        public Dictionary<string, string> Strings { get; protected set; }

        public Language()
        {
            LoadedStrings = false;
            Strings = new Dictionary<string, string>();
        }

        public void LoadStrings()
        {
            UnloadStrings();

            Log.Info($"Loading translation strings for {FormattedName}");
            var languageFilePath = $"assets/minecraft/lang/{Code}.json";
            foreach (var pack in ResourceManager.LoadedPacks)
            {
                var reader = pack.PackReader;
                if (!reader.FileExists(languageFilePath))
                    continue;

                var languageData = JObject.Parse(reader.ReadTextFile(languageFilePath));
                if (languageData.Type != JTokenType.Object)
                    throw new FormatException($"Invalid language file for code {Code} for resource pack {pack.Id}");

                foreach (var kv in languageData.Value<JObject>())
                {
                    if (Strings.ContainsKey(kv.Key))
                        continue;
                    Strings[kv.Key] = kv.Value.Value<string>();
                }
            }
            LoadedStrings = true;
        }

        public void UnloadStrings()
        {
            Strings.Clear();
            LoadedStrings = false;
        }

        public string FormatPlainString(string id, string[] parameters)
        {
            var components = new TextComponent[parameters.Length];
            for (var i = 0; i < parameters.Length; i += 1)
                components[i] = new StringComponent(parameters[i]);
            return FormatString(id, components).RawText;
        }

        public TextComponent FormatString(string id, TextComponent[] parameters)
        {
            // TODO: language string formatting
            return new StringComponent(id);
        }
    }
}
