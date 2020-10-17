using SkyApm.Core;

namespace CInject.Injections
{
    public class Startup
    {
        public Startup()
        {

        } 

        public void OnInvoke()
        {
            //var coreBuilder = new CoreBuilder();
            //coreBuilder.OnStarting(builder =>
            //{
            //    builder.RegisterModule<GrpcModule>();
            //});
            //coreBuilder.Build();
            InstrumentStartup startup = new InstrumentStartup();
            startup.StartAsync();
        }

        public void OnComplete()
        {

        }
    }
}
