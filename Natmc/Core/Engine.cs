using Natmc.Graphics;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Natmc.Core
{
    public class Engine
    {
        public const string Version = "0.1.0";

        public List<EngineObject> EngineObjects { get; protected set; }
        public FramePhase CurrentPhase { get; protected set; }
        public float Time { get; set; }
        public float FPS { get; set; }

        public Thread MainThread { get; protected set; }
        public GameWindow MainWindow { get; protected set; }
        public GlRenderer Renderer { get; protected set; }

        public Engine()
        {
            EngineObjects = new List<EngineObject>();
        }

        public void Run()
        {
            MainThread = Thread.CurrentThread;
            MainThread.Name = "MainThread";

            MainWindow = new GameWindow(
                new GameWindowSettings
                {
                    IsMultiThreaded = false,
                    RenderFrequency = 60.0,
                    UpdateFrequency = 60.0,
                },
                new NativeWindowSettings
                {
                    API = ContextAPI.OpenGL,
                    APIVersion = new Version(3, 3),
                    Title = $"Natmc {Version}",
                    Size = new Vector2i(800, 600),
                });

            MainWindow.Load += () =>
            {
                Renderer = new GlRenderer();
                Renderer.Init();
                AddObject<FramerateCounter>();
            };

            MainWindow.RenderFrame += (args) =>
            {
                foreach (var obj in EngineObjects)
                    obj.Update((float)args.Time);
            };

            MainWindow.Run();
        }
        
        public void AddObject(EngineObject obj)
        {
            EngineObjects.Add(obj);
            obj.Parent = this;
            obj.OnAdd();
        }
        public void RemoveObject(EngineObject obj)
        {
            obj.OnRemove();
            obj.Parent = null;
            EngineObjects.Remove(obj);
        }
        public void AddObject<T>() where T : EngineObject => AddObject(Activator.CreateInstance<T>());
        public T GetObject<T>() where T : EngineObject => EngineObjects.Find(x => x.GetType() == typeof(T)) as T;
        public void RemoveObject<T>() where T : EngineObject => RemoveObject(GetObject<T>());
    }
}
