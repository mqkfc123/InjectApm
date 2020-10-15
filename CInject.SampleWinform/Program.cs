using Autofac;
using SkyApm.Abstractions;
using SkyApm.Abstractions.Config;
using SkyApm.Abstractions.Tracing;
using SkyApm.Abstractions.Transport;
using SkyApm.Core;
using SkyApm.Core.Service;
using SkyApm.Core.Tracing;
using SkyApm.Core.Transport;
using SkyApm.Infrastructure.Configuration;
using SkyApm.Infrastructure.Logging;
using SkyApm.Logging;
using SkyApm.Transport.Grpc;
using SkyApm.Transport.Grpc.V6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CInject.SampleWinform
{
    static class Program
    {
        static void Main()
        {
            //var coreBuilder = new CoreBuilder();
            //coreBuilder.OnStarting(builder =>
            //{
            //    builder.RegisterModule<GrpcModule>();
            //    builder.RegisterModule<AutofacModule>();
            //});
            //coreBuilder.Build();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
