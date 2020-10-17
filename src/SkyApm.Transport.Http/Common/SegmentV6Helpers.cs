﻿using SkyApm.Abstractions.Common;
using SkyApm.Abstractions.Transport;
using SkyApm.Transport.Http.Entity;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SkyApm.Transport.Http.Common
{

    internal static class SegmentV6Helpers
    {
        public static UpstreamSegment Map(SegmentRequest request)
        {
            var upstreamSegment = new UpstreamSegment()
            {
                globalTraceIds = new System.Collections.Generic.List<UniqueId>()
            };

            upstreamSegment.globalTraceIds.AddRange(request.UniqueIds.Select(MapToUniqueId).ToArray());

            var traceSegment = new SegmentObject
            {
                traceSegmentId = MapToUniqueId(request.Segment.SegmentId),
                serviceId = request.Segment.ServiceId,
                serviceInstanceId = request.Segment.ServiceInstanceId,
                isSizeLimited = false,
                spans = new System.Collections.Generic.List<SpanObjectV2>()
            };
            //Add
            traceSegment.spans.AddRange(request.Segment.Spans.Select(MapToSpan).ToArray());

            using (MemoryStream ms = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, traceSegment);

                upstreamSegment.segment = ms.GetBuffer();// traceSegment.ToByteString();
            }
            //upstreamSegment.segment = Newtonsoft.Json.JsonConvert.SerializeObject(traceSegment);
            //upstreamSegment.segment = traceSegment;
            return upstreamSegment;
        }

        private static UniqueId MapToUniqueId(UniqueIdRequest uniqueIdRequest)
        {
            var uniqueId = new UniqueId()
            {
                idParts = new System.Collections.Generic.List<long>()
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
                logs = new System.Collections.Generic.List<Log>(),
                tags = new System.Collections.Generic.List<KeyStringValuePair>(),
                refs = new System.Collections.Generic.List<SegmentReference>()
            };

            ReadStringOrIntValue(spanObject, request.Component, ComponentReader, ComponentIdReader);
            ReadStringOrIntValue(spanObject, request.OperationName, OperationNameReader, OperationNameIdReader);
            ReadStringOrIntValue(spanObject, request.Peer, PeerReader, PeerIdReader);

            //Add
            spanObject.tags.AddRange(request.Tags.Select(x => new KeyStringValuePair { key = x.Key, value = x.Value }));
            spanObject.refs.AddRange(request.References.Select(MapToSegmentReference).ToArray());
            spanObject.logs.AddRange(request.Logs.Select(MapToLogMessage).ToArray());

            return spanObject;
        }

        private static SegmentReference MapToSegmentReference(SegmentReferenceRequest referenceRequest)
        {
            var reference = new SegmentReference
            {
                parentServiceInstanceId = referenceRequest.ParentServiceInstanceId,
                entryServiceInstanceId = referenceRequest.EntryServiceInstanceId,
                parentSpanId = referenceRequest.ParentSpanId,
                refType = (RefType)referenceRequest.RefType,
                parentTraceSegmentId = MapToUniqueId(referenceRequest.ParentSegmentId)
            };

            ReadStringOrIntValue(reference, referenceRequest.NetworkAddress, NetworkAddressReader,
                NetworkAddressIdReader);
            ReadStringOrIntValue(reference, referenceRequest.EntryEndpointName, EntryEndpointReader,
                EntryEndpointIdReader);
            ReadStringOrIntValue(reference, referenceRequest.ParentEndpointName, ParentEndpointReader,
                ParentEndpointIdReader);

            return reference;
        }

        private static Log MapToLogMessage(LogDataRequest request)
        {
            var logMessage = new Log
            {
                data = new System.Collections.Generic.List<KeyStringValuePair>(),
                time = request.Timestamp
            };

            logMessage.data.AddRange(request.Data.Select(x => new KeyStringValuePair { key = x.Key, value = x.Value })
                .ToArray());
            return logMessage;
        }

        private static void ReadStringOrIntValue<T>(T instance, StringOrIntValue stringOrIntValue,
            Action<T, string> stringValueReader, Action<T, int> intValueReader)
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

        private static readonly Action<SegmentReference, string> NetworkAddressReader =
            (s, val) => s.networkAddress = val;

        private static readonly Action<SegmentReference, int> NetworkAddressIdReader =
            (s, val) => s.networkAddressId = val;

        private static readonly Action<SegmentReference, string>
            EntryEndpointReader = (s, val) => s.entryEndpoint = val;

        private static readonly Action<SegmentReference, int> EntryEndpointIdReader =
            (s, val) => s.entryEndpointId = val;

        private static readonly Action<SegmentReference, string> ParentEndpointReader =
            (s, val) => s.parentEndpoint = val;

        private static readonly Action<SegmentReference, int> ParentEndpointIdReader =
            (s, val) => s.parentEndpointId = val;
    }
}
