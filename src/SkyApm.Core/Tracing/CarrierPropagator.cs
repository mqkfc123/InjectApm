using SkyApm.Abstractions.Tracing;
using SkyApm.Abstractions.Tracing.Segments;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Core.Tracing
{

    public class CarrierPropagator : ICarrierPropagator
    {
        private readonly IEnumerable<ICarrierFormatter> _carrierFormatters;
        private readonly ISegmentContextFactory _segmentContextFactory;

        public CarrierPropagator(IEnumerable<ICarrierFormatter> carrierFormatters,
            ISegmentContextFactory segmentContextFactory)
        {
            _carrierFormatters = carrierFormatters;
            _segmentContextFactory = segmentContextFactory;
        }

        public void Inject(SegmentContext segmentContext, ICarrierHeaderCollection headerCollection)
        {
            var reference = segmentContext.References.GetEnumerator().Current;

            var carrier = new Carrier(segmentContext.TraceId, segmentContext.SegmentId, segmentContext.Span.SpanId,
                segmentContext.ServiceInstanceId, reference?.EntryServiceInstanceId ?? segmentContext.ServiceInstanceId)
            {
                NetworkAddress = segmentContext.Span.Peer,
                EntryEndpoint = reference?.EntryEndpoint ?? segmentContext.Span.OperationName,
                ParentEndpoint = segmentContext.Span.OperationName,
                Sampled = segmentContext.Sampled
            };

            foreach (var formatter in _carrierFormatters)
            {
                if (formatter.Enable)
                    headerCollection.Add(formatter.Key, formatter.Encode(carrier));
            }
        }

        public ICarrier Extract(ICarrierHeaderCollection headerCollection)
        {
            ICarrier carrier = NullableCarrier.Instance;
            if (headerCollection == null)
            {
                return carrier;
            }
            foreach (var formatter in _carrierFormatters)
            {
                if (!formatter.Enable)
                {
                    continue;
                }

                foreach (var header in headerCollection)
                {
                    if (formatter.Key == header.Key)
                    {
                        carrier = formatter.Decode(header.Value);
                        if (carrier.HasValue)
                        {
                            if (formatter.Key.EndsWith("sw3") && carrier is Carrier )
                            {
                                ((Carrier)carrier).Sampled = true;
                            }

                            return carrier;
                        }
                    }
                }
            }

            return carrier;
        }
    }
}
