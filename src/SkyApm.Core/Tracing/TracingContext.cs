using SkyApm.Abstractions.Common;
using SkyApm.Abstractions.Tracing;
using SkyApm.Abstractions.Tracing.Segments;
using SkyApm.Abstractions.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyApm.Core.Tracing
{

    public class TracingContext : ITracingContext
    {
        private readonly ISegmentContextFactory _segmentContextFactory;
        private readonly ICarrierPropagator _carrierPropagator;
        private readonly ISegmentDispatcher _segmentDispatcher;

        public TracingContext(ISegmentContextFactory segmentContextFactory, ICarrierPropagator carrierPropagator,
            ISegmentDispatcher segmentDispatcher)
        {
            _segmentContextFactory = segmentContextFactory;
            _carrierPropagator = carrierPropagator;
            _segmentDispatcher = segmentDispatcher;
        }

        public SegmentContext CreateEntrySegmentContext(string operationName, ICarrierHeaderCollection carrierHeader)
        {
            if (operationName == null)
                throw new ArgumentNullException(nameof(operationName));

            var carrier = _carrierPropagator.Extract(carrierHeader);
            return _segmentContextFactory.CreateEntrySegment(operationName, carrier);
        }

        public SegmentContext CreateLocalSegmentContext(string operationName)
        {
            if (operationName == null) throw new ArgumentNullException(nameof(operationName));
            return _segmentContextFactory.CreateLocalSegment(operationName);
        }

        public SegmentContext CreateExitSegmentContext(string operationName, string networkAddress,
            ICarrierHeaderCollection carrierHeader = default(ICarrierHeaderCollection))
        {
            var segmentContext =
                _segmentContextFactory.CreateExitSegment(operationName, new StringOrIntValue(networkAddress));
            if (carrierHeader != null)
                _carrierPropagator.Inject(segmentContext, carrierHeader);
            return segmentContext;
        }

        public void Release(SegmentContext segmentContext)
        {
            if (segmentContext == null)
            {
                return;
            }

            _segmentContextFactory.Release(segmentContext);
            if (segmentContext.Sampled)
                _segmentDispatcher.Dispatch(segmentContext);
        }
    }
}
