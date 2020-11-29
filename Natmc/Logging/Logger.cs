using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Natmc.Logging
{
    public static class Logger
    {
        public static List<ILogOutput> LogOutputs { get; private set; }
        public static Mutex LogMutex { get; private set; }

        public static void Init()
        {
            LogOutputs = new List<ILogOutput>();
            LogMutex = new Mutex();
        }

        public static void Log(LogType type, string module, string message)
        {
            LogMutex.WaitOne();
            foreach (var output in LogOutputs)
                output.Log(type, module, message);
            LogMutex.ReleaseMutex();
        }

        public static void AddLogOutput<T>() => LogOutputs.Add((ILogOutput)Activator.CreateInstance<T>());
        public static void AddLogOutput(ILogOutput logOutput) => LogOutputs.Add(logOutput);

        public static void Info(string module, string message, params object[] o)
            => Log(LogType.Info, module, string.Format(message, o));
        public static void Warn(string module, string message, params object[] o)
            => Log(LogType.Warn, module, string.Format(message, o));
        public static void Error(string module, string message, params object[] o)
            => Log(LogType.Error, module, string.Format(message, o));
    }
}
