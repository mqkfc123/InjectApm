
using NLog;
using System;

namespace SkyApm.Logging
{
    public interface ILoggerFactory
    {
        ILogger CreateLogger(Logger logger);
    }
}