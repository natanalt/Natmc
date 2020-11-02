using Natmc.Graphics;
using Natmc.Input;
using Natmc.Logging;
using Natmc.Platform;
using Natmc.Platform.OTK;
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
        private static readonly LogScope Log = new LogScope("Engine");

        public const string Version = "0.1.0";
        public const string McVersion = "1.16.3";

        public static IPlatform Platform { get; private set; }
        public static StatedWindow MainWindow { get; private set; }
        public static IRenderingApi RenderingApi => MainWindow.RenderingApi;

        public static void Start()
        {
            Thread.CurrentThread.Name = "MainThread";

            Logger.Init();
            Logger.AddLogOutput<ConsoleLogOutput>();

            Log.Info($"Starting Natmc {Version}");
            Log.Info($"Target Minecraft version {McVersion}");

            Filesystem.Init();
            SettingsManager.Init();
            InputManager.Init();

            Log.Info($"Initializing platform");
            Platform = new OpenTKPlatform();

            Log.Info($"Creating main window");
            MainWindow = Platform.CreateWindow("Natmc", new Vector2i(800, 600), new LoaderState());
            MainWindow.Run();
        }

        public static void UpdateMainWindowTitle()
        {
            MainWindow.Title = $"Natmc [{MainWindow.Fps} FPS] - {RenderingApi.DetailedName} - {MainWindow.CurrentState.GetType().Name}";
        }
    }
}
