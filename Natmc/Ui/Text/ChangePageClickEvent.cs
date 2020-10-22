using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Ui.Text
{
    public class ChangePageClickEvent : ClickEvent
    {
        public int TargetPage { get; set; }

        public ChangePageClickEvent(int targetPage)
        {
            Type = ClickEventType.ChangePage;
            TargetPage = targetPage;
        }

        public override void OnClick()
        {
            throw new NotImplementedException();
        }
    }
}
