
using SkyApm.Abstractions.Tracing;
using SkyApm.Abstractions.Tracing.Segments;
using System.Collections.Generic;

namespace SkyApm.Core
{
    public class WorkContext
    {
      
        public static ITracingContext TracingContext { get; set; } 

        public static IEntrySegmentContextAccessor EntrySegmentContextAccessor { get; set; }

        public static List<SegmentContext> SegmentContext { get; set; }

    }

}
