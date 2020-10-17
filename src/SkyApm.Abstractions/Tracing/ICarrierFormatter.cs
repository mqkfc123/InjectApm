using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Abstractions.Tracing
{
    public interface ICarrierFormatter
    {
        string Key { get; }

        bool Enable { get; }

        ICarrier Decode(string content);

        string Encode(ICarrier carrier);
    }
}
