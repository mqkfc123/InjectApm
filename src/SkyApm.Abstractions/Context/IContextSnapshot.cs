using SkyApm.Abstractions.Context.Ids;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Abstractions.Context
{

    public interface IContextSnapshot
    {
        string EntryOperationName { get; set; }

        string ParentOperationName { get; set; }

        DistributedTraceId DistributedTraceId { get; }

        int EntryApplicationInstanceId { get; set; }

        int SpanId { get; }

        bool IsFromCurrent { get; }

        bool IsValid { get; }

        ID TraceSegmentId { get; }

        int EntryOperationId { set; }

        int ParentOperationId { set; }
    }
}
