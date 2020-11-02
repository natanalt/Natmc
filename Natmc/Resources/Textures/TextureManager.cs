using Natmc.Core;
using Natmc.Graphics;
using Natmc.Logging;
using Natmc.Utils;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Resources.Textures
{
    public class TextureManager : IResourceManager
    {
        private static readonly LogScope Log = new LogScope("TextureManager");

        public static readonly Color4 ErrorColorA = new Color4(0xF8, 0x00, 0xF8, 0xFF);
        public static readonly Color4 ErrorColorB = new Color4(0x00, 0x00, 0x00, 0xFF);

        public Dictionary<NamespacedId, ITexture> LoadedTextures { get; protected set; }
        public ITexture ErrorTexture { get; protected set; }

        public void Init()
        {
            LoadedTextures = new Dictionary<NamespacedId, ITexture>();
            
            var errorTextureData = new byte[16 * 16 * 4];
            for (var x = 0; x < 16 / 2; x += 1)
            {
                for (var y = 0; y < 16 / 2; y += 1)
                {
                    errorTextureData[y * 16 * 4 + x * 4 + 0] = (byte)(ErrorColorA.R * 255);
                    errorTextureData[y * 16 * 4 + x * 4 + 1] = (byte)(ErrorColorA.G * 255);
                    errorTextureData[y * 16 * 4 + x * 4 + 2] = (byte)(ErrorColorA.B * 255);
                    errorTextureData[y * 16 * 4 + x * 4 + 3] = (byte)(ErrorColorA.A * 255);
                    errorTextureData[(8 + y) * 16 * 4 + (8 + x) * 4 + 0] = (byte)(ErrorColorA.R * 255);
                    errorTextureData[(8 + y) * 16 * 4 + (8 + x) * 4 + 1] = (byte)(ErrorColorA.G * 255);
                    errorTextureData[(8 + y) * 16 * 4 + (8 + x) * 4 + 2] = (byte)(ErrorColorA.B * 255);
                    errorTextureData[(8 + y) * 16 * 4 + (8 + x) * 4 + 3] = (byte)(ErrorColorA.A * 255);
                    errorTextureData[y * 16 * 4 + (8 + x) * 4 + 0] = (byte)(ErrorColorB.R * 255);
                    errorTextureData[y * 16 * 4 + (8 + x) * 4 + 1] = (byte)(ErrorColorB.G * 255);
                    errorTextureData[y * 16 * 4 + (8 + x) * 4 + 2] = (byte)(ErrorColorB.B * 255);
                    errorTextureData[y * 16 * 4 + (8 + x) * 4 + 3] = (byte)(ErrorColorB.A * 255);
                    errorTextureData[(8 + y) * 16 * 4 + x * 4 + 0] = (byte)(ErrorColorB.R * 255);
                    errorTextureData[(8 + y) * 16 * 4 + x * 4 + 1] = (byte)(ErrorColorB.G * 255);
                    errorTextureData[(8 + y) * 16 * 4 + x * 4 + 2] = (byte)(ErrorColorB.B * 255);
                    errorTextureData[(8 + y) * 16 * 4 + x * 4 + 3] = (byte)(ErrorColorB.A * 255);
                }
            }
            ErrorTexture = Engine.RenderingApi.CreateTexture(errorTextureData, 16, 16);
        }

        public void Deinit()
        {
            ErrorTexture.Dispose();
        }

        public void LoadResources() { }
        
        public void UnloadResources()
        {
            foreach (var kv in LoadedTextures)
                kv.Value.Dispose();
            LoadedTextures.Clear();
        }

        public ITexture Get(NamespacedId id, bool loadIfUnloaded = true)
        {
            if (!LoadedTextures.ContainsKey(id))
            {
                if (!loadIfUnloaded)
                    return ErrorTexture;
                LoadTexture(id);
            }
            return LoadedTextures[id];
        }

        public void LoadTexture(NamespacedId id)
        {
            if (LoadedTextures.ContainsKey(id))
            {
                Log.Info($"Texture {id} is already loaded. Reloading...");
                LoadedTextures[id].Dispose();  
            }

            try
            {
                var data = ImageLoader.Load(ResolveTexturePath(id));
                if (data == null)
                    throw new Exception("Not found");

                LoadedTextures[id] = Engine.RenderingApi.CreateTexture(
                    data.Value.Data,
                    data.Value.Width,
                    data.Value.Height);
            }
            catch (Exception e)
            {
                Log.Error($"Couldn't load texture {id}");
                Log.Error($"{e.GetType().Name}: {e.Message}");
                Log.Error($"{e.StackTrace}");
                LoadedTextures[id] = ErrorTexture;
#if DEBUG
                throw;
#endif
            }
        }

        public string ResolveTexturePath(NamespacedId id, string prefix = "")
        {
            return $"assets/{id.Namespace}/textures/{prefix}{id.Id}.png";
        }
    }
}
