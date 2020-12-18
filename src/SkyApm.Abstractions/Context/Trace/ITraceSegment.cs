using SkyApm.Abstractions.Context.Ids;
using SkyApm.Abstractions.Transport;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Abstractions.Context.Trace
{

    public interface ITraceSegment
    {
        void Archive(AbstractTracingSpan finishedSpan);

        ITraceSegment Finish(bool isSizeLimited);

        int ApplicationId { get; }

        int ApplicationInstanceId { get; }

        List<ITraceSegmentRef> Refs { get; }

        List<DistributedTraceId> RelatedGlobalTraces { get; }

        ID TraceSegmentId { get; }

        bool HasRef { get; }

        bool IsIgnore { get; set; }

        bool IsSingleSpanSegment { get; }

        void Ref(ITraceSegmentRef refSegment);

        void RelatedGlobalTrace(DistributedTraceId distributedTraceId);

        TraceSegmentRequest Transform();
    }
}
