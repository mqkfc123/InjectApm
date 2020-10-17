using SkyApm.Abstractions;
using SkyApm.Abstractions.Config;
using SkyApm.Abstractions.Transport;
using SkyApm.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

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

        protected override void ExecuteAsync()
        {
             _dispatcher.Flush();
        }

        protected override void Stopping()
        {
            _dispatcher.Close();
        }
    }
}
