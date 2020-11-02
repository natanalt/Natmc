using Natmc.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.MainMenu
{
    public class MainMenuState : GameState
    {
        public override void OnEnable()
        {
        }

        public override void OnDisable()
        {
        }

        public override void OnUpdate(float delta)
        {
            Engine.UpdateMainWindowTitle();
        }

        public override void OnRender(float delta)
        {
        }

        public override void OnResize(int nw, int nh)
        {
        }
    }
}
