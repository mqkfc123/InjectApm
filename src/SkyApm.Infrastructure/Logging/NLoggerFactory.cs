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
            //LogManager.LoadConfiguration($"{AppDomain.CurrentDomain.BaseDirectory}NLog.config");
        }

        public ILogger CreateLogger(Type type)
        {
            return new NLogger(LogManager.GetCurrentClassLogger(type));
        }

    }
}
