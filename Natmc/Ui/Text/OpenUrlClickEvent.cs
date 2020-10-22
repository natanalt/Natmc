using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Natmc.Ui.Text
{
    public class OpenUrlClickEvent : ClickEvent
    {
        public Uri Uri { get; set; }

        public OpenUrlClickEvent(Uri uri)
        {
            Type = uri.Scheme.ToLower() == "https" || uri.Scheme.ToLower() == "http"
                ? ClickEventType.OpenUrl
                : ClickEventType.OpenFile;
            Uri = uri;
        }

        public override void OnClick()
        {
            Process.Start(Uri.AbsoluteUri);
        }
    }
}
