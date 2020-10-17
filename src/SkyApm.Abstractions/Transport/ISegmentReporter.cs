using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SkyApm.Abstractions.Transport
{
    public interface ISegmentReporter
    {
        void ReportAsync(ICollection<SegmentRequest> segmentRequests);
    }
}
