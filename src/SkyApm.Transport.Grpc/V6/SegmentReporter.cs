using SkyApm.Abstractions.Config;
using SkyApm.Abstractions.Transport;
using SkyApm.Infrastructure.Configuration;
using SkyApm.Logging;
using SkyApm.Transport.Grpc.Common;
using SkyWalking.NetworkProtocol;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SkyApm.Transport.Grpc.V6
{

    public class SegmentReporter : ISegmentReporter
    {
        private readonly ConnectionManager _connectionManager;
        private readonly ILogger _logger;
        private readonly GrpcConfig _config;

        public SegmentReporter(ConnectionManager connectionManager, IConfigAccessor configAccessor,
            ILoggerFactory loggerFactory)
        {
            _connectionManager = connectionManager;
            _config = configAccessor.Get<GrpcConfig>();
            _logger = loggerFactory.CreateLogger(typeof(SegmentReporter));
        }

        public async Task ReportAsync(IReadOnlyCollection<SegmentRequest> segmentRequests,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!_connectionManager.Ready)
            {
                return;
            }

            var connection = _connectionManager.GetConnection();

            try
            {
                var stopwatch = Stopwatch.StartNew();
                var client = new TraceSegmentReportService.TraceSegmentReportServiceClient(connection);
                using (var asyncClientStreamingCall = client.collect(_config.GetMeta(), _config.GetReportTimeout(), cancellationToken))
                {
                    foreach (var segment in segmentRequests)
                        await asyncClientStreamingCall.RequestStream.WriteAsync(SegmentV6Helpers.Map(segment));
                    await asyncClientStreamingCall.RequestStream.CompleteAsync();
                    await asyncClientStreamingCall.ResponseAsync;
                }

                stopwatch.Stop();
                _logger.Information($"Report {segmentRequests.Count} trace segment. cost: {stopwatch.Elapsed}s");
            }
            catch (Exception ex)
            {
                _logger.Error("Report trace segment fail.", ex);
                _connectionManager.Failure(ex);
            }
        }
    }
}
