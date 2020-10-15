using SkyApm.Abstractions.Tracing.Segments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyApm.Abstractions.Transport
{
    public interface ISegmentContextMapper
    {
        SegmentRequest Map(SegmentContext segmentContext);
    }
}
