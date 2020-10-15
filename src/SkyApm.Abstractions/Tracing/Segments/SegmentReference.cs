using SkyApm.Abstractions.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyApm.Abstractions.Tracing.Segments
{
    public class SegmentReference
    {
        public Reference Reference { get; set; }

        public UniqueId ParentSegmentId { get; set; }

        public int ParentSpanId { get; set; }

        public int ParentServiceInstanceId { get; set; }

        public int EntryServiceInstanceId { get; set; }

        public StringOrIntValue NetworkAddress { get; set; }

        public StringOrIntValue EntryEndpoint { get; set; }

        public StringOrIntValue ParentEndpoint { get; set; }
    }

    public enum Reference
    {
        CrossProcess = 0,
        CrossThread = 1
    }

    public class SegmentReferenceCollection : IEnumerable<SegmentReference>
    {
        private readonly HashSet<SegmentReference> _references = new HashSet<SegmentReference>();

        public bool Add(SegmentReference reference)
        {
            return _references.Add(reference);
        }

        public IEnumerator<SegmentReference> GetEnumerator()
        {
            return _references.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _references.GetEnumerator();
        }

        public int Count => _references.Count;
    }
}
