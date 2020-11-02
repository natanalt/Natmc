using Natmc.Core;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Platform.OTK
{
    public class OpenTKPlatform : IPlatform
    {
        public StatedWindow CreateWindow(string title, Vector2i size, GameState initialState)
        {
            return new OpenTKWindow(title, size, initialState);
        }
    }
}
