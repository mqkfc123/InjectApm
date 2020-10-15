using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyApm.Abstractions.Transport
{
    public class AgentOsInfoRequest
    {
        public string OsName { get; set; }

        public string HostName { get; set; }

        public int ProcessNo { get; set; }

        public string[] IpAddress { get; set; }

        public string Language { get; set; }
    }
}
