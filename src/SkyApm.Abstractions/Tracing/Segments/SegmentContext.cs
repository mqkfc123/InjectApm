using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyApm.Abstractions.Tracing.Segments
{

    public class SegmentContext
    {
        public UniqueId SegmentId { get; }

        public UniqueId TraceId { get; }

        public SegmentSpan Span { get; }

        public int ServiceId { get; }

        public int ServiceInstanceId { get; }

        public bool Sampled { get; }

        public bool IsSizeLimited { get; } = false;

        public SegmentReferenceCollection References { get; } = new SegmentReferenceCollection();

        public SegmentContext(UniqueId traceId, UniqueId segmentId, bool sampled, int serviceId, int serviceInstanceId,
            string operationName, SpanType spanType)
        {
            TraceId = traceId;
            Sampled = sampled;
            SegmentId = segmentId;
            ServiceId = serviceId;
            ServiceInstanceId = serviceInstanceId;
            Span = new SegmentSpan(operationName, spanType);
        }
    }
}
