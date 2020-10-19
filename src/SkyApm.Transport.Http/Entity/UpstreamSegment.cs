using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace SkyApm.Transport.Http.Entity
{
    [Serializable]
    public class UpstreamSegment
    {
        public List<UniqueId> globalTraceIds { get; set; }

        //public string segment { get; set; } 
        public UniqueId traceSegmentId { get; set; }
        public List<SpanObjectV2> spans { get; set; }
        public int serviceId { get; set; }
        public int serviceInstanceId { get; set; }
        public bool isSizeLimited { get; set; }


    }
    [Serializable]
    public class SegmentObject
    {
        public UniqueId traceSegmentId { get; set; }
        public List<SpanObjectV2> spans { get; set; }
        public int serviceId { get; set; }
        public int serviceInstanceId { get; set; }
        public bool isSizeLimited { get; set; }

    }

    [Serializable]
    public class SpanObjectV2
    {
        public int spanId { get; set; }
        public int parentSpanId { get; set; }
        public long startTime { get; set; }
        public long endTime { get; set; }
        public List<SegmentReference> refs { get; set; }
        public int operationNameId { get; set; }
        public string operationName { get; set; }
        public int peerId { get; set; }
        public string peer { get; set; }
        public SpanType spanType { get; set; }
        public SpanLayer spanLayer { get; set; }
        public int componentId { get; set; }
        public string component { get; set; }
        public bool isError { get; set; }
        public List<KeyStringValuePair> tags { get; set; }
        public List<Log> logs { get; set; }
    }

    [Serializable]
    public class Log
    {
        public long time { get; set; }
        public List<KeyStringValuePair> data { get; set; }
    }


    [Serializable]
    public class SegmentReference
    {
        public RefType refType { get; set; }
        public UniqueId parentTraceSegmentId { get; set; }
        public int parentSpanId { get; set; }
        public int parentServiceInstanceId { get; set; }
        public string networkAddress { get; set; }
        public int networkAddressId { get; set; }
        public int entryServiceInstanceId { get; set; }
        public string entryEndpoint { get; set; }
        public int entryEndpointId { get; set; }
        public string parentEndpoint { get; set; }
        public int parentEndpointId { get; set; }

    }

}
