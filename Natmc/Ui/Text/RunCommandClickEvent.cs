using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Ui.Text
{
    public class RunCommandClickEvent : ClickEvent
    {
        public string Command { get; set; }

        public RunCommandClickEvent(string command)
        {
            Type = ClickEventType.RunCommand;
            Command = command;
        }

        public override void OnClick()
        {
            throw new NotImplementedException();
        }
    }
}
