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

        public static void Init()
        {
            LoadedPacks = new List<ResourcePack>();
        }
    }
}
