using SkyApm.Abstractions.Transport;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Abstractions.Context.Trace
{

    public interface ITraceSegmentRef : IEquatable<ITraceSegmentRef>
    {
        string EntryOperationName { get; }

        int EntryOperationId { get; }

        int EntryApplicationInstanceId { get; }

        TraceSegmentReferenceRequest Transform();
    }
}
