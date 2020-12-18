using SkyApm.Abstractions.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Abstractions.Context.Trace
{

    public interface ISpan
    {
        /// <summary>
        /// Set the component id, which defines in ComponentsDefine
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        ISpan SetComponent(IComponent component);

        /// <summary>
        /// Only use this method in explicit skyWalking, like opentracing-skywalking-bridge. It it higher recommend
        /// don't use this for performance consideration.
        /// </summary>
        /// <param name="componentName"></param>
        /// <returns></returns>
        ISpan SetComponent(string componentName);

        /// <summary>
        /// Set a key:value tag on the Span.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        ISpan Tag(string key, string value);

        ISpan SetLayer(SpanLayer layer);

        /// <summary>
        /// Record an exception event of the current walltime timestamp.
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        ISpan Log(Exception exception);

        ISpan ErrorOccurred();

        bool IsEntry { get; }

        bool IsExit { get; }

        ISpan Log(long timestamp, IDictionary<string, object> @event);

        ISpan Start();

        int SpanId { get; }

        string OperationName { get; set; }

        int OperationId { get; set; }

        ISpan Start(long timestamp);

        void Ref(ITraceSegmentRef traceSegmentRef);
    }
}
