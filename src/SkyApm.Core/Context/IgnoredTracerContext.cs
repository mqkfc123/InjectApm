using SkyApm.Abstractions.Context;
using SkyApm.Abstractions.Context.Trace;
using SkyApm.Core.Context.Trace;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace SkyApm.Core.Context
{

    public class IgnoredTracerContext : ITracerContext
    {
        private static readonly NoopSpan noopSpan = new NoopSpan();
        private static readonly NoopEntrySpan noopEntrySpan = new NoopEntrySpan();

        private readonly Stack<ISpan> _spans = new Stack<ISpan>();

        public void Inject(IContextCarrier carrier)
        {
        }

        public void Extract(IContextCarrier carrier)
        {
        }

        public IContextSnapshot Capture { get; }

        public ISpan ActiveSpan
        {
            get
            {
                return _spans.Peek();
            }
        }

        public IDictionary<string, object> Properties { get; } = new Dictionary<string, object>();

        public void Continued(IContextSnapshot snapshot)
        {
        }

        public string GetReadableGlobalTraceId()
        {
            return string.Empty;
        }

        public ISpan CreateEntrySpan(string operationName)
        {
            _spans.Push(noopEntrySpan);
            return noopEntrySpan;
        }

        public ISpan CreateLocalSpan(string operationName)
        {
            _spans.Push(noopSpan);
            return noopSpan;
        }

        public ISpan CreateExitSpan(string operationName, string remotePeer)
        {
            var exitSpan = new NoopExitSpan(remotePeer);
            _spans.Push(exitSpan);
            return exitSpan;
        }

        public void StopSpan(ISpan span)
        {
            var _ = _spans.Pop();

            if (_spans.Count == 0)
            {
                ListenerManager.NotifyFinish(this);
                foreach (var item in Properties)
                {
                    if (item.Value is IDisposable)
                    {
                        ((IDisposable)item.Value).Dispose();
                    }
                }
            }
        }

        public static class ListenerManager
        {
            private static readonly List<IIgnoreTracerContextListener> _listeners = new List<IIgnoreTracerContextListener>();

            [MethodImpl(MethodImplOptions.Synchronized)]
            public static void Add(IIgnoreTracerContextListener listener)
            {
                _listeners.Add(listener);
            }

            public static void NotifyFinish(ITracerContext tracerContext)
            {
                foreach (var listener in _listeners)
                {
                    listener.AfterFinish(tracerContext);
                }
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            public static void Remove(IIgnoreTracerContextListener listener)
            {
                _listeners.Remove(listener);
            }
        }
    }
}
