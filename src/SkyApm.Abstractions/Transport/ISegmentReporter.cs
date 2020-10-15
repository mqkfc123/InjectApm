using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SkyApm.Abstractions.Transport
{
    public interface ISegmentReporter
    {
        Task ReportAsync(IReadOnlyCollection<SegmentRequest> segmentRequests,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}
