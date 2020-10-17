using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Abstractions.Transport
{
    public class PingRequest
    {
        public int ServiceInstanceId { get; set; }

        public string InstanceId { get; set; }
    }
}
