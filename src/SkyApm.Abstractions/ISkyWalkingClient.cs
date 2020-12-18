using SkyApm.Abstractions.Common;
using SkyApm.Abstractions.Transport;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Abstractions
{
    public interface ISkyWalkingClient
    {
        void CollectAsync(IEnumerable<TraceSegmentRequest> request);
    }
}
