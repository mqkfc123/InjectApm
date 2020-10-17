using SkyApm.Abstractions.Common;
using SkyApm.Abstractions.Tracing;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Core.Tracing
{

    public class NullableCarrier : ICarrier
    {
        public static NullableCarrier Instance { get; } = new NullableCarrier();

        public bool HasValue { get; } = false;

        public bool? Sampled { get; }

        public UniqueId TraceId { get; }

        public UniqueId ParentSegmentId { get; }

        public int ParentSpanId { get; }

        public int ParentServiceInstanceId { get; }

        public int EntryServiceInstanceId { get; }

        public StringOrIntValue NetworkAddress { get; }

        public StringOrIntValue EntryEndpoint { get; }

        public StringOrIntValue ParentEndpoint { get; }
    }
}
