using Autofac;
using SkyApm.Abstractions;
using SkyApm.Abstractions.Transport;
using SkyApm.Transport.Grpc.V6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyApm.Transport.Grpc
{
    public class GrpcModule : Module
    {
        #region Overrides of Module

        /// <summary>Override to add registrations to the container.</summary>
        /// <remarks>
        /// Note that the ContainerBuilder parameter is unique to this module.
        /// </remarks>
        /// <param name="builder">The builder through which components can be
        /// registered.</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ConnectService>().As<IExecutionService>().SingleInstance();
            builder.RegisterType<ConnectionManager>().SingleInstance();
            builder.RegisterType<SegmentReporter>().As<ISegmentReporter>().SingleInstance();


            builder.RegisterType<CLRStatsReporter>().As<ICLRStatsReporter>().SingleInstance();
            builder.RegisterType<PingCaller>().As<IPingCaller>().SingleInstance();
            builder.RegisterType<ServiceRegister>().As<IServiceRegister>().SingleInstance();


        }

        #endregion Overrides of Module
    }
}

