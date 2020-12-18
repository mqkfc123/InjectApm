using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Abstractions.Transport
{
    public interface ITraceDispatcher
    {
        bool Dispatch(TraceSegmentRequest segment);

        void Flush();

        void Close();
    }
}
