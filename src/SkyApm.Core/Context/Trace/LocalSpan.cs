using SkyApm.Abstractions.Context.Trace;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Core.Context.Trace
{

    public class LocalSpan : StackBasedTracingSpan
    {
        public LocalSpan(int spanId, int parentSpanId, string operationName) : base(spanId, parentSpanId, operationName)
        {
        }

        public LocalSpan(int spanId, int parentSpanId, int operationId) : base(spanId, parentSpanId, operationId)
        {
        }

        public override bool IsEntry => false;

        public override bool IsExit => false;
    }
}
