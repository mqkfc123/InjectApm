using System;
using System.Collections.Generic;
using System.Text;
using CInject.Injections.Attributes;
using CInject.Injections.Library;
using System.IO;
using SkyApm.Abstractions.Tracing;
using SkyApm.Core.Tracing;
using SkyApm.Abstractions.Tracing.Segments;
using SkyApm.Core;

namespace CInject.Injections.Injectors
{
    [DependentFiles("ObjectSearch.xml", "CInject.Injections.dll", "LogInject.log4net.xml", "log4net.dll")]
    public class ObjectValueInject : ICInject
    {
        private ITracingContext  _tracingContext = WorkContext.TracingContext;

        public class ObjectSearch
        {
            public string[] PropertyNames;
        }

        private const string FileName = "ObjectSearch.xml";
        private DateTime _startTime;
        private CInjection _injection;
        private bool _disposed;
        private SegmentContext _context;

        public void OnInvoke(CInjection injection)
        {
            try
            {
               _context = _tracingContext.CreateEntrySegmentContext(injection.Method.Name, new TextCarrierHeaderCollection(new Dictionary<string, string>()));

                _injection = injection;
                _startTime = DateTime.Now;

                if (!Logger.IsDebugEnabled) return;
                if (_injection == null) return;
                if (!File.Exists(FileName)) return;
                if (!injection.IsValid()) return;

                var objectSearch = CachedSerializer.Deserialize<ObjectSearch>(File.ReadAllText(FileName), Encoding.UTF8);
                if (objectSearch == null || objectSearch.PropertyNames == null) return;

                _context.Span.AddTag("Method", injection.Method.Name);
                foreach (string propertyName in objectSearch.PropertyNames)
                {
                    var dictionary = _injection.GetPropertyValue(propertyName);
                    var method = "";
                    var value = "";
                    foreach (var key in dictionary.Keys)
                    {
                        method = string.Format("Method {0} Argument #{1} :{2}= {3}", injection.Method.Name, key, propertyName, dictionary[key] ?? "<null>");
                        Logger.Debug(method);

                        value += propertyName + "=" + dictionary[key] ?? "<null> ";
                    }

                    _context.Span.AddLog(LogEvent.Event($"{injection.Method.Name} :{value}"));
                }

            }
            catch (Exception ex)
            {
                Logger.Debug("OnInvoke ex:" + ex.Message);
                Logger.Error(ex);
            }

        }

        public void OnComplete()
        {
            if (_injection != null && _injection.IsValid())
                Logger.Info(string.Format("{0} executed in {1} mSec", _injection.Method.Name, DateTime.Now.Subtract(_startTime).TotalMilliseconds));

            _context.Span.AddLog(LogEvent.Message($"OnComplete running at: {DateTime.Now} ,executed in {DateTime.Now.Subtract(_startTime).TotalMilliseconds}"));
            _tracingContext.Release(_context);
        }


        ~ObjectValueInject()
        {
            if (_disposed) return;

            DestroyObject();
        }

        private void DestroyObject()
        {
            _injection = null;
        }

        public void Dispose()
        {
            _injection = null;
            _disposed = true;
            GC.SuppressFinalize(this);
        }

    }
}