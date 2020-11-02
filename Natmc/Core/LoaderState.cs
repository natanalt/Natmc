using Natmc.Logging;
using Natmc.MainMenu;
using Natmc.Resources;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Natmc.Core
{
    public class LoaderState : GameState
    {
        private static readonly LogScope Log = new LogScope("LoaderState");

        public Thread LoadingThread { get; protected set; }

        public override void OnEnable()
        {
            Log.Info($"Used renderer: {RenderingApi.Name} ({RenderingApi.DetailedName})");

            Log.Info($"Starting loader thread");
            LoadingThread = new Thread(() =>
            {
                ResourceManager.Init();
                ResourceManager.LoadResources(new List<string> { "minecraft" });
            }) { Name = "LoadingThread" };
            LoadingThread.Start();
        }

        public override void OnUpdate(float delta)
        {
            Engine.UpdateMainWindowTitle();

            if (!LoadingThread.IsAlive)
            {
                Window.CurrentState = new MainMenuState();
            }
        }

        public override void OnDisable() { }
        public override void OnResize(int nw, int nh) { }
        public override void OnRender(float delta) { }
    }
}
