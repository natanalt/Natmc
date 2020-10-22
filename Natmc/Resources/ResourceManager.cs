using Natmc.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Resources
{
    public static class ResourceManager
    {
        private static readonly LogScope Log = new LogScope("ResourceManager");

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

            // TODO: move this somewhere else
            LoadResources(new List<string> { "default.jar" });
        }

        public static void Unload()
        {
            Languages.Clear();
            LoadedPacks.Clear();
        }

        /// <summary>
        /// Begins loading resources in current thread.
        /// </summary>
        /// <param name="packFilenames">Pack file or directory names</param>
        public static void LoadResources(List<string> packFilenames)
        {
            Log.Info("Resource load requested. Clearing loaded data");
            Unload();

            foreach (var packFilename in packFilenames)
            {
                var pack = new ResourcePack(packFilename);
            }
        }
    }
}
