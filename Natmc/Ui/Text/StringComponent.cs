using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Ui.Text
{
    public class StringComponent : TextComponent
    {
        public string Text;

        public StringComponent(string text)
        {
            Type = TextComponentType.Text;
            Text = text;
        }

        public override string Resolve()
        {
            return Text;
        }
    }
}
