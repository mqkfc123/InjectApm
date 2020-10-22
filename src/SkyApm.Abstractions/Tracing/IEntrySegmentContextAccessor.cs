 
using SkyApm.Abstractions.Tracing.Segments;

namespace SkyApm.Abstractions.Tracing
{
    public interface IEntrySegmentContextAccessor
    {
        SegmentContext Context { get; set; }
    }
}