using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Abstractions.Config
{
    [Config("Sampling")]
    public class SamplingConfig
    {
        public int SamplePer3Secs { get; set; } = -1;

        public double Percentage { get; set; } = -1d;
    }
}
