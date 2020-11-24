using SkyApm.Abstractions;
using SkyApm.Abstractions.Config;
using SkyApm.Abstractions.Transport;
using SkyApm.Infrastructure.Configuration;
using SkyApm.Logging;
using SkyWalking.NetworkProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SkyApm.Transport.Grpc.V6
{

    public class CLRStatsReporter : ICLRStatsReporter
    {
        private readonly ConnectionManager _connectionManager;
        private readonly ILogger _logger;
        private readonly GrpcConfig _config;
        private readonly IRuntimeEnvironment _runtimeEnvironment;

        public CLRStatsReporter(ConnectionManager connectionManager, ILoggerFactory loggerFactory,
            IConfigAccessor configAccessor, IRuntimeEnvironment runtimeEnvironment)
        {
            _connectionManager = connectionManager;
            _logger = loggerFactory.CreateLogger(NLog.LogManager.GetCurrentClassLogger());
            _config = configAccessor.Get<GrpcConfig>();
            _runtimeEnvironment = runtimeEnvironment;
        }

        public async Task ReportAsync(CLRStatsRequest statsRequest,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!_connectionManager.Ready)
            {
                return;
            }

            var connection = _connectionManager.GetConnection();

            try
            {
                var request = new CLRMetricCollection
                {
                    ServiceInstanceId = _runtimeEnvironment.ServiceInstanceId.Value
                };
                var metric = new CLRMetric
                {
                    Cpu = new CPU
                    {
                        UsagePercent = statsRequest.CPU.UsagePercent
                    },
                    Gc = new ClrGC
                    {
                        Gen0CollectCount = statsRequest.GC.Gen0CollectCount,
                        Gen1CollectCount = statsRequest.GC.Gen1CollectCount,
                        Gen2CollectCount = statsRequest.GC.Gen2CollectCount,
                        HeapMemory = statsRequest.GC.HeapMemory
                    },
                    Thread = new ClrThread
                    {
                        AvailableWorkerThreads = statsRequest.Thread.MaxWorkerThreads,
                        AvailableCompletionPortThreads = statsRequest.Thread.MaxCompletionPortThreads,
                        MaxWorkerThreads = statsRequest.Thread.MaxWorkerThreads,
                        MaxCompletionPortThreads = statsRequest.Thread.MaxCompletionPortThreads
                    },
                    Time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                };
                request.Metrics.Add(metric);
                var client = new CLRMetricReportService.CLRMetricReportServiceClient(connection);
                await client.collectAsync(request, _config.GetMeta(), _config.GetTimeout(), cancellationToken);
            }
            catch (Exception e)
            {
                _logger.Warning("Report CLR Stats error. " + e);
            }
        }
    }
}
