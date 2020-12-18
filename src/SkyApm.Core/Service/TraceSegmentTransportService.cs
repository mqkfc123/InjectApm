using SkyApm.Abstractions;
using SkyApm.Abstractions.Config;
using SkyApm.Abstractions.Context;
using SkyApm.Abstractions.Context.Trace;
using SkyApm.Abstractions.Transport;
using SkyApm.Core.Context;
using SkyApm.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Core.Service
{

    public class TraceSegmentTransportService : ExecutionService, ITracingContextListener
    {
        private readonly TransportConfig _config;
        private readonly ITraceDispatcher _dispatcher;

        public TraceSegmentTransportService(IConfigAccessor configAccessor, ITraceDispatcher dispatcher,
            IRuntimeEnvironment runtimeEnvironment, ILoggerFactory loggerFactory)
            : base( runtimeEnvironment, loggerFactory)
        {
            _dispatcher = dispatcher;
            _config = configAccessor.Get<TransportConfig>();
            Period = TimeSpan.FromMilliseconds(_config.Interval);
            TracingContext.ListenerManager.Add(this);
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
            TracingContext.ListenerManager.Remove(this); 
        }

        public void AfterFinished(ITraceSegment traceSegment)
        {
            if (!traceSegment.IsIgnore)
                _dispatcher.Dispatch(traceSegment.Transform());
        }
    }
}
