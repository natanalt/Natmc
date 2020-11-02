using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Ui.Text
{
    public abstract class ClickEvent
    {
        public ClickEventType Type { get; protected set; }

        public abstract void OnClick();
    }
}
