using SkyApm.Abstractions.Config;
using SkyApm.Abstractions.Transport;
using SkyApm.Infrastructure.Configuration;
using SkyApm.Logging;
using SkyApm.Transport.Grpc.Common;
using SkyWalking.NetworkProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SkyApm.Transport.Grpc.V6
{

    public class PingCaller : IPingCaller
    {
        private readonly ConnectionManager _connectionManager;
        private readonly ILogger _logger;
        private readonly GrpcConfig _config;

        public PingCaller(ConnectionManager connectionManager, ILoggerFactory loggerFactory,
            IConfigAccessor configAccessor)
        {
            _connectionManager = connectionManager;
            _config = configAccessor.Get<GrpcConfig>();
            _logger = loggerFactory.CreateLogger(NLog.LogManager.GetCurrentClassLogger());
        }

        public Task PingAsync(PingRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!_connectionManager.Ready)
            {
                return Task.CompletedTask;
            }

            var connection = _connectionManager.GetConnection();
            return new Call(_logger, _connectionManager).Execute(async () =>
            {
                var client = new ServiceInstancePing.ServiceInstancePingClient(connection);
                await client.doPingAsync(new ServiceInstancePingPkg
                {
                    ServiceInstanceId = request.ServiceInstanceId,
                    ServiceInstanceUUID = request.InstanceId,
                    Time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                }, _config.GetMeta(), _config.GetTimeout(), cancellationToken);
            },
                () => ExceptionHelpers.PingError);
        }
    }
}
