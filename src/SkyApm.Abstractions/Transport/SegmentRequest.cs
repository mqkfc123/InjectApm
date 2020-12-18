using SkyApm.Abstractions.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Abstractions.Transport
{
    public class SegmentRequest
    {
        public IEnumerable<UniqueIdRequest> UniqueIds { get; set; }

        public SegmentObjectRequest Segment { get; set; }
    }

    /// <summary>
    /// V3 
    /// </summary>
    public class TraceSegmentRequest
    {
        public IEnumerable<UniqueIdRequest> UniqueIds { get; set; }

        public TraceSegmentObjectRequest Segment { get; set; }
    }

    public class UniqueIdRequest
    {
        public long Part1 { get; set; }

        public long Part2 { get; set; }

        public long Part3 { get; set; }

        public override string ToString()
        {
            return $"{Part1}.{Part2}.{Part3}";
        }
    }

    public class SegmentObjectRequest
    {
        public UniqueIdRequest SegmentId { get; set; }

        public int ServiceId { get; set; }

        public int ServiceInstanceId { get; set; }

        public IList<SpanRequest> Spans { get; set; } = new List<SpanRequest>();
    }

    /// <summary>
    /// V3 
    /// </summary>
    public class TraceSegmentObjectRequest
    {
        public UniqueIdRequest SegmentId { get; set; }

        public int ApplicationId { get; set; }

        public int ApplicationInstanceId { get; set; }

        public IList<SpanRequest> Spans { get; set; } = new List<SpanRequest>();
    }

    public class SpanRequest
    {
        public int SpanId { get; set; }

        public int SpanType { get; set; }

        public int SpanLayer { get; set; }

        public int ParentSpanId { get; set; }

        public long StartTime { get; set; }

        public long EndTime { get; set; }

        public StringOrIntValue Component { get; set; }

        public StringOrIntValue OperationName { get; set; }

        public StringOrIntValue Peer { get; set; }

        public bool IsError { get; set; }

        public IList<TraceSegmentReferenceRequest> References { get; } = new List<TraceSegmentReferenceRequest>();

        public IList<KeyValuePair<string, string>> Tags { get; } = new List<KeyValuePair<string, string>>();

        public IList<LogDataRequest> Logs { get; } = new List<LogDataRequest>();
    }

    public class SegmentReferenceRequest
    {
        public UniqueIdRequest ParentSegmentId { get; set; }

        public int ParentServiceInstanceId { get; set; }

        public int ParentSpanId { get; set; }

        public int EntryServiceInstanceId { get; set; }

        public int RefType { get; set; }

        public StringOrIntValue ParentEndpointName { get; set; }

        public StringOrIntValue EntryEndpointName { get; set; }

        public StringOrIntValue NetworkAddress { get; set; }
    }


    /// <summary>
    /// V3 segment
    /// </summary>
    public class TraceSegmentReferenceRequest
    {
        public UniqueIdRequest ParentTraceSegmentId { get; set; }

        public int ParentApplicationInstanceId { get; set; }

        public int ParentSpanId { get; set; }

        public int EntryApplicationInstanceId { get; set; }

        public int RefType { get; set; }

        public StringOrIntValue ParentServiceName { get; set; }

        public StringOrIntValue EntryServiceName { get; set; }

        public StringOrIntValue NetworkAddress { get; set; }
    }


    public class LogDataRequest
    {
        public long Timestamp { get; set; }

        public IList<KeyValuePair<string, string>> Data { get; } = new List<KeyValuePair<string, string>>();
    }
}
