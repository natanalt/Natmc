using Natmc.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Natmc.Core
{
    public static class Engine
    {
        public const string Version = "0.1.0";
        public const string McVersion = "1.16.3";

        public static MainWindow Window;

        public static void Start()
        {
            Thread.CurrentThread.Name = "MainThread";
            Logger.Init();
            Logger.AddLogOutput<ConsoleLogOutput>();
            Settings.Load();
            Window = new MainWindow();
            Window.Run();
        }
    }
}
