using System;
using Natmc.Graphics;
using Natmc.Graphics.Ogl3;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Natmc.Platform.OpenTK
{
    public class OpenTKWindow : GameWindow, IWindow
    {
        public IWindowEventDispatcher EventDispatcher { get; }
        public Color4 ClearColor { get; set; }
        public IRenderingApi RenderingApi { get; }

        public OpenTKWindow(string title, Vector2i size, IWindowEventDispatcher eventDispatcher) : base(
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
            })
        {
            ClearColor = Color4.Black;
            EventDispatcher = eventDispatcher;
            EventDispatcher.Owner = this;

            Unload += EventDispatcher.OnUnload;
            UpdateFrame += args => EventDispatcher.OnUpdate((float)args.Time);

            Load += () =>
            {
                GL.Enable(EnableCap.DepthTest);
                GL.Enable(EnableCap.Texture2D);
                RenderingApi.Init();
                EventDispatcher.OnLoad();
            };
            
            Resize += args =>
            {
                GL.Viewport(0, 0, args.Width, args.Height);
                EventDispatcher.OnResize();
            };

            RenderFrame += args =>
            {
                GL.ClearColor(ClearColor);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                EventDispatcher.OnRender((float)args.Time);
                SwapBuffers();
            };

            RenderingApi = new Ogl3RenderingApi();
        }
    }
}
