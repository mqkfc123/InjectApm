using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SkyApm.Abstractions
{
    public interface IInstrumentStartup
    {
        void StartAsync();

        void StopAsync();
    }
}
