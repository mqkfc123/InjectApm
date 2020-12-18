using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Core.Context.Trace
{
    public class NoopEntrySpan : NoopSpan
    {
        public override bool IsEntry { get; } = true;
    }
}
