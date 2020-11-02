using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Ui.Text
{
    public class SuggestCommandClickEvent : ClickEvent
    {
        public string Command;

        public SuggestCommandClickEvent(string command)
        {
            Type = ClickEventType.SuggestCommand;
            Command = command;
        }

        public override void OnClick()
        {
            throw new NotImplementedException();
        }
    }
}
