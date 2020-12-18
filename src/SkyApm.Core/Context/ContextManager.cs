using SkyApm.Abstractions;
using SkyApm.Abstractions.Context;
using SkyApm.Abstractions.Context.Trace;
using SkyApm.Infrastructure.Logging;
using SkyApm.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SkyApm.Core.Context
{
    public class ContextManager : ITracingContextListener, IIgnoreTracerContextListener
    {
        static ContextManager()
        {
            var manager = new ContextManager();
            TracingContext.ListenerManager.Add(manager);
            IgnoredTracerContext.ListenerManager.Add(manager); 
        }

        private static ITracerContext _context;

        private static ITracerContext GetOrCreateContext(string operationName, bool forceSampling)
        {
            var context = _context;

            if (context == null)
            {
                if (string.IsNullOrEmpty(operationName))
                {
                    // logger.debug("No operation name, ignore this trace.");
                    _context = new IgnoredTracerContext();
                }
                else
                { 
                    if (WorkContext.RuntimeEnvironment.Initialized)
                    {
                        var sampler = DefaultSampler.Instance;
                        if (forceSampling || sampler.Sampled())
                        {
                            _context = new TracingContext();
                        }
                        else
                        {
                            _context = new IgnoredTracerContext();
                            throw new Exception("IgnoredTracerContext1");
                        }
                    }
                    else
                    {
                        _context = new IgnoredTracerContext();
                        throw new Exception("IgnoredTracerContext2");
                    }
                }
            }

            return _context;
        }

        private static ITracerContext Context => _context;

        public static string GlobalTraceId
        {
            get
            {
                if (_context != null)
                {
                    return _context.GetReadableGlobalTraceId();
                }

                return "N/A";
            }
        }

        public static IContextSnapshot Capture => _context?.Capture;

        public static IDictionary<string, object> ContextProperties => _context?.Properties;

        public static ISpan CreateEntrySpan(string operationName, IContextCarrier carrier)
        {
            var samplingService = DefaultSampler.Instance;

            samplingService.ForceSampled();
            var context = GetOrCreateContext(operationName, true);
         
            var span = context.CreateEntrySpan(operationName);
            //context.Extract(carrier);
            return span;

            //if (carrier != null && carrier.IsValid)
            //{
            //    samplingService.ForceSampled();
            //    var context = GetOrCreateContext(operationName, true);
            //    var span = context.CreateEntrySpan(operationName);
            //    context.Extract(carrier);
            //    return span;
            //}
            //else
            //{
            //    var context = GetOrCreateContext(operationName, false);

            //    return context.CreateEntrySpan(operationName);
            //}
        }

        public static ISpan CreateLocalSpan(string operationName)
        {
            var context = GetOrCreateContext(operationName, false);
            return context.CreateLocalSpan(operationName);
        }

        public static ISpan CreateExitSpan(string operationName, IContextCarrier carrier, string remotePeer)
        {
            var context = GetOrCreateContext(operationName, false);
            var span = context.CreateExitSpan(operationName, remotePeer);
            context.Inject(carrier);
            return span;
        }

        public static ISpan CreateExitSpan(string operationName, string remotePeer)
        {
            var context = GetOrCreateContext(operationName, false);
            var span = context.CreateExitSpan(operationName, remotePeer);
            return span;
        }

        public static void Inject(IContextCarrier carrier)
        {
            Context?.Inject(carrier);
        }

        public static void Extract(IContextCarrier carrier)
        {
            Context?.Extract(carrier);
        }

        public static void Continued(IContextSnapshot snapshot)
        {
            if (snapshot.IsValid && !snapshot.IsFromCurrent)
            {
                Context?.Continued(snapshot);
            }
        }

        public static void StopSpan()
        {
            StopSpan(ActiveSpan);
        }

        public static ISpan ActiveSpan
        {
            get { return Context?.ActiveSpan; }
        }

        public static void StopSpan(ISpan span)
        {
            Context?.StopSpan(span);
        }

        public void AfterFinished(ITraceSegment traceSegment)
        {
            _context = null;
        }

        public void AfterFinish(ITracerContext tracerContext)
        {
            _context = null;
        }
    }
}
