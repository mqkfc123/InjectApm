using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Abstractions.Config
{
    [Config("Transport")]
    public class TransportConfig
    {
        public int QueueSize { get; set; } = 30000;

        /// <summary>
        /// Flush Interval Millisecond
        /// </summary>
        public int Interval { get; set; } = 3000;

        /// <summary>
        /// Data queued beyond this time will be discarded.
        /// </summary>
        public int BatchSize { get; set; } = 3000;

        public string ProtocolVersion { get; set; } = ProtocolVersions.V6;
    }

    public static class ProtocolVersions
    {
        public static string V5 { get; } = "v5";

        public static string V6 { get; } = "v6";
    }
}
