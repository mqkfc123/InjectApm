using SkyApm.Abstractions;
using SkyApm.Abstractions.Config;
using SkyApm.Abstractions.Transport;
using SkyApm.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SkyApm.Core.Service
{
    /// <summary>
    /// trace
    /// </summary>
    public class SegmentReportService : ExecutionService
    {
        private readonly TransportConfig _config;
        private readonly ISegmentDispatcher _dispatcher;

        public SegmentReportService(IConfigAccessor configAccessor, ISegmentDispatcher dispatcher,
            IRuntimeEnvironment runtimeEnvironment, ILoggerFactory loggerFactory)
            : base(runtimeEnvironment, loggerFactory)
        {
            _dispatcher = dispatcher;
            _config = configAccessor.Get<TransportConfig>();
            Period = TimeSpan.FromMilliseconds(_config.Interval);
        }

        protected override TimeSpan DueTime { get; } = TimeSpan.FromSeconds(3);

        protected override TimeSpan Period { get; }

        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            return _dispatcher.Flush(cancellationToken);
        }

        protected override Task Stopping(CancellationToken cancellationToke)
        {
            _dispatcher.Close();
            return Task.CompletedTask;
        }
    }
}
