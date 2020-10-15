using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyApm.Abstractions.Transport
{
    public class ServiceInstanceRequest
    {
        public int ServiceId { get; set; }

        public string InstanceUUID { get; set; }

        public AgentOsInfoRequest Properties { get; set; }
    }
}
