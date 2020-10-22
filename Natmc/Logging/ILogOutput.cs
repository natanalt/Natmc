using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Logging
{
    public interface ILogOutput
    {
        public void Log(LogType type, string module, string message);
    }
}
