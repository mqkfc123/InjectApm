using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyApm.Abstractions.Tracing
{
    public interface ICarrierHeaderCollection : IEnumerable<KeyValuePair<string, string>>
    {
        void Add(string key, string value);
    }
}
