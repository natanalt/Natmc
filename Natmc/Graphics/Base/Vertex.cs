using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Graphics.Base
{
    public struct Vertex
    {
        public Vector3 Position;
        public Vector4 TextureCoords;
        public uint SamplerID;
    }
}
