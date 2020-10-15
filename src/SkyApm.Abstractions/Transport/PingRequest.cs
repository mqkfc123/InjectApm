using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyApm.Abstractions.Transport
{
    public class PingRequest
    {
        public int ServiceInstanceId { get; set; }

        public string InstanceId { get; set; }
    }
}
