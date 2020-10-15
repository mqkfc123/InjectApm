using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyApm.Abstractions.Config
{
    [Config("Sampling")]
    public class SamplingConfig
    {
        public int SamplePer3Secs { get; set; } = -1;

        public double Percentage { get; set; } = -1d;
    }
}
