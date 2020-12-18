using SkyApm.Abstractions.Common;
using SkyApm.Abstractions.Transport;
using SkyApm.Transport.Http.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyApm.Transport.Http.Common
{
    internal static class TraceSegmentHelpers
    {
        public static UpstreamSegment Map(TraceSegmentRequest request)
        {
            var upstreamSegment = new UpstreamSegment()
            {
                globalTraceIds = new List<UniqueId>()
            };

            upstreamSegment.globalTraceIds.AddRange(request.UniqueIds.Select(MapToUniqueId).ToArray());
            upstreamSegment.globalTraceIds.AddRange(request.UniqueIds.Select(MapToUniqueId).ToArray());
            upstreamSegment.traceSegmentId = MapToUniqueId(request.Segment.SegmentId);
            upstreamSegment.spans = new List<SpanObjectV2>();
            upstreamSegment.serviceId = request.Segment.ApplicationId;
            upstreamSegment.serviceInstanceId = request.Segment.ApplicationInstanceId;
            upstreamSegment.isSizeLimited = false;

            upstreamSegment.spans.AddRange(request.Segment.Spans.Select(MapToSpan).ToArray());

            return upstreamSegment;
        }

        private static UniqueId MapToUniqueId(UniqueIdRequest uniqueIdRequest)
        {
            var uniqueId = new UniqueId()
            {
                idParts = new List<long>()
            };
            uniqueId.idParts.Add(uniqueIdRequest.Part1);
            uniqueId.idParts.Add(uniqueIdRequest.Part2);
            uniqueId.idParts.Add(uniqueIdRequest.Part3);
            return uniqueId;
        }

        private static SpanObjectV2 MapToSpan(SpanRequest request)
        {
            var spanObject = new SpanObjectV2
            {
                spanId = request.SpanId,
                parentSpanId = request.ParentSpanId,
                startTime = request.StartTime,
                endTime = request.EndTime,
                spanType = (SpanType)request.SpanType,
                spanLayer = (SpanLayer)request.SpanLayer,
                isError = request.IsError,
                tags = new List<KeyStringValuePair>(),
                refs = new List<SegmentReference>(),
                logs = new List<Log>()
            };

            ReadStringOrIntValue(spanObject, request.Component, ComponentReader, ComponentIdReader);
            ReadStringOrIntValue(spanObject, request.OperationName, OperationNameReader, OperationNameIdReader);
            ReadStringOrIntValue(spanObject, request.Peer, PeerReader, PeerIdReader);

            spanObject.tags.AddRange(request.Tags.Select(x => new KeyStringValuePair { key = x.Key, value = x.Value }));
            spanObject.refs.AddRange(request.References.Select(MapToSegmentReference).ToArray());
            spanObject.logs.AddRange(request.Logs.Select(MapToLogMessage).ToArray());

            return spanObject;
        }

        private static SegmentReference MapToSegmentReference(TraceSegmentReferenceRequest referenceRequest)
        {
            var reference = new SegmentReference
            {
                parentServiceInstanceId = referenceRequest.ParentApplicationInstanceId,
                entryServiceInstanceId = referenceRequest.EntryApplicationInstanceId,
                parentSpanId = referenceRequest.ParentSpanId,
                refType = (RefType)referenceRequest.RefType,
                parentTraceSegmentId = MapToUniqueId(referenceRequest.ParentTraceSegmentId)
            };

            ReadStringOrIntValue(reference, referenceRequest.NetworkAddress, NetworkAddressReader, NetworkAddressIdReader);
            ReadStringOrIntValue(reference, referenceRequest.EntryServiceName, EntryServiceReader, EntryServiceIdReader);
            ReadStringOrIntValue(reference, referenceRequest.ParentServiceName, ParentServiceReader, ParentServiceIdReader);

            return reference;
        }

        private static Log MapToLogMessage(LogDataRequest request)
        {
            var logMessage = new Log
            {
                time = request.Timestamp,
                data = new List<KeyStringValuePair>()
            };

            logMessage.data.AddRange(request.Data.Select(x => new KeyStringValuePair { key = x.Key, value = x.Value }).ToArray());
            return logMessage;
        }

        private static void ReadStringOrIntValue<T>(T instance, StringOrIntValue stringOrIntValue, Action<T, string> stringValueReader, Action<T, int> intValueReader)
        {
            if (stringOrIntValue.HasStringValue)
            {
                stringValueReader.Invoke(instance, stringOrIntValue.GetStringValue());
            }
            else if (stringOrIntValue.HasIntValue)
            {
                intValueReader.Invoke(instance, stringOrIntValue.GetIntValue());
            }
        }

        private static readonly Action<SpanObjectV2, string> ComponentReader = (s, val) => s.component = val;
        private static readonly Action<SpanObjectV2, int> ComponentIdReader = (s, val) => s.componentId = val;
        private static readonly Action<SpanObjectV2, string> OperationNameReader = (s, val) => s.operationName = val;
        private static readonly Action<SpanObjectV2, int> OperationNameIdReader = (s, val) => s.operationNameId = val;
        private static readonly Action<SpanObjectV2, string> PeerReader = (s, val) => s.peer = val;
        private static readonly Action<SpanObjectV2, int> PeerIdReader = (s, val) => s.peerId = val;
        private static readonly Action<SegmentReference, string> NetworkAddressReader = (s, val) => s.networkAddress = val;
        private static readonly Action<SegmentReference, int> NetworkAddressIdReader = (s, val) => s.networkAddressId = val;
        private static readonly Action<SegmentReference, string> EntryServiceReader = (s, val) => s.entryEndpoint = val;
        private static readonly Action<SegmentReference, int> EntryServiceIdReader = (s, val) => s.entryServiceInstanceId = val;
        private static readonly Action<SegmentReference, string> ParentServiceReader = (s, val) => s.parentEndpoint = val;
        private static readonly Action<SegmentReference, int> ParentServiceIdReader = (s, val) => s.parentServiceInstanceId = val;
    }
}
