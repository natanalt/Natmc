using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Platform
{
    public interface IWindowEventDispatcher
    {
        public IWindow Owner { set; }

        public void OnLoad() { }
        public void OnUnload() { }
        public void OnResize() { }
        public void OnUpdate(float delta) { }
        public void OnRender(float delta) { }
    }
}
