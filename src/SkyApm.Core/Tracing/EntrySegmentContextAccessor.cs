


using SkyApm.Abstractions.Tracing;
using SkyApm.Abstractions.Tracing.Segments;

namespace SkyApm.Core.Tracing
{
    public class EntrySegmentContextAccessor : IEntrySegmentContextAccessor
    {
        private SegmentContext _segmentContext ;

        public SegmentContext Context
        {
            get
            {
                return _segmentContext;
            }
            set
            {
                _segmentContext = value;
            }
        }


    }
}