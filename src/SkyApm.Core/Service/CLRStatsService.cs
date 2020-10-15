using SkyApm.Abstractions;
using SkyApm.Abstractions.Transport;
using SkyApm.Core.Common;
using SkyApm.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SkyApm.Core.Service
{

    public class CLRStatsService : ExecutionService
    {
        private readonly ICLRStatsReporter _reporter;

        public CLRStatsService(ICLRStatsReporter reporter, IRuntimeEnvironment runtimeEnvironment,
            ILoggerFactory loggerFactory)
            : base(runtimeEnvironment, loggerFactory)
        {
            _reporter = reporter;
        }

        protected override TimeSpan DueTime { get; } = TimeSpan.FromSeconds(30);
        protected override TimeSpan Period { get; } = TimeSpan.FromSeconds(30);

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var cpuStats = new CPUStatsRequest
            {
                UsagePercent = CpuHelpers.UsagePercent
            };
            var gcStats = new GCStatsRequest
            {
                Gen0CollectCount = GCHelpers.Gen0CollectCount,
                Gen1CollectCount = GCHelpers.Gen1CollectCount,
                Gen2CollectCount = GCHelpers.Gen2CollectCount,
                HeapMemory = GCHelpers.TotalMemory
            };
            var availableWorkerThreads = 0;
            var availableCompletionPortThreads = 0;
            var maxWorkerThreads = 0;
            var maxCompletionPortThreads = 0;

            ThreadPool.GetAvailableThreads(out availableWorkerThreads, out availableCompletionPortThreads);
            ThreadPool.GetMaxThreads(out maxWorkerThreads, out maxCompletionPortThreads);
            var threadStats = new ThreadStatsRequest
            {
                MaxCompletionPortThreads = maxCompletionPortThreads,
                MaxWorkerThreads = maxWorkerThreads,
                AvailableCompletionPortThreads = availableCompletionPortThreads,
                AvailableWorkerThreads = availableWorkerThreads
            };
            var statsRequest = new CLRStatsRequest
            {
                CPU = cpuStats,
                GC = gcStats,
                Thread = threadStats
            };
            try
            {
                await _reporter.ReportAsync(statsRequest, cancellationToken);
                Logger.Information(
                    $"Report CLR Stats. CPU UsagePercent {cpuStats.UsagePercent} GenCollectCount {gcStats.Gen0CollectCount} {gcStats.Gen1CollectCount} {gcStats.Gen2CollectCount} {gcStats.HeapMemory / (1024 * 1024)}M ThreadPool {threadStats.AvailableWorkerThreads} {threadStats.MaxWorkerThreads} {threadStats.AvailableCompletionPortThreads} {threadStats.MaxCompletionPortThreads}");
            }
            catch (Exception exception)
            {
                Logger.Error("Report CLR Stats error.", exception);
            }
        }
    }
}
