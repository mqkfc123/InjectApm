using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Abstractions
{
    public interface ISampler
    {
        bool Sampled();

        void ForceSampled();
    }
}
