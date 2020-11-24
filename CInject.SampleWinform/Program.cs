using SkyApm.Core;
using System;
using System.Windows.Forms;

namespace CInject.SampleWinform
{
    static class Program
    {
        static void Main()
        {

            InstrumentStartup startup = new InstrumentStartup();
            startup.StartAsync();

            Application.EnterThreadModal += Application_EnterThreadModal;
            Application.LeaveThreadModal += Application_LeaveThreadModal;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        private static void Application_EnterThreadModal(Object sender, EventArgs e)
        {
            //MessageBox.Show("You are in the Application.LeaveThreadModal event.");
        }

        private static void Application_LeaveThreadModal(Object sender, EventArgs e)
        {
            //MessageBox.Show("You are in the Application.LeaveThreadModal event.");
        }

    }
}
