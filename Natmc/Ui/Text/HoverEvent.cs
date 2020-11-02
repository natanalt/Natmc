using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Ui.Text
{
    public abstract class HoverEvent
    {
        public HoverEventType Type { get; protected set; }

        // TODO: This might be changed to just generating a tooltip, but tooltips aren't implemented yet
        public abstract void Display();
    }
}
