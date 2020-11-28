using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Resources.Textures
{
    public struct ImageData
    {
        public int Width;
        public int Height;
        public byte[] Data;
        public bool Valid => Data.Length == (Width * Height * 4);
    }
}
