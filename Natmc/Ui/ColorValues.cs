using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Ui
{
    public static class ColorValues
    {
        // https://wiki.vg/Chat#Colors
        // TODO: make those values customizable?

        public static readonly Dictionary<McColor, Color4> ForegroundValues = new Dictionary<McColor, Color4>
        {
            { McColor.Black, new Color4(0, 0, 0, 255) },
            { McColor.DarkBlue, new Color4(0, 0, 170, 255) },
            { McColor.DarkGreen, new Color4(0, 170, 0, 255) },
            { McColor.DarkAqua, new Color4(0, 170, 170, 255) },
            { McColor.DarkRed, new Color4(170, 0, 0, 255) },
            { McColor.DarkPurple, new Color4(170, 0, 170, 255) },
            { McColor.Gold, new Color4(255, 170, 0, 255) },
            { McColor.Gray, new Color4(170, 170, 170, 255) },
            { McColor.DarkGray, new Color4(85, 85, 85, 255) },
            { McColor.Blue, new Color4(85, 85, 255, 255) },
            { McColor.Green, new Color4(85, 255, 85, 255) },
            { McColor.Aqua, new Color4(85, 255, 255, 255) },
            { McColor.Red, new Color4(255, 85, 85, 255) },
            { McColor.LightPurple, new Color4(255, 85, 255, 255) },
            { McColor.Yellow, new Color4(255, 255, 85, 255) },
            { McColor.White, new Color4(255, 255, 255, 255) },
        };

        public static readonly Dictionary<McColor, Color4> BackgroundValues = new Dictionary<McColor, Color4>
        {
            { McColor.Black, new Color4(0, 0, 0, 255) },
            { McColor.DarkBlue, new Color4(0, 0, 42, 255) },
            { McColor.DarkGreen, new Color4(0, 42, 0, 255) },
            { McColor.DarkAqua, new Color4(0, 42, 42, 255) },
            { McColor.DarkRed, new Color4(42, 0, 0, 255) },
            { McColor.DarkPurple, new Color4(42, 0, 42, 255) },
            { McColor.Gold, new Color4(42, 42, 0, 255) },
            { McColor.Gray, new Color4(42, 42, 42, 255) },
            { McColor.DarkGray, new Color4(21, 21, 21, 255) },
            { McColor.Blue, new Color4(21, 21, 63, 255) },
            { McColor.Green, new Color4(21, 63, 63, 255) },
            { McColor.Aqua, new Color4(21, 63, 63, 255) },
            { McColor.Red, new Color4(63, 21, 21, 255) },
            { McColor.LightPurple, new Color4(63, 21, 63, 255) },
            { McColor.Yellow, new Color4(63, 63, 21, 255) },
            { McColor.White, new Color4(63, 63, 63, 255) },
        };

        public static readonly Dictionary<McColor, string> ColorNames = new Dictionary<McColor, string>
        {
            { McColor.Black, "black" },
            { McColor.DarkBlue, "dark_blue" },
            { McColor.DarkGreen, "dark_green" },
            { McColor.DarkAqua, "dark_aqua" },
            { McColor.DarkRed, "dark_red" },
            { McColor.DarkPurple, "dark_purple" },
            { McColor.Gold, "gold" },
            { McColor.Gray, "gray" },
            { McColor.DarkGray, "dark_gray" },
            { McColor.Blue, "blue" },
            { McColor.Green, "green" },
            { McColor.Aqua, "aqua" },
            { McColor.Red, "red" },
            { McColor.LightPurple, "light_purple" },
            { McColor.Yellow, "yellow" },
            { McColor.White, "white" },
        };

        public static McColor? NameToColor(string name)
        {
            foreach (var kv in ColorNames)
                if (kv.Value == name)
                    return kv.Key;
            return null;
        }
    }
}
