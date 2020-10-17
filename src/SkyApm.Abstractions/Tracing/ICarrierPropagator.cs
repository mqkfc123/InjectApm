using SkyApm.Abstractions.Tracing.Segments;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Abstractions.Tracing
{
    public interface ICarrierPropagator
    {
        void Inject(SegmentContext segmentContext, ICarrierHeaderCollection carrier);

        ICarrier Extract(ICarrierHeaderCollection carrier);
    }
}
