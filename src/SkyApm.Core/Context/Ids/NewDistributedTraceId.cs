using SkyApm.Abstractions.Context.Ids;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Core.Context.Ids
{
    public class NewDistributedTraceId : DistributedTraceId
    {
        public NewDistributedTraceId()
            : base(GlobalIdGenerator.Generate())
        {
        }
    }
}
