using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Core
{
    public enum FramePhase
    {
        Update, Render
    }

    public class MainWindow : GameWindow
    {
        public FramePhase CurrentPhase { get; protected set; }

        public int FPS { get; protected set; }
        public ulong TotalFrames { get; protected set; }
        private int FramesThisSecond;
        private float DeltaTimer;

        private bool ChangedStates;

        private IGameState m_CurrentState;
        public IGameState CurrentState
        {
            get => m_CurrentState;
            set
            {
                if (CurrentPhase != FramePhase.Update)
                    throw new InvalidOperationException("Can't change states on render");

                if (m_CurrentState == value)
                    return;

                if (m_CurrentState != null)
                    m_CurrentState.OnDisable();
                m_CurrentState = value;
                if (m_CurrentState != null)
                    m_CurrentState.OnDisable();
                ChangedStates = true;
            }
        }

        public MainWindow() : base(
            new GameWindowSettings()
            {
                IsMultiThreaded = false,
                RenderFrequency = 60.0,
                UpdateFrequency = 60.0,
            },
            new NativeWindowSettings()
            {
                API = ContextAPI.OpenGL,
                APIVersion = new Version(3, 3),
                Size = new Vector2i(1024, 768),
                Title = "Natmc",
            })
        { }

        public override void Run()
        {
            FPS = 0;
            TotalFrames = 0;
            FramesThisSecond = 0;
            DeltaTimer = 0;
            ChangedStates = false;
            base.Run();
        }

        protected override void OnLoad()
        {
            CurrentState?.OnEnable();
            base.OnLoad();
        }

        protected override void OnUnload()
        {
            CurrentState?.OnDisable();
            base.OnUnload();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
            CurrentState?.OnResize(e.Width, e.Height);
            base.OnResize(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            DeltaTimer += (float)args.Time;
            FramesThisSecond += 1;

            if (DeltaTimer > 1)
            {
                FPS = FramesThisSecond;
                DeltaTimer = 0;
                FramesThisSecond = 0;
            }

            Title = $"Natmc [{FPS} FPS]";

            CurrentState?.OnUpdate((float)args.Time);
            base.OnUpdateFrame(args);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            if (ChangedStates)
                CurrentState?.OnRender((float)args.Time);
            base.OnRenderFrame(args);
        }
    }
}
