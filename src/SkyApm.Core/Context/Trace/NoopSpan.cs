using SkyApm.Abstractions.Components;
using SkyApm.Abstractions.Context.Trace;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Core.Context.Trace
{

    public class NoopSpan : ISpan
    {
        public int SpanId => 0;

        public string OperationName
        {
            get { return string.Empty; }

            set { }

        }

        public int OperationId
        {
            get { return 0; }

            set { }
        }

        public ISpan ErrorOccurred()
        {
            return this;
        }

        public ISpan Log(Exception exception)
        {
            return this;
        }

        public ISpan Log(long timestamp, IDictionary<string, object> @event)
        {
            return this;
        }

        public void Ref(ITraceSegmentRef traceSegmentRef)
        {
        }

        public ISpan SetComponent(IComponent component)
        {
            return this;
        }

        public ISpan SetComponent(string componentName)
        {
            return this;
        }

        public ISpan SetLayer(SpanLayer layer)
        {
            return this;
        }

        public ISpan Start()
        {
            return this;
        }

        public ISpan Start(long timestamp)
        {
            return this;
        }

        public ISpan Tag(string key, string value)
        {
            return this;
        }


        public virtual bool IsEntry => false;

        public virtual bool IsExit => false;
    }
}
