using Natmc.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Natmc.Logging
{
    public class ConsoleLogOutput : ILogOutput
    {
        protected Mutex LogMutex { get; set; }
        public Dictionary<LogType, ConsoleColor> LogColors { get; protected set; }
        public ConsoleColor ThreadModuleColor { get; set; }
        public ConsoleColor MessageColor { get; set; }

        public ConsoleLogOutput()
        {
            LogMutex = new Mutex();
            LogColors = new Dictionary<LogType, ConsoleColor>
            {
                [LogType.Info] = ConsoleColor.Cyan,
                [LogType.Warn] = ConsoleColor.Yellow,
                [LogType.Error] = ConsoleColor.Red
            };
            ThreadModuleColor = ConsoleColor.DarkGray;
            MessageColor = ConsoleColor.White;

            Console.Title = $"Natmc {Engine.Version} Debug Logs";
            Console.BufferWidth = 120;
            Console.WindowWidth = 120;
            Console.WindowHeight = 50;
        }

        public void Log(LogType type, string module, string message)
        {
            LogMutex.WaitOne();

            Console.ForegroundColor = LogColors[type];
            Console.Write($"[{type}]");

            Console.ForegroundColor = ThreadModuleColor;
            Console.Write($"[{Thread.CurrentThread.Name}][{module}] ");

            Console.ForegroundColor = MessageColor;
            Console.WriteLine(message);

            LogMutex.ReleaseMutex();
        }
    }
}
