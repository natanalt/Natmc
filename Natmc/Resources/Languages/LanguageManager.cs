using Natmc.Logging;
using Natmc.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Resources.Languages
{
    public class LanguageManager : IResourceManager
    {
        public List<Language> Languages { get; private set; }

        public void Init()
        {
            Languages = new List<Language>();
        }

        public void Deinit() { }

        public void LoadResources()
        {
            Language.Current.LoadStrings();
        }

        public void UnloadResources()
        {
            foreach (var lang in Languages)
                lang.UnloadStrings();
        }

        public Language FindByCode(string code) => Languages.Find(x => x.Code.Equals(code, StringComparison.OrdinalIgnoreCase));

        public void AddLanguage(Language lang, bool overwrite = true)
        {
            var existing = FindByCode(lang.Code);
            if (existing != null)
            {
                if (!overwrite)
                    return;
                Languages.Remove(existing);
            }
            Languages.Add(lang);
            if (overwrite && SettingsManager.SelectedLanguageCode == lang.Code)
                lang.LoadStrings();
        }
    }
}
