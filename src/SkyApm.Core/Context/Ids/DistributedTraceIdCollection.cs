using SkyApm.Abstractions.Context.Ids;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Core.Context.Ids
{

    public class DistributedTraceIdCollection : IDistributedTraceIdCollection
    {
        private readonly List<DistributedTraceId> _relatedGlobalTraces;

        public DistributedTraceIdCollection()
        {
            _relatedGlobalTraces = new List<DistributedTraceId>();
        }

        public IList<DistributedTraceId> GetRelatedGlobalTraces()
        {
            return _relatedGlobalTraces.AsReadOnly();
        }

        public void Append(DistributedTraceId distributedTraceId)
        {
            if (_relatedGlobalTraces.Count > 0 && _relatedGlobalTraces[0] is NewDistributedTraceId)
            {
                _relatedGlobalTraces.RemoveAt(0);
            }
            if (!_relatedGlobalTraces.Contains(distributedTraceId))
            {
                _relatedGlobalTraces.Add(distributedTraceId);
            }
        }
    }
}
