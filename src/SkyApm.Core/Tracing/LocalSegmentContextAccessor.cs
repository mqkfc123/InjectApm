
using SkyApm.Abstractions.Tracing;
using SkyApm.Abstractions.Tracing.Segments;

namespace SkyApm.Core.Tracing
{
    public class LocalSegmentContextAccessor : ILocalSegmentContextAccessor
    {
        private SegmentContext _segmentContext;

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
