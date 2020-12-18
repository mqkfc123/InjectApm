using SkyApm.Abstractions.Context.Trace;
using SkyApm.Core.Context;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CInject.Injections
{
    public class GlobalStopwatch
    {
        private static volatile CurrentStopwatch CurrentStopwatch;

        private static volatile ISpan TraceSpan;

        public static CurrentStopwatch GetStopwatch()
        {
            return CurrentStopwatch;
        }
        public static CurrentStopwatch SetStopwatch(CurrentStopwatch stopwatch)
        {
            return CurrentStopwatch = stopwatch;
        }

        public static ISpan GetSpan()
        {
            return TraceSpan;
        }
        public static ISpan SetSpan(ISpan span)
        {
            return TraceSpan = span;
        }

        public static void Instance()
        {
            CurrentStopwatch = new CurrentStopwatch();
        }

        public static void InstanceSpan(string operationName, string action = "Entry")
        {
            if (action == "Entry")
                TraceSpan = ContextManager.CreateEntrySpan(operationName, null);
            else if (action == "Local")
                TraceSpan = ContextManager.CreateLocalSpan(operationName);
        }

        public static void Start()
        {
            CurrentStopwatch?.Start();
        }

        public static void Stop()
        {
            CurrentStopwatch?.Stop();
        }

        public static void Reset()
        {
            CurrentStopwatch.Reset();
            CurrentStopwatch = null;
            TraceSpan = null;
        }

        public static double Elapsed()
        {
            return CurrentStopwatch.Elapsed();
        }

    }
}
