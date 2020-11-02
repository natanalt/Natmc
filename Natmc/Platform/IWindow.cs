using Natmc.Graphics;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Platform
{
    public interface IWindow
    {
        public string Title { get; set; }
        public Vector2i Size { get; set; }
        public Color4 ClearColor { get; set; }
        public IWindowEventDispatcher EventDispatcher { get; }
        public IRenderingApi RenderingApi { get; }

        public void Run();
    }
}
