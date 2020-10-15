using SkyApm.Abstractions.Common;
using SkyApm.Abstractions.Tracing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyApm.Core.Tracing
{

    public class Carrier : ICarrier
    {
        public bool HasValue { get; } = true;

        public bool? Sampled { get; set; }

        public UniqueId TraceId { get; }

        public UniqueId ParentSegmentId { get; }

        public int ParentSpanId { get; }

        public int ParentServiceInstanceId { get; }

        public int EntryServiceInstanceId { get; }

        public StringOrIntValue NetworkAddress { get; set; }

        public StringOrIntValue EntryEndpoint { get; set; }

        public StringOrIntValue ParentEndpoint { get; set; }

        public Carrier(UniqueId traceId, UniqueId parentSegmentId, int parentSpanId, int parentServiceInstanceId,
            int entryServiceInstanceId)
        {
            TraceId = traceId;
            ParentSegmentId = parentSegmentId;
            ParentSpanId = parentSpanId;
            ParentServiceInstanceId = parentServiceInstanceId;
            EntryServiceInstanceId = entryServiceInstanceId;
        }
    }
}
