
using SkyApm.Abstractions.Config;

namespace SkyApm.Infrastructure.Configuration
{
    public class SkyApmSettings
    {
        public GrpcConfig GrpcConfig { get; set; }

        public InstrumentConfig SkyWalking { get; set; }

        public TransportConfig Transport { get; set; }

        public SamplingConfig Sampling { get; set; }

    }

}
