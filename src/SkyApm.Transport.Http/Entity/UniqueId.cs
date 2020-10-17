using System;
using System.Collections.Generic;

namespace SkyApm.Transport.Http.Entity
{

    [Serializable]
    public class UniqueId
    {
        public List<long> idParts { get; set; }

    }
}
