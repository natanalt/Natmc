using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Logging
{
    public static class Logger
    {
        public static List<ILogOutput> LogOutputs { get; private set; }

        public static void Init()
        {
            LogOutputs = new List<ILogOutput>();
        }

        public static void Log(LogType type, string module, string message)
        {
            foreach (var output in LogOutputs)
            {
                output.Log(type, module, message);
            }
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
