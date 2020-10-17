using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Abstractions.Transport
{
    public class ServiceRequest
    {
        public string ServiceName { get; set; }

        public Dictionary<string, string> Tags { get; set; }

        public Dictionary<string, string> Properties { get; set; }
    }
}
