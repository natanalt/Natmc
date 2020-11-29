using Natmc.Core;
using Natmc.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Graphics
{
    public class GlRenderer : IDisposable
    {
        private static readonly LogScope log = new LogScope("Renderer");

        public List<Texture> Textures { get; protected set; }

        public void Init()
        {
            Textures = new List<Texture>();
        }

        public void Dispose()
        {
            log.Info("Disposing textures...");
            foreach (var texture in Textures)
                texture.Dispose();
            Textures.Clear();
        }

        public Texture AddTexture(byte[] data, int width, int height)
        {
            if (data.Length != width * height)
                throw new ArgumentException("data.Length != width * height");

            var result = new Texture(width, height, data);
            Textures.Add(result);
            return result;
        }
    }
}
