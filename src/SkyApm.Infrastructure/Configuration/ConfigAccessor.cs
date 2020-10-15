using SkyApm.Abstractions.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;
using System.IO;
using Newtonsoft.Json;

namespace SkyApm.Infrastructure.Configuration
{

    public class ConfigAccessor : IConfigAccessor
    {
        public static SkyApmSettings _SkyApmSettings;

        public ConfigAccessor()
        {
            LoadSettings();
        }

        public T Get<T>() where T : class, new()
        {
            var config = typeof(T).GetCustomAttributes(typeof(ConfigAttribute), false).FirstOrDefault();
            var instance = New<T>.Instance();
            Type type = typeof(T);

            if (type.Name == "GrpcConfig")
            {
                instance = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(_SkyApmSettings.GrpcConfig));
            }
            if (type.Name == "InstrumentConfig")
            {
                instance = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(_SkyApmSettings.SkyWalking));
            }
            if (type.Name == "TransportConfig")
            {
                instance = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(_SkyApmSettings.Transport));
            }

            return instance;
        }

        public T Value<T>(string key, params string[] sections)
        {
            throw new NotSupportedException();
        }


        private static class New<T> where T : new()
        {
            public static readonly Func<T> Instance = Expression.Lambda<Func<T>>
            (
                Expression.New(typeof(T))
            ).Compile();
        }


        private void LoadSettings()
        {

            var path = Path.Combine("SkyApm", "SkyApmSettings.json");

            if (!Directory.Exists(Path.Combine("SkyApm")))
            {
                Directory.CreateDirectory(Path.Combine("SkyApm"));
            }

            if (!File.Exists(path))
            {
                File.Create(path).Dispose();

                _SkyApmSettings = new SkyApmSettings()
                {
                    GrpcConfig = new GrpcConfig
                    {
                        Authentication = "Auth",
                        ConnectTimeout = 6000,
                        Servers = "10.16.2.113:11800",
                        Timeout = 6000,
                        ReportTimeout = 6000
                    },
                    SkyWalking = new InstrumentConfig
                    {
                        ServiceName = "Auth",
                        HeaderVersions = null,
                        Namespace = ""
                    },
                    Transport = new TransportConfig
                    {
                        BatchSize = 1,
                        Interval = 6000,
                        ProtocolVersion = "V6",
                        QueueSize = 1
                    }
                };

                File.WriteAllText(path, JsonConvert.SerializeObject(_SkyApmSettings), Encoding.UTF8);
            }
            else
            {
                _SkyApmSettings = JsonConvert
                    .DeserializeObject<SkyApmSettings>(File.ReadAllText(path, Encoding.UTF8));
            }

        }

    }
}
