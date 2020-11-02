using Natmc.MainMenu;
using Natmc.Resources;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Natmc.Core
{
    public class LoaderState : IGameState
    {
        public MainWindow Window { get; set; }
        public Thread LoadingThread;

        public void OnEnable()
        {
            LoadingThread = new Thread(() =>
            {
                ResourceManager.Init();
                ResourceManager.LoadResources(new List<string> { "minecraft" });
            }) { Name = "LoadingThread" };
            LoadingThread.Start();
        }

        public void OnUpdate(float delta)
        {
            if (!LoadingThread.IsAlive)
            {
                Window.CurrentState = new MainMenuState();
            }
        }

        public void OnDisable() { }
        public void OnResize(int nw, int nh) { }
        public void OnRender(float delta) { }
    }
}
