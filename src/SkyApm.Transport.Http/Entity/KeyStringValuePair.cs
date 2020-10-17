
using System;

namespace SkyApm.Transport.Http.Entity
{

    [Serializable]
    public class KeyStringValuePair
    {
        public string key { get; set; }
        public string value { get; set; }
    }
}