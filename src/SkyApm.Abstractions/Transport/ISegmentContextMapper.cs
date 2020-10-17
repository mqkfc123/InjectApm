using SkyApm.Abstractions.Tracing.Segments;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Abstractions.Transport
{
    public interface ISegmentContextMapper
    {
        SegmentRequest Map(SegmentContext segmentContext);
    }
}
