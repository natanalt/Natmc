using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Graphics
{
    public interface ITexture : IDisposable
    {
        public int Width { get; }
        public int Height { get; }

        public Vector2 PixelToNormalized(Vector2i coords)
            => new Vector2((float)coords.X / Width, (float)coords.Y / Height);
    }
}
