using SkyApm.Abstractions;
using SkyApm.Abstractions.Config;
using SkyApm.Infrastructure.Configuration;
using SkyApm.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Transport.Http.V6
{
    public class TraceHttpClient : ISkyWalkingClient
    {
        private readonly ILogger _logger;
        private readonly GrpcConfig _config;

        public TraceHttpClient( IConfigAccessor configAccessor, ILoggerFactory loggerFactory)
        {
            _config = configAccessor.Get<GrpcConfig>();
            _logger = loggerFactory.CreateLogger(NLog.LogManager.GetCurrentClassLogger());

        }

        public async Task CollectAsync(IEnumerable<TraceSegmentRequest> request, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!_connectionManager.Ready)
            {
                return;
            }

            var connection = _connectionManager.GetConnection();

            var client = new TraceSegmentService.TraceSegmentServiceClient(connection);
            try
            {
                using (var asyncClientStreamingCall = client.collect(null, null, cancellationToken))
                {
                    foreach (var segment in request)
                        await asyncClientStreamingCall.RequestStream.WriteAsync(TraceSegmentHelpers.Map(segment));
                    await asyncClientStreamingCall.RequestStream.CompleteAsync();
                    await asyncClientStreamingCall.ResponseAsync;
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Heartbeat error.", ex);
                _connectionManager.Failure(ex);
            }
        }

        private async Task ExecuteWithCatch(Func<Task> task, Func<string> errMessage)
        {
            try
            {
                await task();
            }
            catch (Exception ex)
            {
                _logger.Error(errMessage(), ex);
                _connectionManager.Failure(ex);
            }
        }

        private async Task<T> ExecuteWithCatch<T>(Func<Task<T>> task, Func<T> errCallback, Func<string> errMessage)
        {
            try
            {
                return await task();
            }
            catch (Exception ex)
            {
                _logger.Error(errMessage(), ex);
                _connectionManager.Failure(ex);
                return errCallback();
            }
        }
    }
}