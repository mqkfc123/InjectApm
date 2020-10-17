using SkyApm.Abstractions.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Abstractions.Tracing
{

    public interface ICarrier
    {
        bool HasValue { get; }

        bool? Sampled { get; }

        UniqueId TraceId { get; }

        UniqueId ParentSegmentId { get; }

        int ParentSpanId { get; }

        int ParentServiceInstanceId { get; }

        int EntryServiceInstanceId { get; }

        StringOrIntValue NetworkAddress { get; }

        StringOrIntValue EntryEndpoint { get; }

        StringOrIntValue ParentEndpoint { get; }
    }
}
