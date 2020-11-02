using Natmc.Core;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Platform
{
    public interface IPlatform
    {
        public StatedWindow CreateWindow(string title, Vector2i size, GameState initialState);
    }
}
