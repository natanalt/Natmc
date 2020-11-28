using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Logging
{
    public class LogScope
    {
        public string Module { get; protected set; }

        public LogScope(string module) => Module = module;

        public void Info(string message, params object[] o)
           => Logger.Info(Module, string.Format(message, o));
        public void Warn(string message, params object[] o)
           => Logger.Warn(Module, string.Format(message, o));
        public void Error(string message, params object[] o)
           => Logger.Error(Module, string.Format(message, o));
    }
}
