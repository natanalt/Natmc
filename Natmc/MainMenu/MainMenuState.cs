using Natmc.Core;
using OpenTK.Mathematics;
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
            RenderingApi.BeginUi();
            RenderingApi.DrawColoredQuad(
                new Vector2(100, 100),
                new Vector2(100, 200),
                Color4.AliceBlue);
            RenderingApi.EndUi();
        }

        public override void OnResize(int nw, int nh)
        {
        }
    }
}
