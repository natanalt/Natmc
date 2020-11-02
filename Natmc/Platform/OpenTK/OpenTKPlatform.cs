using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Platform.OpenTK
{
    public class OpenTKPlatform : IPlatform
    {
        public IWindow CreateWindow(string title, Vector2i size, IWindowEventDispatcher dispatcher)
            => new OpenTKWindow(title, size, dispatcher);
    }
}
