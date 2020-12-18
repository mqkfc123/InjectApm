﻿using SkyApm.Abstractions.Common;
using SkyApm.Abstractions.Components;
using SkyApm.Abstractions.Transport;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Abstractions.Context.Trace
{

    public abstract class AbstractTracingSpan : ISpan
    {
        protected readonly int _spanId;
        protected readonly int _parnetSpanId;
        protected Dictionary<string, string> _tags;
        protected string _operationName;
        protected int _operationId;
        protected SpanLayer? _layer;

        /// <summary>
        /// The start time of this Span.
        /// </summary>
        protected long _startTime;

        /// <summary>
        /// The end time of this Span.
        /// </summary>
        protected long _endTime;

        protected bool _errorOccurred = false;

        protected int _componentId = 0;

        protected string _componentName;

        /// <summary>
        /// Log is a concept from OpenTracing spec. <p> {@see https://github.com/opentracing/specification/blob/master/specification.md#log-structured-data}
        /// </summary>
        protected ICollection<LogDataEntity> _logs;

        /// <summary>
        /// The refs of parent trace segments, except the primary one. For most RPC call, {@link #refs} contains only one
        /// element, but if this segment is a start span of batch process, the segment faces multi parents, at this moment,
        /// we use this {@link #refs} to link them.
        /// </summary>
        protected ICollection<ITraceSegmentRef> _refs;

        protected AbstractTracingSpan(int spanId, int parentSpanId, string operationName)
        {
            _operationName = operationName;
            _spanId = spanId;
            _parnetSpanId = parentSpanId;
        }

        protected AbstractTracingSpan(int spanId, int parentSpanId, int operationId)
        {
            _operationName = null;
            _operationId = operationId;
            _spanId = spanId;
            _parnetSpanId = parentSpanId;
        }

        public abstract bool IsEntry { get; }

        public abstract bool IsExit { get; }

        public virtual int SpanId => _spanId;

        public virtual string OperationName
        {
            get { return _operationName; }
            set
            {
                _operationName = value;
                _operationId = 0;
            }
        }

        public virtual int OperationId
        {
            get { return _operationId; }
            set
            {
                _operationId = value;
                _operationName = null;
            }
        }

        public virtual ISpan SetComponent(IComponent component)
        {
            _componentId = component.Id;
            return this;
        }

        public virtual ISpan SetComponent(string componentName)
        {
            _componentName = componentName;
            return this;
        }

        public virtual ISpan Tag(string key, string value)
        {
            if (_tags == null)
            {
                _tags = new Dictionary<string, string>();
            }

            _tags.Add(key, value);
            return this;
        }

        public virtual ISpan SetLayer(SpanLayer layer)
        {
            _layer = layer;
            return this;
        }

        private long ToUnixTimeMilliseconds()
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var t3 = Convert.ToInt64((DateTime.Now - epoch).TotalMilliseconds);
            return t3;
        }

        /// <summary>
        /// Record an exception event of the current walltime timestamp.
        /// </summary>
        public virtual ISpan Log(Exception exception)
        {
            EnsureLogs();
            _logs.Add(new LogDataEntity.Builder()
                .Add("event", "error")
                .Add("error.kind", exception.GetType().FullName)
                .Add("message", exception.Message)
                .Add("stack", exception.StackTrace)
                .Build(ToUnixTimeMilliseconds()));
            return this;
        }

        public virtual ISpan ErrorOccurred()
        {
            _errorOccurred = true;
            return this;
        }

        public virtual ISpan Log(long timestamp, IDictionary<string, object> events)
        {
            EnsureLogs();
            var builder = new LogDataEntity.Builder();
            foreach (var @event in events)
            {
                builder.Add(@event.Key, @event.Value.ToString());
            }

            _logs.Add(builder.Build(timestamp));
            return this;
        }

        public virtual ISpan Start()
        {
            _startTime = ToUnixTimeMilliseconds();
            return this;
        }

        public virtual ISpan Start(long timestamp)
        {
            _startTime = timestamp;
            return this;
        }

        public virtual void Ref(ITraceSegmentRef traceSegmentRef)
        {
            if (_refs == null)
            {
                _refs = new List<ITraceSegmentRef>();
            }

            if (!_refs.Contains(traceSegmentRef))
            {
                _refs.Add(traceSegmentRef);
            }
        }

        /// <summary>
        /// Finish the active Span. When it is finished, it will be archived by the given {@link TraceSegment}, which owners it
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public virtual bool Finish(ITraceSegment owner)
        {
            _endTime = ToUnixTimeMilliseconds();
            owner.Archive(this);
            return true;
        }

        public virtual SpanRequest Transform()
        {
            var spanRequest = new SpanRequest
            {
                SpanId = _spanId,
                ParentSpanId = _parnetSpanId,
                StartTime = _startTime,
                EndTime = _endTime,
                OperationName = new StringOrIntValue(_operationId, _operationName),
                Component = new StringOrIntValue(_componentId, _componentName),
                IsError = _errorOccurred
            };

            if (IsEntry)
            {
                spanRequest.SpanType = 0;
            }
            else if (IsExit)
            {
                spanRequest.SpanType = 1;
            }
            else
            {
                spanRequest.SpanType = 2;
            }

            if (_layer.HasValue)
            {
                spanRequest.SpanLayer = (int)_layer.Value;
            }


            foreach (var tag in _tags)
            {
                spanRequest.Tags.Add(new KeyValuePair<string, string>(tag.Key, tag.Value));
            }

            if (_logs != null)
            {
                foreach (var logDataEntity in _logs)
                {
                    spanRequest.Logs.Add(logDataEntity.Transform());
                }
            }

            if (_refs == null) return spanRequest;
            foreach (var traceSegmentRef in _refs)
            {
                spanRequest.References.Add(traceSegmentRef.Transform());
            }

            return spanRequest;
        }

        private void EnsureLogs()
        {
            if (_logs == null)
            {
                _logs = new LinkedList<LogDataEntity>();
            }
        }
    }
}
