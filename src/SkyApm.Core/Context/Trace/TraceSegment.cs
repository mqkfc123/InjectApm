using SkyApm.Abstractions.Context.Ids;
using SkyApm.Abstractions.Context.Trace;
using SkyApm.Abstractions.Transport;
using SkyApm.Core.Context.Ids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyApm.Core.Context.Trace
{

    public class TraceSegment : ITraceSegment
    {
        private readonly IList<ITraceSegmentRef> _refs;
        private readonly IList<AbstractTracingSpan> _spans;
        private readonly DistributedTraceIdCollection _relatedGlobalTraces;
        private bool _isSizeLimited;

        public int ApplicationId => WorkContext.RuntimeEnvironment.ServiceId.Value;

        public int ApplicationInstanceId => WorkContext.RuntimeEnvironment.ServiceInstanceId.Value;

        public List<ITraceSegmentRef> Refs => _refs.ToList();

        public List<DistributedTraceId> RelatedGlobalTraces => _relatedGlobalTraces.GetRelatedGlobalTraces().ToList();

        public ID TraceSegmentId { get; }

        public bool HasRef => _refs.Count > 0;

        public bool IsIgnore { get; set; }

        public bool IsSingleSpanSegment => _spans.Count == 1;

        public TraceSegment()
        {
            TraceSegmentId = GlobalIdGenerator.Generate();
            _spans = new List<AbstractTracingSpan>();
            _relatedGlobalTraces = new DistributedTraceIdCollection();
            _relatedGlobalTraces.Append(new NewDistributedTraceId());
            _refs = new List<ITraceSegmentRef>();
        }

        public void Archive(AbstractTracingSpan finishedSpan)
        {
            _spans.Add(finishedSpan);
        }

        public ITraceSegment Finish(bool isSizeLimited)
        {
            _isSizeLimited = isSizeLimited;
            return this;
        }

        /// <summary>
        /// Establish the link between this segment and its parents.
        /// </summary>
        public void Ref(ITraceSegmentRef refSegment)
        {
            if (!_refs.Contains(refSegment))
            {
                _refs.Add(refSegment);
            }
        }

        public void RelatedGlobalTrace(DistributedTraceId distributedTraceId)
        {
            _relatedGlobalTraces.Append(distributedTraceId);
        }

        public TraceSegmentRequest Transform()
        {
            var upstreamSegment = new TraceSegmentRequest
            {
                UniqueIds = _relatedGlobalTraces.GetRelatedGlobalTraces()
                    .Select(x => x.ToUniqueId()).ToArray()
            };

            upstreamSegment.Segment = new TraceSegmentObjectRequest
            {
                SegmentId = TraceSegmentId.Transform(),
                Spans = _spans.Select(x => x.Transform()).ToArray(),
                ApplicationId = ApplicationId,
                ApplicationInstanceId = ApplicationInstanceId
            };

            return upstreamSegment;
        }

        public override string ToString()
        {
            return "TraceSegment{"
                   +
                   $"traceSegmentId='{TraceSegmentId}', refs={_refs}, spans={_spans}, relatedGlobalTraces={_relatedGlobalTraces}"
                   + "}";
        }
    }
}
