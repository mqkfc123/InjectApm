using CInject.Injections.Library;
using SkyApm.Core;
using System;
using System.Threading;
using System.Windows.Forms;

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

                Application.EnterThreadModal += Application_EnterThreadModal;
                Application.LeaveThreadModal += Application_LeaveThreadModal;
              
            }
            catch (Exception ex)
            {
                Logger.Debug("startup:" + ex.Message);
            }
        }

        public void OnComplete()
        {

        }

        private static void Application_EnterThreadModal(object sender, EventArgs e)
        {
            Logger.Debug("GlobalStopwatch.Start:" + DateTime.Now.ToString());
           
            GlobalStopwatch.Start();
        }

        private static void Application_LeaveThreadModal(object sender, EventArgs e)
        {
            Logger.Debug("GlobalStopwatch.Stop:" + DateTime.Now.ToString());
            GlobalStopwatch.Stop();
        }

    }
}
