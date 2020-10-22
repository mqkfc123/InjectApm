

using SkyApm.Abstractions.Tracing.Segments;

namespace SkyApm.Abstractions.Tracing
{
    public interface ILocalSegmentContextAccessor
    {
        SegmentContext Context { get; set; }
    }
}