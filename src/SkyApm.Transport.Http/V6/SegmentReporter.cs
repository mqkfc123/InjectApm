using SkyApm.Abstractions.Config;
using SkyApm.Abstractions.Transport;
using SkyApm.Infrastructure.Configuration;
using SkyApm.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SkyApm.Transport.Grpc.V6
{

    public class SegmentReporter : ISegmentReporter
    {
        private readonly ILogger _logger;
        private readonly GrpcConfig _config;

        public SegmentReporter(IConfigAccessor configAccessor,
            ILoggerFactory loggerFactory)
        {
            _config = configAccessor.Get<GrpcConfig>();
            _logger = loggerFactory.CreateLogger(typeof(SegmentReporter));
        }

        public void ReportAsync(ICollection<SegmentRequest> segmentRequests)
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();
               
                    //foreach (var segment in segmentRequests)
                    //    await asyncClientStreamingCall.RequestStream.WriteAsync(SegmentV6Helpers.Map(segment));
                    //await asyncClientStreamingCall.RequestStream.CompleteAsync();
                    //await asyncClientStreamingCall.ResponseAsync;
                 
                stopwatch.Stop();
                _logger.Information($"Report {segmentRequests.Count} trace segment. cost: {stopwatch.Elapsed}s");
            }
            catch (Exception ex)
            {
                _logger.Error("Report trace segment fail.", ex);
            }
        }
    }
}
