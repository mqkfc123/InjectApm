using SkyApm.Abstractions.Context.Ids;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Core.Context.Ids
{
    public class PropagatedTraceId : DistributedTraceId
    {
        public PropagatedTraceId(string id)
            : base(id)
        {
        }
    }
}
