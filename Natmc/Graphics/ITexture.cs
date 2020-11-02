using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Graphics
{
    public interface ITexture : IDisposable
    {
        public int Width { get; }
        public int Height { get; }
    }
}
