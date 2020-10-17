using SkyApm.Abstractions;
using SkyApm.Abstractions.Common;
using SkyApm.Abstractions.Tracing;
using SkyApm.Abstractions.Tracing.Segments;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Core.Tracing
{
    public class SegmentContextFactory : ISegmentContextFactory
    {
        private readonly IRuntimeEnvironment _runtimeEnvironment;
        private readonly IUniqueIdGenerator _uniqueIdGenerator;

        public SegmentContextFactory(IRuntimeEnvironment runtimeEnvironment, IUniqueIdGenerator uniqueIdGenerator)
        {
            _runtimeEnvironment = runtimeEnvironment;
            _uniqueIdGenerator = uniqueIdGenerator;
        }

        public SegmentContext CreateEntrySegment(string operationName, ICarrier carrier)
        {
            var traceId = GetTraceId(carrier);
            var segmentId = GetSegmentId();
            var sampled = true;
            var segmentContext = new SegmentContext(traceId, segmentId, sampled,
                _runtimeEnvironment.ServiceId.Value,
                _runtimeEnvironment.ServiceInstanceId.Value,
                operationName, SpanType.Entry);

            if (carrier.HasValue)
            {
                var segmentReference = new SegmentReference
                {
                    Reference = Reference.CrossProcess,
                    EntryEndpoint = carrier.EntryEndpoint,
                    NetworkAddress = carrier.NetworkAddress,
                    ParentEndpoint = carrier.ParentEndpoint,
                    ParentSpanId = carrier.ParentSpanId,
                    ParentSegmentId = carrier.ParentSegmentId,
                    EntryServiceInstanceId = carrier.EntryServiceInstanceId,
                    ParentServiceInstanceId = carrier.ParentServiceInstanceId
                };
                segmentContext.References.Add(segmentReference);
            }
             
            return segmentContext;
        }

        public SegmentContext CreateLocalSegment(string operationName)
        {
            var parentSegmentContext = GetParentSegmentContext(SpanType.Local);
            var traceId = GetTraceId(parentSegmentContext);
            var segmentId = GetSegmentId();
            var sampled = true;
            var segmentContext = new SegmentContext(traceId, segmentId, sampled, _runtimeEnvironment.ServiceId.Value,
                _runtimeEnvironment.ServiceInstanceId.Value, operationName, SpanType.Local);

            if (parentSegmentContext != null)
            {
                var parentReference = parentSegmentContext.References.GetEnumerator().Current;
                var reference = new SegmentReference
                {
                    Reference = Reference.CrossThread,
                    EntryEndpoint = parentReference?.EntryEndpoint ?? parentSegmentContext.Span.OperationName,
                    NetworkAddress = parentReference?.NetworkAddress ?? parentSegmentContext.Span.OperationName,
                    ParentEndpoint = parentSegmentContext.Span.OperationName,
                    ParentSpanId = parentSegmentContext.Span.SpanId,
                    ParentSegmentId = parentSegmentContext.SegmentId,
                    EntryServiceInstanceId =
                        parentReference?.EntryServiceInstanceId ?? parentSegmentContext.ServiceInstanceId,
                    ParentServiceInstanceId = parentSegmentContext.ServiceInstanceId
                };
                segmentContext.References.Add(reference);
            }
             
            return segmentContext;
        }

        public SegmentContext CreateExitSegment(string operationName, StringOrIntValue networkAddress)
        {
            var parentSegmentContext = GetParentSegmentContext(SpanType.Exit);
            var traceId = GetTraceId(parentSegmentContext);
            var segmentId = GetSegmentId();
            var sampled = true;
            var segmentContext = new SegmentContext(traceId, segmentId, sampled, _runtimeEnvironment.ServiceId.Value,
                _runtimeEnvironment.ServiceInstanceId.Value, operationName, SpanType.Exit);

            if (parentSegmentContext != null)
            {
                 
               var parentReference = parentSegmentContext.References.GetEnumerator().Current;
                var reference = new SegmentReference
                {
                    Reference = Reference.CrossThread,
                    EntryEndpoint = parentReference?.EntryEndpoint ?? parentSegmentContext.Span.OperationName,
                    NetworkAddress = parentReference?.NetworkAddress ?? parentSegmentContext.Span.OperationName,
                    ParentEndpoint = parentSegmentContext.Span.OperationName,
                    ParentSpanId = parentSegmentContext.Span.SpanId,
                    ParentSegmentId = parentSegmentContext.SegmentId,
                    EntryServiceInstanceId =
                        parentReference?.EntryServiceInstanceId ?? parentSegmentContext.ServiceInstanceId,
                    ParentServiceInstanceId = parentSegmentContext.ServiceInstanceId
                };
                segmentContext.References.Add(reference);
            }

            segmentContext.Span.Peer = networkAddress;
            return segmentContext;
        }

        public void Release(SegmentContext segmentContext)
        {
            segmentContext.Span.Finish();
            switch (segmentContext.Span.SpanType)
            {
                case SpanType.Entry:
                    break;
                case SpanType.Local:
                    break;
                case SpanType.Exit:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(SpanType), segmentContext.Span.SpanType, "Invalid SpanType.");
            }
        }

        private UniqueId GetTraceId(ICarrier carrier)
        {
            return carrier.HasValue ? carrier.TraceId : _uniqueIdGenerator.Generate();
        }

        private UniqueId GetTraceId(SegmentContext parentSegmentContext)
        {
            return parentSegmentContext?.TraceId ?? _uniqueIdGenerator.Generate();
        }

        private UniqueId GetSegmentId()
        {
            return _uniqueIdGenerator.Generate();
        }

     

     

        private SegmentContext GetParentSegmentContext(SpanType spanType)
        {
            switch (spanType)
            {
                case SpanType.Entry:
                    return null;
                case SpanType.Local:
                    return null;
                case SpanType.Exit:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException(nameof(spanType), spanType, "Invalid SpanType.");
            }
        }
    }
}
