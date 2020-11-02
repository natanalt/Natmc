using Natmc.Graphics;
using Natmc.Input;
using Natmc.Logging;
using Natmc.Platform;
using Natmc.Platform.OpenTK;
using Natmc.Resources;
using Natmc.Settings;
using OpenTK.Mathematics;
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

        public static IPlatform Platform;
        public static IWindow Window;
        public static MainWindow MainWindow => (MainWindow)Window.EventDispatcher;

        public static int Fps => MainWindow.Fps;
        public static IRenderingApi RenderingApi => Window.RenderingApi;

        public static void Start()
        {
            Thread.CurrentThread.Name = "MainThread";
            Platform = new OpenTKPlatform();

            Logger.Init();
            Logger.AddLogOutput<ConsoleLogOutput>();

            Filesystem.Init();
            SettingsManager.Init();
            InputManager.Init();

            Window = Platform.CreateWindow("Natmc", new Vector2i(1024, 768), new MainWindow());
            MainWindow.CurrentState = new LoaderState();
            Window.Run();
        }
    }
}
