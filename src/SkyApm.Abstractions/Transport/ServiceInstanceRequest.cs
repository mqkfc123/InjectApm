using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Abstractions.Transport
{
    public class ServiceInstanceRequest
    {
        public int ServiceId { get; set; }

        public string InstanceUUID { get; set; }

        public AgentOsInfoRequest Properties { get; set; }
    }
}
