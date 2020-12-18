using SkyApm.Abstractions.Tracing;
using SkyApm.Abstractions.Tracing.Segments;
using SkyApm.Abstractions.Transport;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Core.Transport
{

    public class SegmentContextMapper : ISegmentContextMapper
    {
        public SegmentRequest Map(SegmentContext segmentContext)
        {
            var segmentRequest = new SegmentRequest
            {
                UniqueIds = new[]
                {
                    MapUniqueId(segmentContext.TraceId)
                }
            };
            var segmentObjectRequest = new SegmentObjectRequest
            {
                SegmentId = MapUniqueId(segmentContext.SegmentId),
                ServiceId = segmentContext.ServiceId,
                ServiceInstanceId = segmentContext.ServiceInstanceId
            };
            segmentRequest.Segment = segmentObjectRequest;
            var span = new SpanRequest
            {
                SpanId = segmentContext.Span.SpanId,
                ParentSpanId = segmentContext.Span.ParentSpanId,
                OperationName = segmentContext.Span.OperationName,
                StartTime = segmentContext.Span.StartTime,
                EndTime = segmentContext.Span.EndTime,
                SpanType = (int)segmentContext.Span.SpanType,
                SpanLayer = (int)segmentContext.Span.SpanLayer,
                IsError = segmentContext.Span.IsError,
                Peer = segmentContext.Span.Peer,
                Component = segmentContext.Span.Component
            };
            foreach (var reference in segmentContext.References)
            {
                //span.References.Add(new SegmentReferenceRequest
                //{
                //    ParentSegmentId = MapUniqueId(reference.ParentSegmentId),
                //    ParentServiceInstanceId = reference.ParentServiceInstanceId,
                //    ParentSpanId = reference.ParentSpanId,
                //    ParentEndpointName = reference.ParentEndpoint,
                //    EntryServiceInstanceId = reference.EntryServiceInstanceId,
                //    EntryEndpointName = reference.EntryEndpoint,
                //    NetworkAddress = reference.NetworkAddress,
                //    RefType = (int)reference.Reference
                //});
            }
                

            foreach (var tag in segmentContext.Span.Tags)
                span.Tags.Add(new KeyValuePair<string, string>(tag.Key, tag.Value));

            foreach (var log in segmentContext.Span.Logs)
            {
                var logData = new LogDataRequest { Timestamp = log.Timestamp };
                foreach (var data in log.Data)
                    logData.Data.Add(new KeyValuePair<string, string>(data.Key, data.Value));
                span.Logs.Add(logData);
            }

            segmentObjectRequest.Spans.Add(span);
            return segmentRequest;
        }

        private static UniqueIdRequest MapUniqueId(UniqueId uniqueId)
        {
            return new UniqueIdRequest
            {
                Part1 = uniqueId.Part1,
                Part2 = uniqueId.Part2,
                Part3 = uniqueId.Part3
            };
        }
    }
}
