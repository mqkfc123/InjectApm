using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyApm.Abstractions.Transport
{
    public class CLRStatsRequest
    {
        public CPUStatsRequest CPU { get; set; }

        public GCStatsRequest GC { get; set; }

        public ThreadStatsRequest Thread { get; set; }
    }

    public class CPUStatsRequest
    {
        public double UsagePercent { get; set; }
    }

    public class GCStatsRequest
    {
        public long Gen0CollectCount { get; set; }

        public long Gen1CollectCount { get; set; }

        public long Gen2CollectCount { get; set; }

        public long HeapMemory { get; set; }
    }

    public class ThreadStatsRequest
    {
        public int AvailableCompletionPortThreads { get; set; }

        public int AvailableWorkerThreads { get; set; }

        public int MaxCompletionPortThreads { get; set; }

        public int MaxWorkerThreads { get; set; }
    }
}
