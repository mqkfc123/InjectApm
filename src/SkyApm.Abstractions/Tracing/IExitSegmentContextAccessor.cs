

using SkyApm.Abstractions.Tracing.Segments;

namespace SkyApm.Abstractions.Tracing
{
    public interface IExitSegmentContextAccessor
    {
        SegmentContext Context { get; set; }
    }
}