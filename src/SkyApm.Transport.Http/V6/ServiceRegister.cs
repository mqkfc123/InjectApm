using SkyApm.Abstractions.Common;
using SkyApm.Abstractions.Config;
using SkyApm.Abstractions.Transport;
using SkyApm.Infrastructure.Configuration;
using SkyApm.Logging;
using SkyApm.Transport.Http.Common;
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

        private const string register = "/v2/service/register";
        private const string instanceRegister = "/v2/instance/register";

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
                serviceName = serviceRequest.ServiceName,
                type = ServiceType.Normal.ToString()
            };
            services.Add(service);

            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("services", services);
            
            //http 请求
            var result= HttpHelper.PostMode(_config.Servers + register,Newtonsoft.Json.JsonConvert.SerializeObject(param));
            if (string.IsNullOrEmpty(result))
            {
                return NullableValue.Null;
            }
            else
            {
                List<KeyStringValuePair> values = Newtonsoft.Json.JsonConvert.DeserializeObject<List<KeyStringValuePair>>(result);
                return new NullableValue(Convert.ToInt32(values[0].value));
            }
        }


        public NullableValue RegisterServiceInstanceAsync(ServiceInstanceRequest serviceInstanceRequest)
        {

            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var t3 = Convert.ToInt64((DateTime.Now - epoch).TotalMilliseconds);


            var instances = new List<ServiceInstance>();
            
            var instance = new ServiceInstance
            {
                serviceId = serviceInstanceRequest.ServiceId,
                instanceUUID = serviceInstanceRequest.InstanceUUID,
                time = t3
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
            instance.properties = properties;
            instances.Add(instance);


            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("instances", instances);

            //http 请求
            var result = HttpHelper.PostMode(_config.Servers + instanceRegister, Newtonsoft.Json.JsonConvert.SerializeObject(param));
            if (string.IsNullOrEmpty(result))
            {
                return NullableValue.Null;
            }
            else
            {
                List<KeyStringValuePair> values = Newtonsoft.Json.JsonConvert.DeserializeObject<List<KeyStringValuePair>>(result);
                return new NullableValue(Convert.ToInt32(values[0].value));
            }
             
        }

    }
}
