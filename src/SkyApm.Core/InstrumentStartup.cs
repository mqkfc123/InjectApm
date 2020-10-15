using SkyApm.Abstractions;
using SkyApm.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SkyApm.Core
{

    public class InstrumentStartup : IInstrumentStartup
    {
        //private readonly TracingDiagnosticProcessorObserver _observer;
        private readonly IEnumerable<IExecutionService> _services;
        private readonly ILogger _logger;

        public InstrumentStartup(IEnumerable<IExecutionService> services, ILoggerFactory loggerFactory)
        {
            //_observer = observer;
            _services = services;
            _logger = loggerFactory.CreateLogger(typeof(InstrumentStartup));
        }

        public async Task StartAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            _logger.Information("Initializing ...");
            foreach (var service in _services)
                await service.StartAsync(cancellationToken);
            
            //DiagnosticListener.AllListeners.Subscribe(_observer);
            _logger.Information("Started SkyAPM .NET Core Agent.");
        }

        public async Task StopAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (var service in _services)
                await service.StopAsync(cancellationToken);
            _logger.Information("Stopped SkyAPM .NET Core Agent.");
            // ReSharper disable once MethodSupportsCancellation
            await Task.Delay(TimeSpan.FromSeconds(2));
        }
    }
}
