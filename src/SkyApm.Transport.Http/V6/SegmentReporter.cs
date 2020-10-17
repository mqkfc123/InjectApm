using SkyApm.Abstractions.Config;
using SkyApm.Abstractions.Transport;
using SkyApm.Infrastructure.Configuration;
using SkyApm.Logging;
using SkyApm.Transport.Http.Common;
using SkyApm.Transport.Http.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SkyApm.Transport.Grpc.V6
{

    public class SegmentReporter : ISegmentReporter
    {
        private readonly ILogger _logger;
        private readonly GrpcConfig _config;
        private const string segments = "/v2/segments";

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
                foreach (var segment in segmentRequests)
                {
                    var param = SegmentV6Helpers.Map(segment);
                   
                    //http 请求
                    var result = HttpHelper.PostMode(_config.Servers + segments, Newtonsoft.Json.JsonConvert.SerializeObject(param));
                    if (string.IsNullOrEmpty(result))
                    {
                    }
                    else
                    {
                        List<KeyStringValuePair> values = Newtonsoft.Json.JsonConvert.DeserializeObject<List<KeyStringValuePair>>(result);
                    }
                }
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
