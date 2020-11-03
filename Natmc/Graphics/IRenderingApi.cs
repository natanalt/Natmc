using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Graphics
{
    public interface IRenderingApi
    {
        public string Name { get; }
        public virtual string DetailedName => Name;

        public void Init();
        public void Deinit();

        public ITexture CreateTexture(byte[] rawData, int width, int height);

        public void BeginFrame();
        public void EndFrame();

        public void BeginUi();
        public void DrawColoredQuad(Vector2 position, Vector2 size, Color4 color);
        public void DrawTexturedQuad(
            Vector2 position,
            Vector2 size,
            ITexture texture,
            Vector2? textureOrigin = null,
            Vector2? textureSize = null);
        public void EndUi();

        public void BeginWorld();
        public void DrawSky();
        public void DrawChunk(/*...*/);
        public void DrawEntity(/*...*/);
        public void EndWorld();
    }
}
