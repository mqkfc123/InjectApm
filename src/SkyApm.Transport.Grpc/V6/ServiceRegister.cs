using SkyApm.Abstractions.Common;
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

    public class ServiceRegister : IServiceRegister
    {
        private const string OS_NAME = "os_name";
        private const string HOST_NAME = "host_name";
        private const string IPV4 = "ipv4";
        private const string PROCESS_NO = "process_no";
        private const string LANGUAGE = "language";

        private readonly ConnectionManager _connectionManager;
        private readonly ILogger _logger;
        private readonly GrpcConfig _config;

        public ServiceRegister(ConnectionManager connectionManager, IConfigAccessor configAccessor,
            ILoggerFactory loggerFactory)
        {
            _connectionManager = connectionManager;
            _config = configAccessor.Get<GrpcConfig>();
            _logger = loggerFactory.CreateLogger(NLog.LogManager.GetCurrentClassLogger());
        }

        public async Task<NullableValue> RegisterServiceAsync(ServiceRequest serviceRequest,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!_connectionManager.Ready)
            {
                return NullableValue.Null;
            }

            var connection = _connectionManager.GetConnection();
            return await new Call(_logger, _connectionManager).Execute(async () =>
            {
                var client = new Register.RegisterClient(connection);
                var services = new Services();
                services.Services_.Add(new Service
                {
                    ServiceName = serviceRequest.ServiceName
                });
                var mapping = await client.doServiceRegisterAsync(services,
                    _config.GetMeta(), _config.GetTimeout(), cancellationToken);
                foreach (var service in mapping.Services)
                    if (service.Key == serviceRequest.ServiceName)
                        return new NullableValue(service.Value);
                return NullableValue.Null;
            },
                () => NullableValue.Null,
                () => ExceptionHelpers.RegisterServiceError);
        }

        public async Task<NullableValue> RegisterServiceInstanceAsync(ServiceInstanceRequest serviceInstanceRequest,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!_connectionManager.Ready)
            {
                return NullableValue.Null;
            }

            var connection = _connectionManager.GetConnection();
            return await new Call(_logger, _connectionManager).Execute(async () =>
            {
                var client = new Register.RegisterClient(connection);
                var instance = new ServiceInstance
                {
                    ServiceId = serviceInstanceRequest.ServiceId,
                    InstanceUUID = serviceInstanceRequest.InstanceUUID,
                    Time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                };

                instance.Properties.Add(new KeyStringValuePair
                { Key = OS_NAME, Value = serviceInstanceRequest.Properties.OsName });
                instance.Properties.Add(new KeyStringValuePair
                { Key = HOST_NAME, Value = serviceInstanceRequest.Properties.HostName });
                instance.Properties.Add(new KeyStringValuePair
                { Key = PROCESS_NO, Value = serviceInstanceRequest.Properties.ProcessNo.ToString() });
                instance.Properties.Add(new KeyStringValuePair
                { Key = LANGUAGE, Value = serviceInstanceRequest.Properties.Language });
                foreach (var ip in serviceInstanceRequest.Properties.IpAddress)
                    instance.Properties.Add(new KeyStringValuePair
                    { Key = IPV4, Value = ip });

                var serviceInstances = new ServiceInstances();
                serviceInstances.Instances.Add(instance);
                var mapping = await client.doServiceInstanceRegisterAsync(serviceInstances,
                    _config.GetMeta(), _config.GetTimeout(), cancellationToken);
                foreach (var serviceInstance in mapping.ServiceInstances)
                    if (serviceInstance.Key == serviceInstanceRequest.InstanceUUID)
                        return new NullableValue(serviceInstance.Value);
                return NullableValue.Null;
            },
                () => NullableValue.Null,
                () => ExceptionHelpers.RegisterServiceInstanceError);
        }
    }
}
