using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Abstractions.Tracing
{
    public interface ICarrierHeaderCollection : IEnumerable<KeyValuePair<string, string>>
    {
        void Add(string key, string value);
    }
}
