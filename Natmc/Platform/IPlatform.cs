using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Platform
{
    public interface IPlatform
    {
        public IWindow CreateWindow(string title, Vector2i size, IWindowEventDispatcher dispatcher);
    }
}
