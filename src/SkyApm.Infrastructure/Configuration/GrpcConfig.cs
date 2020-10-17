using SkyApm.Abstractions.Config;
using System;

namespace SkyApm.Infrastructure.Configuration
{
    [Config("GrpcConfig")]
    public class GrpcConfig
    {
        public string Servers { get; set; }

        public int ConnectTimeout { get; set; }

        public int Timeout { get; set; }

        public int ReportTimeout { get; set; }

        public string Authentication { get; set; }
    }


    public static class GrpcConfigExtensions
    {
        public static DateTime GetTimeout(this GrpcConfig config)
        {
            return DateTime.UtcNow.AddMilliseconds(config.Timeout);
        }

        public static DateTime GetConnectTimeout(this GrpcConfig config)
        {
            return DateTime.UtcNow.AddMilliseconds(config.ConnectTimeout);
        }

        public static DateTime GetReportTimeout(this GrpcConfig config)
        {
            return DateTime.UtcNow.AddMilliseconds(config.ReportTimeout);
        }

        //public static Metadata GetMeta(this GrpcConfig config)
        //{
        //    if (string.IsNullOrEmpty(config.Authentication))
        //    {
        //        return null;
        //    }
        //    return new Metadata { new Metadata.Entry("Authentication", config.Authentication) };
        //}

    }

}
