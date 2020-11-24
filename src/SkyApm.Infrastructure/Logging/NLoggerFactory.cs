using NLog;
using SkyApm.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyApm.Infrastructure.Logging
{
    public class NLoggerFactory : ILoggerFactory
    {

        public NLoggerFactory()
        {
            LogManager.LoadConfiguration($"{AppDomain.CurrentDomain.BaseDirectory}NLog.config");

        }

        public SkyApm.Logging.ILogger CreateLogger(Logger logger)
        {
            return new NLogger(logger);
        }

    }
}
