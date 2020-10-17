
namespace SkyApm.Transport.Http.Entity
{
    public class CLRMetric
    {
        public long time { get; set; }
        public CPU cpu { get; set; }
        public ClrGC gc { get; set; }
        public ClrThread thread { get; set; }

    }

    public class CPU
    {
        public double usagePercent { get; set; }
    }

    public class ClrGC
    {
        public long gen0CollectCount { get; set; }
        public long gen1CollectCount { get; set; }
        public long gen2CollectCount { get; set; }
        public long heapMemory { get; set; }

    }

    public class ClrThread
    {
        public int availableCompletionPortThreads { get; set; }
        public int availableWorkerThreads { get; set; }
        public int maxCompletionPortThreads { get; set; }
        public int maxWorkerThreads { get; set; }
    }

}
