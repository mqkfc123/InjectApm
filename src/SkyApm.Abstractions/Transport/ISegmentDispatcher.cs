using SkyApm.Abstractions.Tracing.Segments;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SkyApm.Abstractions.Transport
{
    public interface ISegmentDispatcher
    {
        bool Dispatch(SegmentContext segmentContext);

        void Flush();

        void Close();
    }
}
