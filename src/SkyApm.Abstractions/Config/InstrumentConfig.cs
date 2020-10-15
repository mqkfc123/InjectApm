using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyApm.Abstractions.Config
{

    [Config("SkyWalking")]
    public class InstrumentConfig
    {
        public string Namespace { get; set; }

        public string ServiceName { get; set; }

        public string[] HeaderVersions { get; set; }
    }

    public static class HeaderVersions
    {
        public static string SW3 { get; } = "sw3";

        public static string SW6 { get; } = "sw6";
    }
}
