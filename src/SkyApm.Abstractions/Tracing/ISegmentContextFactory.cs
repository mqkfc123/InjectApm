using SkyApm.Abstractions.Common;
using SkyApm.Abstractions.Tracing.Segments;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Abstractions.Tracing
{
    public interface ISegmentContextFactory
    {
        SegmentContext CreateEntrySegment(string operationName, ICarrier carrier);

        SegmentContext CreateLocalSegment(string operationName);

        SegmentContext CreateExitSegment(string operationName, StringOrIntValue networkAddress);

        void Release(SegmentContext segmentContext);
    }
}
