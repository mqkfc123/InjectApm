using SkyApm.Abstractions;
using SkyApm.Abstractions.Config;
using SkyApm.Abstractions.Tracing;
using SkyApm.Abstractions.Transport;
using SkyApm.Core.Service;
using SkyApm.Core.Tracing;
using SkyApm.Core.Transport;
using SkyApm.Infrastructure.Configuration;
using SkyApm.Infrastructure.Logging;
using SkyApm.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Core
{
    internal class CoreModule 
    {
        #region Overrides of Module

        /// <summary>Override to add registrations to the container.</summary>
        /// <remarks>
        /// Note that the ContainerBuilder parameter is unique to this module.
        /// </remarks>
        /// <param name="builder">The builder through which components can be
        /// registered.</param>
        protected  void Load()
        {

            //builder.RegisterType<NLoggerFactory>().As<ILoggerFactory>().SingleInstance();
            //builder.RegisterType<ConfigAccessor>().As<IConfigAccessor>().SingleInstance();

            //builder.RegisterType<AsyncQueueSegmentDispatcher>().As<ISegmentDispatcher>().SingleInstance();
            //builder.RegisterType<SegmentContextMapper>().As<ISegmentContextMapper>().SingleInstance();

            //builder.RegisterType<TracingContext>().As<ITracingContext>().SingleInstance();
            //builder.RegisterType<SegmentContextFactory>().As<ISegmentContextFactory>().SingleInstance();
            //builder.RegisterType<CarrierPropagator>().As<ICarrierPropagator>().SingleInstance();
            //builder.RegisterType<UniqueIdGenerator>().As<IUniqueIdGenerator>().SingleInstance();
            //*builder.RegisterType<Carrier>().As<ICarrier>().SingleInstance();
            //*builder.RegisterType<NullableCarrier>().As<ICarrier>().SingleInstance();

            //builder.RegisterType<RegisterService>().As<IExecutionService>().SingleInstance();
            //builder.RegisterType<CLRStatsService>().As<IExecutionService>().SingleInstance();
            //builder.RegisterType<PingService>().As<IExecutionService>().SingleInstance();
            //builder.RegisterType<SegmentReportService>().As<IExecutionService>().SingleInstance();

            //builder.RegisterType<RuntimeEnvironment>().As<IRuntimeEnvironment>().SingleInstance();
            //builder.RegisterType<InstrumentStartup>().As<IInstrumentStartup>().SingleInstance();
        }

        #endregion Overrides of Module
    }
}

