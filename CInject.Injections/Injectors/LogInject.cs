﻿
using System;
using CInject.Injections.Attributes;
using CInject.Injections;
using CInject.Injections.Library;
using System.Text;
using System.Collections;

namespace CInject.Injections.Injectors
{
    [DependentFiles("CInject.Injections.dll", "LogInject.log4net.xml", "log4net.dll")]
    public class LogInject : ICInject
    {
        private bool _disposed;
        private CInjection _injection;

        #region ICInject Members

        public void OnInvoke(CInjection injection)
        {
            if (injection == null) return;

            _injection = injection;

            try
            {
                if (injection.IsValid())
                    Logger.Info(_injection.GetMessage("Invoked"));

                if (!Logger.IsDebugEnabled) return;

                var parameters = injection.Method.GetParameters();
                if (injection.Arguments != null)
                {
                    Logger.Debug(String.Format(">> Paramerters: {0}", injection.Arguments.Length));
                    for (int i = 0; i < injection.Arguments.Length; i++)
                    {
                        var currentArgument = injection.Arguments[i];
                        if (currentArgument == null)
                        {
                            Logger.Debug(String.Format("    [{0}]: <null>", parameters[i].Name));
                            continue;
                        }

                        if (currentArgument is IDictionary)
                        {
                            var dictionary = (IDictionary)currentArgument;
                            var dictionaryBuilder = new StringBuilder();
                            foreach (var key in dictionary.Keys)
                            {
                                dictionaryBuilder.AppendFormat("{0}={1}|", key, GetStringValue(dictionary[key]));
                            }

                            Logger.Debug(String.Format("    [{0}]: {1}", parameters[i].Name, dictionaryBuilder.ToString().TrimEnd(new[] { '|' })));
                        }
                        else if (currentArgument is ICollection)
                        {
                            ICollection collection = (ICollection)currentArgument;
                            IEnumerator enumerator = collection.GetEnumerator();
                            StringBuilder dictionaryBuilder = new StringBuilder();

                            while (enumerator.MoveNext())
                            {
                                dictionaryBuilder.AppendFormat("{0},", GetStringValue(enumerator.Current)).AppendLine();
                            }

                            Logger.Debug(String.Format("    [{0}]: {1}", parameters[i].Name, dictionaryBuilder.ToString().TrimEnd(new[] { ',' })));
                        }
                        else if (currentArgument is String)
                        {
                            Logger.Debug(String.Format("    [{0}]: {1}", parameters[i].Name, currentArgument.ToString()));
                        }
                        else if (currentArgument is IEnumerable)
                        {
                            IEnumerable enumerator = (IEnumerable)currentArgument;
                            StringBuilder dictionaryBuilder = new StringBuilder();

                            foreach (var item in enumerator)
                            {
                                dictionaryBuilder.AppendFormat("{0},", GetStringValue(item)).AppendLine();
                            }
                            Logger.Debug(String.Format("    [{0}]: {1}", parameters[i].Name, dictionaryBuilder.ToString().TrimEnd(new[] { ',' })));
                        }
                        else
                        {
                            Logger.Debug(String.Format("    [{0}]: {1}", parameters[i].Name, GetStringValue(currentArgument)));
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
            }
        }

        private string GetStringValue(object input)
        {
            if (input == null)
                return "null";

            try
            {
                return CachedSerializer.Serialize(input.GetType(), input, Encoding.UTF8);
            }
            catch // can not serialize, then call ToString() method.
            {
                return input.ToString();
            }
        }

        #endregion

        ~LogInject()
        {
            try
            {
                if (_disposed) return;
                DestroyObject();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
            }
        }

        public void OnComplete()
        {
            if (!_injection.IsValid()) return;

            Logger.Info(_injection.GetMessage("Completed"));
        }

        public void Dispose()
        {
            try
            {
                DestroyObject();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
            }
            finally
            {
                _disposed = true;
                GC.SuppressFinalize(this);
            }
        }

        private void DestroyObject()
        {
            Logger.Debug(_injection.GetMessage("Destroyed"));
            _injection = null;
        }
    }
}