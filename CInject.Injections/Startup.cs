using Autofac;
using SkyApm.Core;
using SkyApm.Transport.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CInject.Injections
{
    public class Startup
    {
        public Startup()
        {

        } 

        public void OnInvoke()
        {
            var coreBuilder = new CoreBuilder();
            coreBuilder.OnStarting(builder =>
            {
                builder.RegisterModule<GrpcModule>();
            });
            coreBuilder.Build();
        }

        public void OnComplete()
        {

        }
    }
}
