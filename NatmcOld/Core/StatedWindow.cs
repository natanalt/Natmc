using Natmc.Graphics;
using Natmc.Logging;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Core
{
    public class StatedWindow : GameWindow
    {
        public FramePhase CurrentPhase { get; protected set; }
        public bool IsRunning { get; protected set; }
        public Color4 ClearColor { get; set; }

        public int Fps { get; protected set; }
        public long TotalFrames { get; protected set; }
        protected int FramesThisSecond;
        protected float DeltaTimer;

        public GfxRenderer Renderer { get; protected set; }

        protected bool ChangedStateInUpdate;
        private GameState m_CurrentState;
        public virtual GameState CurrentState
        {
            get => m_CurrentState;
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                if (CurrentPhase != FramePhase.Update)
                    throw new InvalidOperationException("Can't change game state in non update phase");

                m_CurrentState.OnDisable();

                m_CurrentState = value;
                m_CurrentState.Window = this;

                if (IsRunning)
                {
                    m_CurrentState.OnEnable();
                    ChangedStateInUpdate = true;
                }
            }
        }

        public StatedWindow(string title, Vector2i size, GameState initialState)
            : base(new GameWindowSettings
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
            Fps = 0;
            TotalFrames = 0;
            FramesThisSecond = 0;
            DeltaTimer = 0;

            m_CurrentState = initialState;
            m_CurrentState.Window = this;

            Load += () =>
            {
                GL.Enable(EnableCap.DepthTest);
                Renderer = new GfxRenderer(this);
                Renderer.Init();
                CurrentState.OnEnable();
            };

            Unload += () =>
            {
                CurrentState.OnDisable();
                Renderer.Deinit();
            };

            Resize += (args) =>
            {
                GL.Viewport(0, 0, args.Width, args.Height);
                CurrentState.OnResize(args.Width, args.Height);
            };

            UpdateFrame += (args) =>
            {
                UpdateFramerate((float)args.Time);
                CurrentState.OnUpdate((float)args.Time);
            };

            RenderFrame += (args) =>
            {
                GL.ClearColor(ClearColor);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                CurrentState.OnRender((float)args.Time);
                SwapBuffers();
            };

            KeyDown += (args) => { };
        }
    
        public void UpdateFramerate(float addedTime)
        {
            TotalFrames += 1;
            FramesThisSecond += 1;
            DeltaTimer += addedTime;

            if (DeltaTimer >= 1)
            {
                Fps = FramesThisSecond;
                FramesThisSecond = 0;
                DeltaTimer -= 1;
            }
        }
    }
}
