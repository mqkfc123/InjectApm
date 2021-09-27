using System;
using System.Collections.Generic;
using System.Text;
using CInject.Injections.Attributes;
using CInject.Injections.Library;
using System.IO;
using SkyApm.Core.Context;
using SkyApm.Abstractions.Context.Trace;
using SkyApm.Abstractions.Context.Utils;
using SkyApm.Abstractions.Utils;

namespace CInject.Injections.Injectors
{
    [DependentFiles("ObjectSearch.xml", "CInject.Injections.dll", "LogInject.log4net.xml", "log4net.dll")]
    public class ObjectValueInject : ICInject
    {
        //private ITracingContext _tracingContext = WorkContext.TracingContext;

        public class ObjectSearch
        {
            public string[] PropertyNames;
        }

        private const string FileName = "ObjectSearch.xml";
        private DateTime _startTime;
        private CInjection _injection;
        private bool _disposed;
        //private SegmentContext _context;
        private CurrentStopwatch _parentStopwatch;

        private ISpan _parentTraceSpan;

        public void OnInvoke(CInjection injection)
        {
            try
            {
                this._parentStopwatch = GlobalStopwatch.GetStopwatch();
                GlobalStopwatch.Instance();
                if (_parentStopwatch == null)
                {
                    Logger.Info("Entry");
                    GlobalStopwatch.InstanceSpan(injection.Method.Name);
                }
                else
                {
                    Logger.Info("Local");
                    GlobalStopwatch.InstanceSpan(injection.Method.Name, "Local");
                }
                _parentTraceSpan = GlobalStopwatch.GetSpan();
                _parentTraceSpan.AsHttp();

                _injection = injection;
                _startTime = DateTime.Now;

                Logger.Debug($"{ _injection.Method.Name} startTime:{_startTime}");
                if (!Logger.IsDebugEnabled)
                    return;
                if (_injection == null)
                    return;
                if (!File.Exists(FileName))
                    return;
                if (!injection.IsValid())
                    return;

                var objectSearch = CachedSerializer.Deserialize<ObjectSearch>(File.ReadAllText(FileName), Encoding.UTF8);
                if (objectSearch == null || objectSearch.PropertyNames == null)
                    return;

                Logger.Info("============Method==========" + _injection.Method.ReflectedType.Namespace + "." + _injection.Method.DeclaringType.Name + "." + _injection.Method.Name);

                _parentTraceSpan.Tag("Assembly", _injection.Method.ReflectedType.Namespace + "." + _injection.Method.DeclaringType.Name);
                _parentTraceSpan.Tag("Method", _injection.Method.Name);

                var method = "";
                var value = "";

                foreach (string propertyName in objectSearch.PropertyNames)
                {
                    var dictionary = _injection.GetPropertyValue(propertyName);

                    foreach (var key in dictionary.Keys)
                    {
                        method = string.Format("Method {0} Argument #{1} :{2}= {3}", injection.Method.Name, key, propertyName, dictionary[key] ?? "<null>");
                        Logger.Debug(method);

                        value += propertyName + "=" + dictionary[key] ?? "<null> ";
                    }
                }
                if (!string.IsNullOrEmpty(value))
                    _parentTraceSpan.Log(DateTimeOffsetUtcNow.ToUnixTimeMilliseconds(),
                        new Dictionary<string, object>
                            {
                                {injection.Method.Name,value }
                            });

                var parameters = _injection.Method.GetParameters();

                var paramStr = "";
                if (_injection.Arguments != null)
                {
                    Logger.Debug("Arguments:" + _injection.Arguments.Length);
                    for (int i = 0; i < _injection.Arguments.Length; i++)
                    {
                        if (_injection.Arguments[i] == null)
                        {
                            Logger.Debug($"_injection.Arguments[{parameters[i].Name}]: is null ");
                            continue;
                        }
                        if (parameters[i].ParameterType.FullName == "System.Windows.Forms.Form"
                            || parameters[i].ParameterType.FullName == "Com.Yilz.PubSystem.Common.Util.RefreshHandler"
                            || parameters[i].ParameterType.FullName.Contains("DevComponents.DotNetBar"))
                        {
                            continue;
                        }

                        if (parameters[i].Name == "sender" || parameters[i].Name == "e")
                        {
                            continue;
                        }

                        Logger.Debug("参数名称:" + parameters[i].Name);
                        paramStr += parameters[i].Name + ":" + Newtonsoft.Json.JsonConvert.SerializeObject(_injection.Arguments[i]) + " \r\n";

                    }
                }

                _parentTraceSpan.Log(DateTimeOffsetUtcNow.ToUnixTimeMilliseconds(),
                    new Dictionary<string, object>
                            {
                                {"Arguments",paramStr }
                            }
                    );
            }
            catch (Exception ex)
            {
                Logger.Debug("OnInvoke ex:" + ex.Message);
                Logger.Error(ex);
            }

        }

        public void OnComplete()
        {
            try
            {
                if (_injection != null && _injection.IsValid())
                {
                    double elapsed = 0;
                    var _endTime = DateTime.Now;
                    Logger.Debug($"{ _injection.Method.Name} endTime:{_endTime}");

                    elapsed = GlobalStopwatch.Elapsed();

                    Logger.Debug($"========================模态窗口占用总时间========================{elapsed}ms");

                    _parentTraceSpan.Log(DateTimeOffsetUtcNow.ToUnixTimeMilliseconds(),
                          new Dictionary<string, object>
                                {
                                {"Message",$"OnComplete running at: {_endTime} ,executed in {_endTime.Subtract(_startTime).TotalMilliseconds}" }
                                }
                        );

                    Logger.Info("StopSpan");
                    ContextManager.StopSpan(_parentTraceSpan);

                    if (this._parentStopwatch == null)
                    {
                        Logger.Info(string.Format("{0} end executed in {1} mSec ", _injection.Method.Name, _endTime.Subtract(_startTime).TotalMilliseconds - GlobalStopwatch.GetStopwatch().ChildElapsed - elapsed));

                        GlobalStopwatch.Reset();
                        _parentTraceSpan = null;
                    }
                    else
                    {
                        this._parentStopwatch.ChildElapsed += elapsed;
                        this._parentStopwatch.ChildElapsed += GlobalStopwatch.GetStopwatch().ChildElapsed;
                        Logger.Info(string.Format("{0} executed in {1} mSec ", _injection.Method.Name, _endTime.Subtract(_startTime).TotalMilliseconds - GlobalStopwatch.GetStopwatch().ChildElapsed - elapsed));

                        GlobalStopwatch.SetStopwatch(this._parentStopwatch);
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.Debug("OnComplete ex:" + ex.Message);
                Logger.Error(ex);
            }

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