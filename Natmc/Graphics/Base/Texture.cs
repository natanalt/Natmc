using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Natmc.Graphics.Base
{
    public class Texture : IDisposable
    {
        public int Handle { get; protected set; }

        public Texture(Stream stream)
        {
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
