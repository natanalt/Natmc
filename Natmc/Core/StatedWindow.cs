using Natmc.Graphics;
using Natmc.Logging;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Core
{
    public abstract class StatedWindow
    {
        public FramePhase CurrentPhase { get; protected set; }
        public bool IsRunning { get; protected set; }
        public Color4 ClearColor { get; set; }

        public abstract string Title { get; set; }
        public abstract Vector2i Size { get; set; }
        public abstract IRenderingApi RenderingApi { get; protected set; }

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

        public int Fps { get; protected set; }
        public long TotalFrames { get; protected set; }
        protected int FramesThisSecond;
        protected float DeltaTimer;

        public StatedWindow(GameState initialState)
        {
            ClearColor = Color4.Black;
            Fps = 0;
            TotalFrames = 0;
            FramesThisSecond = 0;
            DeltaTimer = 0;

            m_CurrentState = initialState;
            m_CurrentState.Window = this;
        }

        public abstract void ContextMakeCurrent();
        public abstract void Run();
    
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
