using Natmc.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Ui
{
    public class UiManager : IDisposable
    {
        public GameState Parent { get; }
        public Widget Root { get; protected set; }

        public UiManager(GameState parent)
        {
            Widgets = new List<Widget>();
            Parent = parent;
        }

        public void Dispose()
        {
        }
    }
}
