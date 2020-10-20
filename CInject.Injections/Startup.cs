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
            InstrumentStartup startup = new InstrumentStartup();
            startup.StartAsync();
        }

        public void OnComplete()
        {

        }

    }
}
