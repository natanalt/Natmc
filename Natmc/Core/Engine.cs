using Natmc.Graphics;
using Natmc.Input;
using Natmc.Logging;
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

        public static StatedWindow MainWindow { get; private set; }
        public static GfxRenderer Renderer => MainWindow.Renderer;

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

            Log.Info($"Creating main window");
            MainWindow = new StatedWindow("Natmc", new Vector2i(800, 600), new LoaderState());
            MainWindow.Run();
        }

        public static void UpdateMainWindowTitle()
        {
            // FIXME: OpenGL 3.3 rendering dependency
            MainWindow.Title =
                $"Natmc [{MainWindow.Fps} FPS] - " +
                $"{MainWindow.CurrentState.GetType().Name} - ";

            if (Renderer.DrawCallsPerFrame == 1)
                MainWindow.Title += $"{Renderer.DrawCallsPerFrame} draw call per frame";
            else
                MainWindow.Title += $"{Renderer.DrawCallsPerFrame} draw calls per frame";

        }
    }
}
