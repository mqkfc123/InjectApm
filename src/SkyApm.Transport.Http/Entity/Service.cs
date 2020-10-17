using System.Collections.Generic;

namespace SkyApm.Transport.Http.Entity
{
    public class Service
    {
        public string serviceName { get; set; }
        public List<KeyStringValuePair> tags { get; set; }
        public List<KeyStringValuePair> properties { get; set; }
        public string type { get; set; }

    }
}
