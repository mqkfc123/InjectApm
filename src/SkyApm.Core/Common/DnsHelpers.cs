
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace SkyApm.Core.Common
{
    public static class DnsHelpers
    {
        public static string GetHostName()
        {
            return Dns.GetHostName();
        }

        public static string[] GetIpV4s()
        {
            try
            {
                var ipAddresses = Dns.GetHostAddresses(Dns.GetHostName());

                List<string> ips = new List<string>();

                foreach (var item in ipAddresses)
                {
                    if (item.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ips.Add(item.ToString());
                    }
                }
                return ips.ToArray();
            }
            catch
            {
                return new string[0];
            }
        }
    }
}