using SkyApm.Abstractions.Tracing.Segments;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Abstractions.Tracing
{
    public interface ITracingContext
    {
        SegmentContext CreateEntrySegmentContext(string operationName, ICarrierHeaderCollection carrierHeader);

        SegmentContext CreateLocalSegmentContext(string operationName);

        SegmentContext CreateExitSegmentContext(string operationName, string networkAddress, ICarrierHeaderCollection carrierHeader = default(ICarrierHeaderCollection));

        void Release(SegmentContext segmentContext);
    }
}
