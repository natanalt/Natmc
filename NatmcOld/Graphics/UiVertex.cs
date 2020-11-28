using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Natmc.Graphics
{
    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public struct UiVertex
    {
        public Vector2 Position;
        public Color4 Color;
        public Vector2 TextureCoords;
        public int SamplerIndex;

        public UiVertex(Vector2 position, Color4 color, Vector2? textureCoords = null, int? samplerIndex = null)
        {
            Position = position;
            Color = color;
            TextureCoords = textureCoords ?? Vector2.Zero;
            SamplerIndex = samplerIndex ?? 0;
        }
    }
}
