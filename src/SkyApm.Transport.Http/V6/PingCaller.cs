using SkyApm.Abstractions.Config;
using SkyApm.Abstractions.Transport;
using SkyApm.Infrastructure.Configuration;
using SkyApm.Logging;
using SkyApm.Transport.Http.Entity;
using System;

namespace SkyApm.Transport.Grpc.V6
{

    public class PingCaller : IPingCaller
    {
        private readonly ILogger _logger;
        private readonly GrpcConfig _config;

        public PingCaller(ILoggerFactory loggerFactory,
            IConfigAccessor configAccessor)
        {
            _config = configAccessor.Get<GrpcConfig>();
            _logger = loggerFactory.CreateLogger(typeof(PingCaller));
        }

        public void PingAsync(PingRequest request)
        {
            var serviceInstancePingPkg = new ServiceInstancePingPkg
            {
                serviceInstanceId = request.ServiceInstanceId,
                serviceInstanceUUID = request.InstanceId,
                time = DateTimeOffset.UtcNow.UtcTicks  //DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };

        }

    }
}
