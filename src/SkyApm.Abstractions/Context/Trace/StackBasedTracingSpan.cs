using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Abstractions.Context.Trace
{

    public abstract class StackBasedTracingSpan : AbstractTracingSpan
    {
        protected int _stackDepth;

        protected StackBasedTracingSpan(int spanId, int parentSpanId, string operationName)
            : base(spanId, parentSpanId, operationName)
        {
            _stackDepth = 0;
        }

        protected StackBasedTracingSpan(int spanId, int parentSpanId, int operationId)
            : base(spanId, parentSpanId, operationId)
        {
            _stackDepth = 0;
        }
    }
}
