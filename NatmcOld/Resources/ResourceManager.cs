using Natmc.Logging;
using Natmc.Resources.Languages;
using Natmc.Resources.Textures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Natmc.Resources
{
    public static class ResourceManager
    {
        private static readonly LogScope Log = new LogScope("ResourceManager");

        public static LanguageManager Language { get; private set; }
        public static TextureManager Texture { get; private set; }

        public static List<IResourceManager> Managers { get; private set; }
        public static List<ResourcePack> LoadedPacks { get; private set; }
        
        public static void Init()
        {
            Managers = new List<IResourceManager>();
            LoadedPacks = new List<ResourcePack>();

            Language = Add<LanguageManager>();
            Texture = Add<TextureManager>();
        }

        public static void Deinit()
        {
            foreach (var manager in Managers)
                manager.Deinit();
        }

        public static void LoadResources(List<string> packFilenames)
        {
            Log.Info("Resource load requested");
            UnloadResources();

            foreach (var packFilename in packFilenames)
            {
                try
                {
                    var pack = new ResourcePack(Path.Combine("resourcepacks", packFilename));
                    Log.Info($"Adding resource pack {packFilename}");
                    Log.Info($"    {pack.Description.RawText}");
                    LoadedPacks.Add(pack);

                    foreach (var lang in pack.AdditionalLanguages)
                        Language.AddLanguage(lang, overwrite: false);
                }
                catch (Exception e)
                {
                    Log.Error($"Couldn't parse resource pack {packFilename}: {e.Message}");
                    throw;
                }       
            }

            foreach (var manager in Managers)
                manager.LoadResources();
        }

        public static void UnloadResources()
        {
            foreach (var manager in Managers)
                manager.UnloadResources();
            LoadedPacks.Clear();
        }

        public static T Get<T>() where T : IResourceManager => (T)Managers.Find(x => x.GetType() == typeof(T));
        
        public static T Add<T>() where T : IResourceManager
        {
            if (Get<T>() != null)
                throw new InvalidOperationException($"Resource manager of type {typeof(T).Name} already exists");

            var manager = Activator.CreateInstance<T>();
            Managers.Add(manager);
            manager.Init();
            return manager;
        }
    }
}
