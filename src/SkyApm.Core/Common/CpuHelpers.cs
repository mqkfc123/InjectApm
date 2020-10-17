using System;
using System.Diagnostics;
using System.Threading;

namespace SkyApm.Core.Common
{
    internal static class CpuHelpers
    {
        private const int interval = 1000;
        private static double _usagePercent;
        private static readonly Thread _task;

        public static double UsagePercent => _usagePercent;

        static CpuHelpers()
        {
            var process = Process.GetCurrentProcess();
            
            _task = new Thread(new ParameterizedThreadStart(ThreadMethod));
            _task.Start(process);

        }

        static void ThreadMethod(object process)
        {
            Process _process = (Process)process;

           var _prevCpuTime = _process.TotalProcessorTime.TotalMilliseconds;
            while (true)
            {
                var prevCpuTime = _prevCpuTime;
                var currentCpuTime = _process.TotalProcessorTime;
                var usagePercent = (currentCpuTime.TotalMilliseconds - prevCpuTime) / interval;
                Interlocked.Exchange(ref _prevCpuTime, currentCpuTime.TotalMilliseconds);
                Interlocked.Exchange(ref _usagePercent, usagePercent);
                Thread.Sleep(1000);
            }
        }

    }
}