using SkyApm.Abstractions.Common;
using SkyApm.Abstractions.Config;
using SkyApm.Abstractions.Transport;
using SkyApm.Infrastructure.Configuration;
using SkyApm.Logging;
using SkyApm.Transport.Http.Entity;
using System;
using System.Collections.Generic;

namespace SkyApm.Transport.Grpc.V6
{

    public class ServiceRegister : IServiceRegister
    {
        private const string OS_NAME = "os_name";
        private const string HOST_NAME = "host_name";
        private const string IPV4 = "ipv4";
        private const string PROCESS_NO = "process_no";
        private const string LANGUAGE = "language";

        private readonly ILogger _logger;
        private readonly GrpcConfig _config;

        public ServiceRegister(IConfigAccessor configAccessor,
            ILoggerFactory loggerFactory)
        {
            _config = configAccessor.Get<GrpcConfig>();
            _logger = loggerFactory.CreateLogger(typeof(ServiceRegister));
        }

        public NullableValue RegisterServiceAsync(ServiceRequest serviceRequest)
        {
            List<Service> services = new List<Service>();

            var service = new Service
            {
                serviceName = serviceRequest.ServiceName
            };
            services.Add(service);

            //http 请求

            return NullableValue.Null;
        }


        public NullableValue RegisterServiceInstanceAsync(ServiceInstanceRequest serviceInstanceRequest)
        {

            var instances = new List<ServiceInstance>();
            
            var instance = new ServiceInstance
            {
                serviceId = serviceInstanceRequest.ServiceId,
                instanceUUID = serviceInstanceRequest.InstanceUUID,
                time = DateTimeOffset.UtcNow.UtcTicks
            };

            List<KeyStringValuePair> properties = new List<KeyStringValuePair>();

            properties.Add(new KeyStringValuePair
            { key = OS_NAME, value = serviceInstanceRequest.Properties.OsName });
            properties.Add(new KeyStringValuePair
            { key = HOST_NAME, value = serviceInstanceRequest.Properties.HostName });
            properties.Add(new KeyStringValuePair
            { key = PROCESS_NO, value = serviceInstanceRequest.Properties.ProcessNo.ToString() });
            properties.Add(new KeyStringValuePair
            { key = LANGUAGE, value = serviceInstanceRequest.Properties.Language });

            foreach (var ip in serviceInstanceRequest.Properties.IpAddress)
            {
                properties.Add(new KeyStringValuePair { key = IPV4, value = ip });
            }

            instances.Add(instance);


            return NullableValue.Null;

        }

    }
}
