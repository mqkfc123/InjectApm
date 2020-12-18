using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Abstractions.Context.Ids
{
    public interface IDistributedTraceIdCollection
    {
        IList<DistributedTraceId> GetRelatedGlobalTraces();

        void Append(DistributedTraceId distributedTraceId);
    }
}
