using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Graphics.Ogl3
{
    public class Ogl3Texture : ITexture
    {
        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public int GlHandle { get; protected set; }
        public bool Valid => GlHandle > 0;

        public Ogl3Texture(int width, int height, byte[] rgbaData)
        {
            Width = width;
            Height = height;

            GlHandle = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, GlHandle);
            GL.TexImage2D(
                TextureTarget.Texture2D,
                0,
                PixelInternalFormat.Rgba,
                width,
                height,
                0,
                PixelFormat.Rgba,
                PixelType.UnsignedByte,
                rgbaData);

            // TODO: more texture customization APIs (mostly wrapping setting)
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        public void BindAndActivate(int unit)
        {
            GL.BindTexture(TextureTarget.Texture2D, GlHandle);
            GL.ActiveTexture(TextureUnit.Texture0 + unit);
        }

        public void Dispose()
        {
            if (!Valid)
                throw new InvalidOperationException("Texture already unloaded");

            GL.DeleteTexture(GlHandle);
            GlHandle = 0;
        }
    }
}
