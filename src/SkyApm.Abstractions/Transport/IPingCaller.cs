using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SkyApm.Abstractions.Transport
{
    public interface IPingCaller
    {
        void PingAsync(PingRequest request);
    }
}
