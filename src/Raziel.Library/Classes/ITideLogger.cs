using System;
using System.Collections.Generic;
using System.Text;
using Raziel.Library.Models;

namespace Raziel.Library.Classes
{
    public interface ITideLogger {
        void Log(TideLog log);
    }

    public enum TideLogLevel {
        Information,
        Warning,
        Error
    }
}
