using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SkyApm.Core.Common
{
    internal static class CpuHelpers
    {
        private const int interval = 1000;
        private static double _usagePercent;
        private static readonly Task _task;

        public static double UsagePercent => _usagePercent;

        static CpuHelpers()
        {
            var process = Process.GetCurrentProcess();
            _task = Task.Factory.StartNew(async () =>
            {
                var _prevCpuTime = process.TotalProcessorTime.TotalMilliseconds;
                while (true)
                {
                    var prevCpuTime = _prevCpuTime;
                    var currentCpuTime = process.TotalProcessorTime;
                    var usagePercent = (currentCpuTime.TotalMilliseconds - prevCpuTime) / interval;
                    Interlocked.Exchange(ref _prevCpuTime, currentCpuTime.TotalMilliseconds);
                    Interlocked.Exchange(ref _usagePercent, usagePercent);
                    await Task.Delay(1000);
                }
            });
        }

    }
}