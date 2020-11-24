using NLog;
using NLog.Config;
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
            LogManager.Configuration = new XmlLoggingConfiguration($"{AppDomain.CurrentDomain.BaseDirectory}NLog.config");
        }

        public ILogger CreateLogger(Logger logger)
        {
            return new NLogger(logger);
        }

    }
}
