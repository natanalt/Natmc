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
        public void RenderUntexturedQuad(Vector2i position, Vector2i size, Color4 color);
        public void RenderTexturedQuad(
            Vector2i position,
            Vector2i size,
            ITexture texture,
            Vector2i? textureOrigin = null,
            Vector2i? textureSize = null);
        public void EndUi();

        public void BeginWorld();
        public void RenderSky();
        public void RenderChunk(/*...*/);
        public void RenderEntity(/*...*/);
        public void EndWorld();
    }
}
