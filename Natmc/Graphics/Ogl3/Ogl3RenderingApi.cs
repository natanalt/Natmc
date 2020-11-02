using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Graphics.Ogl3
{
    public class Ogl3RenderingApi : IRenderingApi
    {
        public string Name => "OGL3";
        public virtual string DetailedName => "Natmc OpenGL 3.3 Renderer";

        public List<Ogl3Texture> Textures { get; protected set; }

        public void Init()
        {
            Textures = new List<Ogl3Texture>();
        }

        public void Deinit()
        {
            foreach (var texture in Textures)
            {
                if (texture.Valid)
                    texture.Dispose();
            }
        }

        public ITexture CreateTexture(byte[] rawData, int width, int height)
        {
            var texture = new Ogl3Texture(width, height, rawData);
            Textures.Add(texture);
            return texture;
        }

        public void BeginFrame()
        {
        }

        public void EndFrame()
        {
        }

        public void BeginUi()
        {

        }
        
        public void RenderUntexturedQuad(Vector2i position, Vector2i size, Color4 color)
        {

        }
        
        public void RenderTexturedQuad(
            Vector2i position,
            Vector2i size,
            ITexture texture,
            Vector2i? textureOriginOpt = null,
            Vector2i? textureSizeOpt = null)
        {

        }

        public void EndUi()
        {

        }

        public void BeginWorld()
        {

        }

        public void RenderSky()
        {

        }

        public void RenderChunk(/*...*/)
        {

        }

        public void RenderEntity(/*...*/)
        {

        }

        public void EndWorld()
        {

        }
    }
}
