using System;
using Natmc.Core;
using Natmc.Graphics;
using Natmc.Graphics.Ogl3;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Natmc.Platform.OTK
{
    public class OpenTKWindow : StatedWindow
    {
        public GameWindow InternalWindow { get; protected set; }

        public override string Title { get => InternalWindow.Title; set => InternalWindow.Title = value; }
        public override Vector2i Size { get => InternalWindow.Size; set => InternalWindow.Size = value; }
        public override IRenderingApi RenderingApi { get; protected set; }

        public OpenTKWindow(string title, Vector2i size, GameState state) : base(state)
        {
            InternalWindow = new GameWindow(
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
                    Title = title,
                    Size = size
                });

            InternalWindow.Load += () =>
            {
                GL.Enable(EnableCap.DepthTest);
                RenderingApi = new Ogl3RenderingApi(this);
                RenderingApi.Init();
                CurrentState.OnEnable();
            };

            InternalWindow.Unload += () =>
            {
                CurrentState.OnDisable();
                RenderingApi.Deinit();
            };

            InternalWindow.Resize += (args) =>
            {
                GL.Viewport(0, 0, args.Width, args.Height);
                CurrentState.OnResize(args.Width, args.Height);
            };

            InternalWindow.UpdateFrame += (args) =>
            {
                UpdateFramerate((float)args.Time);
                CurrentState.OnUpdate((float)args.Time);
            };

            InternalWindow.RenderFrame += (args) =>
            {
                GL.ClearColor(ClearColor);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                CurrentState.OnRender((float)args.Time);
                InternalWindow.SwapBuffers();
            };
        }

        public override void Run() => InternalWindow.Run();
    }
}
