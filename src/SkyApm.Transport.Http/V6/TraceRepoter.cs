using SkyApm.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using SkyApm.Abstractions.Transport;
using SkyApm.Abstractions.Config;
using SkyApm.Infrastructure.Configuration;
using SkyApm.Logging;
using System.Diagnostics;
using SkyApm.Transport.Http.Common;

namespace SkyApm.Transport.Http.V6
{
    public class TraceRepoter : ISkyWalkingClient
    {
        private readonly ILogger _logger;
        private readonly GrpcConfig _config;
        private const string segments = "/v2/segments";

        public TraceRepoter(IConfigAccessor configAccessor, ILoggerFactory loggerFactory)
        {
            _config = configAccessor.Get<GrpcConfig>();
            _logger = loggerFactory.CreateLogger(NLog.LogManager.GetCurrentClassLogger());
        }

        public void CollectAsync(IEnumerable<TraceSegmentRequest> request)
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();
                foreach (var segment in request)
                {
                    var param = TraceSegmentHelpers.Map(segment);
                    //http 请求
                    var result = HttpHelper.PostMode(_config.Servers + segments, Newtonsoft.Json.JsonConvert.SerializeObject(param));
                    if (!string.IsNullOrEmpty(result))
                    {
                        _logger.Information($"Report {result}");
                    }
                }
                stopwatch.Stop();
                _logger.Information($"Report trace segment. cost: {stopwatch.Elapsed}s");
            }
            catch (Exception ex)
            {
                _logger.Error("Report trace segment fail.", ex);
            }

        }

    }
}
