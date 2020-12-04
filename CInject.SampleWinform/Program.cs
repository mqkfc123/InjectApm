using SkyApm.Core;
using System.Windows.Forms;

namespace CInject.SampleWinform
{
    static class Program
    {
        static void Main()
        {
            InstrumentStartup startup = new InstrumentStartup();
            startup.StartAsync();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

    }
}
