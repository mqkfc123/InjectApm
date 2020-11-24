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

    public class PingCaller : IPingCaller
    {
        private readonly ILogger _logger;
        private readonly GrpcConfig _config;
        private const string heartbeat = "/v2/instance/heartbeat";
        
        public PingCaller(ILoggerFactory loggerFactory,
            IConfigAccessor configAccessor)
        {
            _config = configAccessor.Get<GrpcConfig>();
            _logger = loggerFactory.CreateLogger(NLog.LogManager.GetCurrentClassLogger());
        }

        public void PingAsync(PingRequest request)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var t3 = Convert.ToInt64((DateTime.Now - epoch).TotalMilliseconds);

            var serviceInstancePingPkg = new ServiceInstancePingPkg
            {
                serviceInstanceId = request.ServiceInstanceId,
                serviceInstanceUUID = request.InstanceId,
                time = t3  //DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };

            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("InstancePingPkg", serviceInstancePingPkg);

            //http 请求
            var result = HttpHelper.PostMode(_config.Servers + heartbeat, Newtonsoft.Json.JsonConvert.SerializeObject(serviceInstancePingPkg));
            if (!string.IsNullOrEmpty(result))
            {
                _logger.Information($"PingAsync : {result}");
            }
            
        }

    }
}
