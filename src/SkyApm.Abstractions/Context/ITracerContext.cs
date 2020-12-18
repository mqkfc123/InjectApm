using SkyApm.Abstractions.Context.Trace;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Abstractions.Context
{

    public interface ITracerContext
    {
        void Inject(IContextCarrier carrier);

        void Extract(IContextCarrier carrier);

        IContextSnapshot Capture { get; }

        ISpan ActiveSpan { get; }

        IDictionary<string, object> Properties { get; }

        void Continued(IContextSnapshot snapshot);

        string GetReadableGlobalTraceId();

        ISpan CreateEntrySpan(string operationName);

        ISpan CreateLocalSpan(string operationName);

        ISpan CreateExitSpan(string operationName, string remotePeer);

        void StopSpan(ISpan span);
    }
}
