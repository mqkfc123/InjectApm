using System.Collections.Generic;

namespace SkyApm.Transport.Http.Entity
{
    public class CLRMetricCollection
    {
        public List<CLRMetric> metrics { get; set; }
        public int serviceInstanceId { get; set; }
    }

}
