using Natmc.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Core
{
    public abstract class GameState
    {
        public StatedWindow Window { get; set; }
        public GfxRenderer Renderer => Window.Renderer;

        public abstract void OnEnable();
        public abstract void OnDisable();
        public abstract void OnResize(int nw, int nh);
        public abstract void OnUpdate(float delta);
        public abstract void OnRender(float delta);
    }
}
