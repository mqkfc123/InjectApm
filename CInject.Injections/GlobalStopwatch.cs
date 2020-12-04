using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CInject.Injections
{
    public class GlobalStopwatch
    {
        private static volatile CurrentStopwatch CurrentStopwatch;

        public static CurrentStopwatch GetStopwatch()
        {
            return CurrentStopwatch;
        }
        public static CurrentStopwatch SetStopwatch(CurrentStopwatch stopwatch)
        {
            return CurrentStopwatch = stopwatch;
        }

        public static void Instance()
        {
            CurrentStopwatch = new CurrentStopwatch();
        }

        public static void Start()
        {
            CurrentStopwatch.Start();
        }

        public static void Stop()
        {
            CurrentStopwatch.Stop();
        }

        public static void Reset()
        {
            CurrentStopwatch.Reset();
            CurrentStopwatch = null; 
        }

        public static double Elapsed()
        {
            return CurrentStopwatch.Elapsed();
        }

    }
}
