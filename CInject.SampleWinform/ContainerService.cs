using SkyApm.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CInject.SampleWinform
{
    public class ContainerService
    {

        public IInstrumentStartup InstrumentStartup;

        public ContainerService(IInstrumentStartup instrumentStartup)
        {
            InstrumentStartup = instrumentStartup;
        }

    }
}
