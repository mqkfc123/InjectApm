using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CInject.Injections
{
    public class CurrentStopwatch
    {
        public Stopwatch Stopwatch { get; set; }

        public double ChildElapsed { get; set; }

        public CurrentStopwatch()
        {
            Stopwatch = new Stopwatch();
            ChildElapsed = 0L;
        }

        public void Start()
        {
            if (Stopwatch.IsRunning)
                return;

            Stopwatch.Start();
        }

        public void Stop()
        {
            if (!Stopwatch.IsRunning)
                return;

            Stopwatch.Stop();
        }

        public void Reset()
        {
            Stopwatch.Reset();
            Stopwatch = null;
            ChildElapsed = 0;
        }

        public double Elapsed()
        {
            return Stopwatch.ElapsedMilliseconds;
        }

    }

}
