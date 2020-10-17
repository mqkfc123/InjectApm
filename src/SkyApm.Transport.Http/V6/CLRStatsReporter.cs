using SkyApm.Abstractions;
using SkyApm.Abstractions.Config;
using SkyApm.Abstractions.Transport;
using SkyApm.Infrastructure.Configuration;
using SkyApm.Logging;
using SkyApm.Transport.Http.Entity;
using System;

namespace SkyApm.Transport.Grpc.V6
{

    public class CLRStatsReporter : ICLRStatsReporter
    {
        private readonly ILogger _logger;
        private readonly GrpcConfig _config;
        private readonly IRuntimeEnvironment _runtimeEnvironment;

        public CLRStatsReporter(ILoggerFactory loggerFactory,
            IConfigAccessor configAccessor, IRuntimeEnvironment runtimeEnvironment)
        {
            _logger = loggerFactory.CreateLogger(typeof(CLRStatsReporter));
            _config = configAccessor.Get<GrpcConfig>();
            _runtimeEnvironment = runtimeEnvironment;
        }

        public  void ReportAsync(CLRStatsRequest statsRequest)
        {

            try
            {
                var request = new CLRMetricCollection
                {
                    serviceInstanceId = _runtimeEnvironment.ServiceInstanceId.Value
                };
                var metric = new CLRMetric
                {
                    cpu = new CPU
                    {
                        usagePercent = statsRequest.CPU.UsagePercent
                    },
                    gc = new ClrGC
                    {
                        gen0CollectCount = statsRequest.GC.Gen0CollectCount,
                        gen1CollectCount = statsRequest.GC.Gen1CollectCount,
                        gen2CollectCount = statsRequest.GC.Gen2CollectCount,
                        heapMemory = statsRequest.GC.HeapMemory
                    },
                    thread = new ClrThread
                    {
                        availableWorkerThreads = statsRequest.Thread.MaxWorkerThreads,
                        availableCompletionPortThreads = statsRequest.Thread.MaxCompletionPortThreads,
                        maxWorkerThreads = statsRequest.Thread.MaxWorkerThreads,
                        maxCompletionPortThreads = statsRequest.Thread.MaxCompletionPortThreads
                    },
                    time = DateTimeOffset.UtcNow.UtcTicks //DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                };
                request.metrics.Add(metric);

            }
            catch (Exception e)
            {
                _logger.Warning("Report CLR Stats error. " + e);
            }
        }
    }
}
