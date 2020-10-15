using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyApm.Abstractions.Transport
{
    public class ServiceRequest
    {
        public string ServiceName { get; set; }

        public Dictionary<string, string> Tags { get; set; }

        public Dictionary<string, string> Properties { get; set; }
    }
}
