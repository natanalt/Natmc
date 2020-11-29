using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Graphics
{
    public class BufferObject : IDisposable
    {
        public int Handle { get; protected set; }

        public BufferObject()
        {
            Handle = GL.GenBuffer();
        }

        public void Dispose()
        {
            GL.DeleteBuffer(Handle);
            Handle = -1;
        }
    }
}
