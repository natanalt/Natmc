using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Core
{
    public interface IGameState
    {
        public void OnEnable() { }
        public void OnDisable() { }
        public void OnResize(int nw, int nh) { }
        public void OnUpdate(float delta) { }
        public void OnRender(float delta) { }
    }
}
