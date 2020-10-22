using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Resources
{
    public static class ResourceManager
    {
        /// <summary>
        /// Loaded packs with the lowest indices in this list take the highest
        /// priority when loading resources.
        /// </summary>
        public static List<ResourcePack> LoadedPacks;
        public static List<Language> Languages;

        public static void Init()
        {
            LoadedPacks = new List<ResourcePack>();
            Languages = new List<Language>();
        }

        /// <summary>
        /// Begins loading resources in current thread.
        /// </summary>
        /// <param name="packFilenames">Pack file or directory names</param>
        public static void LoadResources(List<string> packFilenames)
        {

        }
    }
}
