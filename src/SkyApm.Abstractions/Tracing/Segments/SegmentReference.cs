using SkyApm.Abstractions.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

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
        private readonly List<SegmentReference> _references = new List<SegmentReference>();

        public bool Add(SegmentReference reference)
        {
            try
            {
                _references.Add(reference);
                return true;
            }
            catch
            {
                return false;
            }
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
