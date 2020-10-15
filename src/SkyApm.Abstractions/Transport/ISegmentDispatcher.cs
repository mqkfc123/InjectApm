using SkyApm.Abstractions.Tracing.Segments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SkyApm.Abstractions.Transport
{
    public interface ISegmentDispatcher
    {
        bool Dispatch(SegmentContext segmentContext);

        Task Flush(CancellationToken token = default(CancellationToken));

        void Close();
    }
}
