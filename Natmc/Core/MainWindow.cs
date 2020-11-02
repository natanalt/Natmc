using Natmc.Logging;
using Natmc.Platform;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Core
{
    public class MainWindow : IWindowEventDispatcher
    {
        private static readonly LogScope Log = new LogScope("MainWindow");

        public IWindow Owner { get; set; }
        public FramePhase CurrentPhase { get; protected set; }
        public bool Started { get; protected set; }

        public int Fps;
        public long TotalFrames;
        private int FramesThisSecond;
        private float DeltaTimer;

        private bool ChangedStateInUpdate;
        private IGameState m_CurrentState;
        public IGameState CurrentState
        {
            get => m_CurrentState;
            set
            {
                if (value == null)
                    throw new ArgumentNullException();

                if (CurrentPhase != FramePhase.Update)
                    throw new InvalidOperationException("Can't change game state in non update phase");

                if (m_CurrentState != null)
                    m_CurrentState.OnDisable();
                m_CurrentState = value;
                m_CurrentState.Window = this;

                if (Started)
                {
                    m_CurrentState.OnEnable();
                    ChangedStateInUpdate = true;
                }
            }
        }

        public MainWindow()
        {
            Fps = 0;
            TotalFrames = 0;
            FramesThisSecond = 0;
            DeltaTimer = 0;
        }

        public void OnLoad()
        {
            Started = true;
            Log.Info($"Using {Owner.RenderingApi.DetailedName} as renderer");

            if (CurrentState != null)
            {
                CurrentState.Window = this;
                CurrentState.OnEnable();
            }
        }
        
        public void OnUnload()
        {
            CurrentState?.OnDisable();
            Started = false;
        }
        
        public void OnResize()
        {
            CurrentState?.OnResize(Owner.Size.X, Owner.Size.Y);
        }
        
        public void OnUpdate(float delta)
        {
            CurrentPhase = FramePhase.Update;

            TotalFrames += 1;
            FramesThisSecond += 1;
            DeltaTimer += delta;
            
            if (DeltaTimer >= 1)
            {
                Fps = FramesThisSecond;
                FramesThisSecond = 0;
                DeltaTimer -= 1;
            }

            Owner.Title = $"Natmc [{Fps} FPS] - {Owner.RenderingApi.DetailedName} - {CurrentState.GetType().Name}";
            CurrentState?.OnUpdate(delta);
        }

        public void OnRender(float delta)
        {
            CurrentPhase = FramePhase.Render;

            if (ChangedStateInUpdate)
                return;

            CurrentState?.OnRender(delta);
        }
    }
}
