using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Core
{
    public static class Engine
    {
        public const string Version = "0.1.0";
        public const string McVersion = "1.16.3";

        public static MainWindow Window;

        public static void Start()
        {
            Window = new MainWindow();
            Window.Run();
        }
    }
}
