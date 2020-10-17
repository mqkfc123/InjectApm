using SkyApm.Abstractions;
using SkyApm.Abstractions.Config;
using SkyApm.Abstractions.Transport;
using SkyApm.Logging;
using System;

namespace SkyApm.Core.Service
{

    public class PingService : ExecutionService
    {
        private readonly IPingCaller _pingCaller;
        private readonly TransportConfig _transportConfig;

        public PingService(IConfigAccessor configAccessor, IPingCaller pingCaller,
            IRuntimeEnvironment runtimeEnvironment,
            ILoggerFactory loggerFactory) : base(
            runtimeEnvironment, loggerFactory)
        {
            _pingCaller = pingCaller;
            _transportConfig = configAccessor.Get<TransportConfig>();
        }

        protected override bool CanExecute() =>
            _transportConfig.ProtocolVersion == ProtocolVersions.V6 && base.CanExecute();

        protected override TimeSpan DueTime { get; } = TimeSpan.FromSeconds(30);
        protected override TimeSpan Period { get; } = TimeSpan.FromSeconds(60);

        protected override  void ExecuteAsync()
        {
            try
            {
                 _pingCaller.PingAsync(
                    new PingRequest
                    {
                        ServiceInstanceId = RuntimeEnvironment.ServiceInstanceId.Value,
                        InstanceId = RuntimeEnvironment.InstanceId.ToString("N")
                    });
                Logger.Information($"Ping server @{DateTimeOffset.UtcNow}");
            }
            catch (Exception exception)
            {
                Logger.Error($"Ping server fail @{DateTimeOffset.UtcNow}", exception);
            }
        }
    }
}
