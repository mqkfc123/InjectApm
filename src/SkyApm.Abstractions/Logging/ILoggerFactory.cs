
using System;

namespace SkyApm.Logging
{
    public interface ILoggerFactory
    {
        ILogger CreateLogger(Type type);
    }
}