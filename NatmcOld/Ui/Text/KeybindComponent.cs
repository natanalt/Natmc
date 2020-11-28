using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Ui.Text
{
    public class KeybindComponent : TextComponent
    {
        public string Keybind;

        public KeybindComponent(string keybind)
        {
            Type = TextComponentType.Keybind;
            Keybind = keybind;
        }

        public override string Resolve()
        {
            throw new NotImplementedException();
        }
    }
}
