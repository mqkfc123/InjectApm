using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;
using System;

namespace CInject.Injections.Library
{
    internal static class Logger
    {
        private static readonly ILog Log;
        static Logger()
        {
            if (File.Exists("LogInject.log4net.xml"))
                XmlConfigurator.Configure(new FileInfo("LogInject.log4net.xml"));
            else
                BasicConfigurator.Configure();

            Log = LogManager.GetLogger("CInject");
        }

        public static bool IsDebugEnabled
        {
            get { return Log.IsDebugEnabled; }
        }

        public static void Debug(string message)
        {
            if (Log.IsDebugEnabled)
                Log.Debug(message);
        }

        public static void Info(string message)
        {
            if (Log.IsInfoEnabled)
                Log.Info(message);
        }

        public static void Error(Exception exception)
        {
            if (Log.IsErrorEnabled)
                Log.Error("An error occured while logging", exception);
        }
    }
}