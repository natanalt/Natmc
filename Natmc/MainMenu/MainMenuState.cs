using Natmc.Core;
using Natmc.Resources;
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
            RenderingApi.BeginFrame(delta);
            RenderingApi.BeginUi();
            RenderingApi.DrawColoredQuad(new Vector2(0, 0), new Vector2(100, 200), Color4.DarkSlateGray);
            RenderingApi.DrawTexturedQuad(new Vector2(100, 100), new Vector2(100, 100), Color4.White, ResourceManager.Texture.ErrorTexture);
            RenderingApi.EndUi();
            RenderingApi.EndFrame();
        }

        public override void OnResize(int nw, int nh)
        {
        }
    }
}
