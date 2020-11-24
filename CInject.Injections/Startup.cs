using CInject.Injections.Library;
using SkyApm.Core;
using System;

namespace CInject.Injections
{
    public class Startup
    {
        
        public void OnInvoke()
        {
            try
            {
                InstrumentStartup startup = new InstrumentStartup();
                startup.StartAsync();
            }
            catch (Exception ex)
            {
                Logger.Debug("startup:" + ex.Message);
            }
        }

        public void OnComplete()
        {

        }

    }
}
