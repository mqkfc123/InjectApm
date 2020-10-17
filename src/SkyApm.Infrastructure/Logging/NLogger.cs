using SkyApm.Logging;
using System;
using NLoggers = NLog.Logger;

namespace SkyApm.Infrastructure.Logging
{
    internal class NLogger : ILogger
    {
        private readonly NLoggers _readLogger;

        public NLogger(NLoggers readLogger)
        {
            _readLogger = readLogger;
        }

        public void Debug(string message)
        {
            _readLogger.Debug(message);
        }

        public void Information(string message)
        {
            _readLogger.Info(message);
        }

        public void Warning(string message)
        {
            _readLogger.Warn(message);
        }

        public void Error(string message, Exception exception)
        {
            _readLogger.Error(message + Environment.NewLine + exception);
        }

        public void Trace(string message)
        {
            _readLogger.Trace(message);
        }

    }
}
