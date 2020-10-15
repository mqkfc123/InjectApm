﻿using SkyApm.Abstractions;
using SkyApm.Abstractions.Common;
using SkyApm.Abstractions.Config;
using SkyApm.Abstractions.Transport;
using SkyApm.Core.Common;
using SkyApm.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SkyApm.Core.Service
{

    public class RegisterService : ExecutionService
    {
        private readonly InstrumentConfig _config;
        private readonly IServiceRegister _serviceRegister;
        private readonly TransportConfig _transportConfig;

        public RegisterService(IConfigAccessor configAccessor, IServiceRegister serviceRegister,
            IRuntimeEnvironment runtimeEnvironment, ILoggerFactory loggerFactory) : base(runtimeEnvironment,
            loggerFactory)
        {
            _serviceRegister = serviceRegister;
            _config = configAccessor.Get<InstrumentConfig>();
            _transportConfig = configAccessor.Get<TransportConfig>();
        }

        protected override TimeSpan DueTime { get; } = TimeSpan.Zero;

        protected override TimeSpan Period { get; } = TimeSpan.FromSeconds(30);

        protected override bool CanExecute() =>
            _transportConfig.ProtocolVersion == ProtocolVersions.V6 && !RuntimeEnvironment.Initialized;

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await RegisterServiceAsync(cancellationToken);
            await RegisterServiceInstanceAsync(cancellationToken);
        }

        private async Task RegisterServiceAsync(CancellationToken cancellationToken)
        {
            if (!RuntimeEnvironment.ServiceId.HasValue)
            {
                var request = new ServiceRequest
                {
                    ServiceName = _config.ServiceName
                };

                var value = await Polling(3,
                    () => _serviceRegister.RegisterServiceAsync(request, cancellationToken),
                    cancellationToken);

                if (value.HasValue && RuntimeEnvironment is RuntimeEnvironment)
                {
                    var environment = (RuntimeEnvironment)RuntimeEnvironment;

                    environment.ServiceId = value;
                    Logger.Information($"Registered Service[Id={environment.ServiceId.Value}].");
                }

            }
        }

        private async Task RegisterServiceInstanceAsync(CancellationToken cancellationToken)
        {
            if (RuntimeEnvironment.ServiceId.HasValue && !RuntimeEnvironment.ServiceInstanceId.HasValue)
            {
                var properties = new AgentOsInfoRequest
                {
                    HostName = DnsHelpers.GetHostName(),
                    IpAddress = DnsHelpers.GetIpV4s(),
                    OsName = PlatformInformation.GetOSName(),
                    ProcessNo = Process.GetCurrentProcess().Id,
                    Language = "dotnet"
                };
                var request = new ServiceInstanceRequest
                {
                    ServiceId = RuntimeEnvironment.ServiceId.Value,
                    InstanceUUID = RuntimeEnvironment.InstanceId.ToString("N"),
                    Properties = properties
                };
                var value = await Polling(3,
                    () => _serviceRegister.RegisterServiceInstanceAsync(request, cancellationToken),
                    cancellationToken);

                if (value.HasValue && RuntimeEnvironment is RuntimeEnvironment)
                {
                    var environment = (RuntimeEnvironment)RuntimeEnvironment;

                    environment.ServiceInstanceId = value;
                    Logger.Information($"Registered ServiceInstance[Id={environment.ServiceInstanceId.Value}].");
                }
            }
        }

        private static async Task<NullableValue> Polling(int retry, Func<Task<NullableValue>> execute,
            CancellationToken cancellationToken)
        {
            var index = 0;
            while (index++ < retry)
            {
                var value = await execute();
                if (value.HasValue)
                {
                    return value;
                }

                await Task.Delay(500, cancellationToken);
            }

            return NullableValue.Null;
        }
    }
}
