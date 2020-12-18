using SkyApm.Abstractions;
using SkyApm.Abstractions.Context;
using SkyApm.Abstractions.Context.Trace;
using SkyApm.Abstractions.Utils;
using SkyApm.Core.Context.Trace;
using SkyApm.Logging;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace SkyApm.Core.Context
{

    public class TracingContext : ITracerContext
    {
        private long _lastWarningTimestamp = 0;
        private readonly ISampler _sampler;
        private readonly ITraceSegment _segment;
        private readonly Stack<ISpan> _activeSpanStacks;
        private int _spanIdGenerator;

        public TracingContext()
        {
            _sampler = DefaultSampler.Instance;
            _segment = new TraceSegment();
            _activeSpanStacks = new Stack<ISpan>();
        }

        /// <summary>
        /// Inject the context into the given carrier, only when the active span is an exit one.
        /// </summary>
        public void Inject(IContextCarrier carrier)
        {
            var span = InternalActiveSpan();
            if (!span.IsExit)
            {
                throw new InvalidOperationException("Inject can be done only in Exit Span");
            }

            var spanWithPeer = span as IWithPeerInfo;
            var peer = spanWithPeer.Peer;
            var peerId = spanWithPeer.PeerId;

            carrier.TraceSegmentId = _segment.TraceSegmentId;
            carrier.SpanId = span.SpanId;
            carrier.ParentApplicationInstanceId = _segment.ApplicationInstanceId;

            if (peerId == 0)
            {
                carrier.PeerHost = peer;
            }
            else
            {
                carrier.PeerId = peerId;
            }

            var refs = _segment.Refs;
            var last = _activeSpanStacks.Count;
            var firstSpan = _activeSpanStacks.ToArray()[last - 1];

            var metaValue = GetMetaValue(refs);

            carrier.EntryApplicationInstanceId = metaValue.EntryApplicationInstanceId;

            if (metaValue.OperationId == 0)
            {
                carrier.EntryOperationName = metaValue.OperationName;
            }
            else
            {
                carrier.EntryOperationId = metaValue.OperationId;
            }

            var parentOperationId = firstSpan.OperationId;
            if (parentOperationId == 0)
            {
                carrier.ParentOperationName = firstSpan.OperationName;
            }
            else
            {
                carrier.ParentOperationId = parentOperationId;
            }

            carrier.SetDistributedTraceIds(_segment.RelatedGlobalTraces);
        }

        /// <summary>
        /// Extract the carrier to build the reference for the pre segment.
        /// </summary>
        public void Extract(IContextCarrier carrier)
        {
            var traceSegmentRef = new TraceSegmentRef(carrier);
            _segment.Ref(traceSegmentRef);
            _segment.RelatedGlobalTrace(carrier.DistributedTraceId);
            var span = InternalActiveSpan();
            if (span is EntrySpan)
            {
                span.Ref(traceSegmentRef);
            }
        }

        /// <summary>
        /// Capture the snapshot of current context.
        /// </summary>
        public IContextSnapshot Capture => InternalCapture();

        public ISpan ActiveSpan => InternalActiveSpan();

        public IDictionary<string, object> Properties { get; } = new Dictionary<string, object>();

        public void Continued(IContextSnapshot snapshot)
        {
            var segmentRef = new TraceSegmentRef(snapshot);
            _segment.Ref(segmentRef);
            ActiveSpan.Ref(segmentRef);
            _segment.RelatedGlobalTrace(snapshot.DistributedTraceId);
        }

        public string GetReadableGlobalTraceId()
        {
            if (_segment.RelatedGlobalTraces != null && _segment.RelatedGlobalTraces.Count > 0)
            {
                return _segment.RelatedGlobalTraces.ToArray()[0].ToString();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Create an entry span
        /// </summary>
        public ISpan CreateEntrySpan(string operationName)
        {
            ISpan noopSpan = null;
            if (!EnsureLimitMechanismWorking(out noopSpan))
            {
                return noopSpan;
            }
            ISpan parentSpan = null;

            if (_activeSpanStacks.Count > 0)
            {
                parentSpan = _activeSpanStacks.Peek();
            }

            var parentSpanId = parentSpan?.SpanId ?? -1;

            if (parentSpan != null && parentSpan.IsEntry)
            {
                parentSpan.OperationName = operationName;
                return parentSpan.Start();
            }
            else
            {
                var entrySpan = new EntrySpan(_spanIdGenerator++, parentSpanId, operationName);

                entrySpan.Start();

                _activeSpanStacks.Push(entrySpan);

                return entrySpan;
            }
        }

        /// <summary>
        /// Create a local span
        /// </summary>
        public ISpan CreateLocalSpan(string operationName)
        {
            ISpan noopSpan = null;

            if (!EnsureLimitMechanismWorking(out noopSpan))
            {
                return noopSpan;
            }

            ISpan parentSpan = null;
            if (_activeSpanStacks.Count > 0)
                parentSpan = _activeSpanStacks.Peek();

            var parentSpanId = parentSpan?.SpanId ?? -1;

            var span = new LocalSpan(_spanIdGenerator++, parentSpanId, operationName);
            span.Start();
            _activeSpanStacks.Push(span);
            return span;
        }

        /// <summary>
        /// Create an exit span
        /// </summary>
        public ISpan CreateExitSpan(string operationName, string remotePeer)
        {

            ISpan parentSpan = null;
            if (_activeSpanStacks.Count > 0)
                parentSpan = _activeSpanStacks.Peek();

            if (parentSpan != null && parentSpan.IsExit)
            {
                return parentSpan.Start();
            }
            else
            {
                var parentSpanId = parentSpan?.SpanId ?? -1;
                var exitSpan = IsLimitMechanismWorking() ? (ISpan)new NoopExitSpan(remotePeer) : new ExitSpan(_spanIdGenerator++, parentSpanId, operationName, remotePeer);
                _activeSpanStacks.Push(exitSpan);
                return exitSpan.Start();
            }
        }

        /// <summary>
        /// Stop the given span, if and only if this one is the top element of {@link #activeSpanStack}. Because the tracing
        /// core must make sure the span must match in a stack module, like any program did.
        /// </summary>
        public void StopSpan(ISpan span)
        {
            ISpan lastSpan = null;
            try
            {
                if (_activeSpanStacks.Count > 0)
                    lastSpan = _activeSpanStacks.Peek();

                if (lastSpan == span)
                {
                    if (lastSpan is AbstractTracingSpan)
                    {
                        var tracingSpan = (AbstractTracingSpan)lastSpan;

                        if (tracingSpan.Finish(_segment))
                        {
                            _activeSpanStacks.Pop();
                        }
                    }
                    else
                    {
                        _activeSpanStacks.Pop();
                    }
                }
                else
                {
                    _activeSpanStacks.Clear();
                    throw new InvalidOperationException("执行 StopSpan 返回 Stopping the unexpected span = " + span);
                }

                if (_activeSpanStacks.Count == 0)
                {
                    Finish();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void Finish()
        {
            var finishedSegment = _segment.Finish(IsLimitMechanismWorking());

            if (!_segment.HasRef && _segment.IsSingleSpanSegment)
            {
                if (!_sampler.Sampled())
                {
                    finishedSegment.IsIgnore = true;
                }
            }

            ListenerManager.NotifyFinish(finishedSegment);

            foreach (var item in Properties)
            {
                if (item.Value is IDisposable)
                {
                    var disposable = (IDisposable)item.Value;
                    disposable.Dispose();
                }
            }
        }

        private ISpan InternalActiveSpan()
        {
            var span = _activeSpanStacks.Peek();
            if (span == null)
            {
                throw new InvalidOperationException("No active span.");
            }

            return span;
        }

        private IContextSnapshot InternalCapture()
        {
            var refs = _segment.Refs;

            var snapshot = new ContextSnapshot(_segment.TraceSegmentId, ActiveSpan.SpanId, _segment.RelatedGlobalTraces);

            var metaValue = GetMetaValue(refs);

            snapshot.EntryApplicationInstanceId = metaValue.EntryApplicationInstanceId;

            if (metaValue.OperationId == 0)
            {
                snapshot.EntryOperationName = metaValue.OperationName;
            }
            else
            {
                snapshot.EntryOperationId = metaValue.OperationId;
            }

            var last = _activeSpanStacks.Count;
            var parentSpan = _activeSpanStacks.ToArray()[last - 1]; //_activeSpanStacks.Last()

            if (parentSpan.OperationId == 0)
            {
                snapshot.ParentOperationName = parentSpan.OperationName;
            }
            else
            {
                snapshot.ParentOperationId = parentSpan.OperationId;
            }

            return snapshot;
        }

        private MetaValue GetMetaValue(List<ITraceSegmentRef> refs)
        {

            //(string operationName, int operationId, int entryApplicationInstanceId)
            MetaValue meateValue = new MetaValue();

            if (refs != null && refs.Count > 0)
            {
                var segmentRef = refs.ToArray()[0];
                return new MetaValue()
                {
                    OperationName = segmentRef.EntryOperationName,
                    OperationId = segmentRef.EntryOperationId,
                    EntryApplicationInstanceId = segmentRef.EntryApplicationInstanceId
                };
            }
            else
            {
                //var span = _activeSpanStacks.Last();
                var last = _activeSpanStacks.Count;
                var span = _activeSpanStacks.ToArray()[last - 1];

                return new MetaValue()
                {
                    OperationName = span.OperationName,
                    OperationId = span.OperationId,
                    EntryApplicationInstanceId = _segment.ApplicationInstanceId
                };

            }

        }

        private bool IsLimitMechanismWorking()
        {
            if (_spanIdGenerator < 300)
            {
                return false;
            }

            var currentTimeMillis = DateTimeOffsetUtcNow.ToUnixTimeMilliseconds();
            if (currentTimeMillis - _lastWarningTimestamp > 30 * 1000)
            {
                //todo log warning
                _lastWarningTimestamp = currentTimeMillis;
            }

            return true;
        }

        private bool EnsureLimitMechanismWorking(out ISpan noopSpan)
        {
            if (IsLimitMechanismWorking())
            {
                var span = new NoopSpan();
                _activeSpanStacks.Push(span);
                noopSpan = span;
                return false;
            }

            noopSpan = null;

            return true;
        }


        public static class ListenerManager
        {
            private static readonly IList<ITracingContextListener> _listeners = new List<ITracingContextListener>();


            [MethodImpl(MethodImplOptions.Synchronized)]
            public static void Add(ITracingContextListener listener)
            {
                _listeners.Add(listener);
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            public static void Remove(ITracingContextListener listener)
            {
                _listeners.Remove(listener);
            }

            public static void NotifyFinish(ITraceSegment traceSegment)
            {
                foreach (var listener in _listeners)
                {
                    listener.AfterFinished(traceSegment);
                }
            }
        }

        private class MetaValue
        {
            private string operationName;

            public string OperationName
            {
                get
                {
                    return operationName;
                }
                set
                {
                    operationName = value;
                }
            }

            private int operationId;
            public int OperationId
            {
                get
                {
                    return operationId;
                }
                set
                {
                    operationId = value;
                }
            }
            private int entryApplicationInstanceId;

            public int EntryApplicationInstanceId
            {
                get
                {
                    return entryApplicationInstanceId;
                }
                set
                {
                    entryApplicationInstanceId = value;
                }
            }

        }

    }
}
