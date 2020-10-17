using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Abstractions.Tracing
{
    public interface IUniqueIdGenerator
    {
        UniqueId Generate();
    }
}
