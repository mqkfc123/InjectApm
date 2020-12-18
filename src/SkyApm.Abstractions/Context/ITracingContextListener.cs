using SkyApm.Abstractions.Context.Trace;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Abstractions.Context
{
    public interface ITracingContextListener
    {
        void AfterFinished(ITraceSegment traceSegment);
    }
}
