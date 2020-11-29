using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Core
{
    public class FramerateCounter : EngineObject
    {
        public bool UpdateMainWindowTitle;

        private int framesThisSecond;
        private float countedTime;

        public override void OnAdd()
        {
            UpdateMainWindowTitle = true;
            framesThisSecond = 0;
            countedTime = 0;
        }

        public override void Update(float delta)
        {
            framesThisSecond += 1;
            countedTime += delta;

            if (countedTime >= 1.0f)
            {
                Parent.FPS = framesThisSecond;
                framesThisSecond = 0;
                countedTime = 0;
            }

            if (UpdateMainWindowTitle)
            {
                Parent.MainWindow.Title = $"Natmc {Engine.Version} [{Parent.FPS} FPS]";
            }
        }
    }
}
