using SkyApm.Abstractions;
using SkyApm.Abstractions.Config;
using SkyApm.Abstractions.Tracing;
using SkyApm.Abstractions.Transport;
using SkyApm.Core.Service;
using SkyApm.Core.Tracing;
using SkyApm.Core.Transport;
using SkyApm.Infrastructure.Configuration;
using SkyApm.Infrastructure.Logging;
using SkyApm.Logging;
using SkyApm.Transport.Grpc.V6;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SkyApm.Core
{

    public class InstrumentStartup : IInstrumentStartup
    {
        private readonly List<IExecutionService> _services;
        private readonly ILogger _logger;

        public InstrumentStartup()
        {
            IRuntimeEnvironment runtimeEnvironment = new RuntimeEnvironment();
            IConfigAccessor configAccessor = new ConfigAccessor();
            ILoggerFactory loggerFactory = new NLoggerFactory();
            ISegmentContextMapper segmentContextMapper = new SegmentContextMapper();
            ISegmentReporter segmentReporter = new SegmentReporter(configAccessor, loggerFactory);

            ISegmentDispatcher segmentDispatcher = new AsyncQueueSegmentDispatcher(configAccessor, segmentReporter, runtimeEnvironment, segmentContextMapper, loggerFactory);
            IServiceRegister serviceRegister = new ServiceRegister(configAccessor, loggerFactory);
            IPingCaller pingCaller = new PingCaller(loggerFactory, configAccessor);

            _services.Add(new RegisterService(configAccessor, serviceRegister, runtimeEnvironment, loggerFactory));
            _services.Add(new PingService(configAccessor, pingCaller, runtimeEnvironment, loggerFactory));
            _services.Add(new SegmentReportService(configAccessor, segmentDispatcher, runtimeEnvironment, loggerFactory));

            IUniqueIdGenerator uniqueIdGenerator = new UniqueIdGenerator(runtimeEnvironment);
            ISegmentContextFactory segmentContextFactory = new SegmentContextFactory(runtimeEnvironment, uniqueIdGenerator);

            ICarrierPropagator carrierPropagator = new CarrierPropagator(null, segmentContextFactory);

            ITracingContext tracingContext = new TracingContext(segmentContextFactory, carrierPropagator, segmentDispatcher);

            WorkContext.TracingContext = tracingContext;

            _logger = loggerFactory.CreateLogger(typeof(InstrumentStartup));
        }

        public void StartAsync()
        {
            _logger.Information("Initializing ...");
            foreach (var service in _services)
            {
                service.StartAsync();
            }
            _logger.Information("Started SkyAPM .NET Core Agent.");
        }

        public void StopAsync()
        {
            foreach (var service in _services)
            {
                service.StopAsync();
            }
            _logger.Information("Stopped SkyAPM .NET Core Agent.");
            Thread.Sleep(TimeSpan.FromSeconds(2));
        }

    }
}
