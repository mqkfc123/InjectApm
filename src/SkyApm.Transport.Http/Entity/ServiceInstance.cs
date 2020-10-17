using System.Collections.Generic;

namespace SkyApm.Transport.Http.Entity
{

    public class ServiceInstance
    {
        public int serviceId { get; set; }
        public string instanceUUID { get; set; }
        public long time { get; set; }
        public List<KeyStringValuePair> tags { get; set; }
        public List<KeyStringValuePair> properties { get; set; }
    }

}
