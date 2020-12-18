using SkyApm.Abstractions.Common;
using SkyApm.Abstractions.Context;
using SkyApm.Abstractions.Context.Ids;
using SkyApm.Abstractions.Context.Trace;
using SkyApm.Abstractions.Tracing.Segments;
using SkyApm.Abstractions.Transport;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Core.Context.Trace
{

    public class TraceSegmentRef : ITraceSegmentRef
    {
        private readonly Reference _type;
        private readonly ID _traceSegmentId;
        private readonly int _spanId = -1;
        private readonly int _peerId = 0;
        private readonly string _peerHost;
        private readonly int _entryApplicationInstanceId = 0;
        private readonly int _parentApplicationInstanceId = 0;
        private readonly string _entryOperationName;
        private readonly int _entryOperationId = 0;
        private readonly string _parentOperationName;
        private readonly int _parentOperationId = 0;

        public TraceSegmentRef(IContextCarrier carrier)
        {
            _type = Reference.CrossProcess;
            _traceSegmentId = carrier.TraceSegmentId;
            _spanId = carrier.SpanId;
            _parentApplicationInstanceId = carrier.ParentApplicationInstanceId;
            _entryApplicationInstanceId = carrier.EntryApplicationInstanceId;
            string host = carrier.PeerHost;
            if (host.ToCharArray()[0] == '#')
            {
                _peerHost = host.Substring(1);
            }
            else
            {
                int.TryParse(host, out _peerId);
            }

            string entryOperationName = carrier.EntryOperationName;
            if (entryOperationName.Contains("#"))
            {
                _entryOperationName = entryOperationName.Substring(1);
            }
            else
            {
                int.TryParse(entryOperationName, out _entryOperationId);
            }

            string parentOperationName = carrier.EntryOperationName;
            if (parentOperationName.Contains("#"))
            {
                _parentOperationName = parentOperationName.Substring(1);
            }
            else
            {
                int.TryParse(parentOperationName, out _parentOperationId);
            }
        }

        public TraceSegmentRef(IContextSnapshot contextSnapshot)
        {
            _type = Reference.CrossThread;
            _traceSegmentId = contextSnapshot.TraceSegmentId;
            _spanId = contextSnapshot.SpanId;
            _parentApplicationInstanceId = RuntimeEnvironment.Instance.ServiceInstanceId.Value;
            _entryApplicationInstanceId = contextSnapshot.EntryApplicationInstanceId;
            string entryOperationName = contextSnapshot.EntryOperationName;
            if (entryOperationName.Contains("#"))
            {
                _entryOperationName = entryOperationName.Substring(1);
            }
            else
            {
                int.TryParse(entryOperationName, out _entryOperationId);
            }

            string parentOperationName = contextSnapshot.ParentOperationName;
            if (parentOperationName.Contains("#"))
            {
                _parentOperationName = parentOperationName.Substring(1);
            }
            else
            {
                int.TryParse(parentOperationName, out _parentOperationId);
            }
        }

        public bool Equals(ITraceSegmentRef other)
        {
            TraceSegmentRef segmentRef;
            if (other == null)
            {
                return false;
            }

            if (other == this)
            {
                return true;
            }

            if (!(other is TraceSegmentRef))
            {
                return false;
            }
            segmentRef = (TraceSegmentRef)other;
            if (_spanId != segmentRef._spanId)
            {
                return false;
            }

            return _traceSegmentId.Equals(segmentRef._traceSegmentId);
        }

        public override bool Equals(object obj)
        {
            var other = obj as ITraceSegmentRef;
            return Equals(other);
        }

        public override int GetHashCode()
        {
            int result = _traceSegmentId.GetHashCode();
            result = 31 * result + _spanId;
            return result;
        }

        public string EntryOperationName => _entryOperationName;

        public int EntryOperationId => _entryOperationId;

        public int EntryApplicationInstanceId => _entryApplicationInstanceId;

        public TraceSegmentReferenceRequest Transform()
        {
            TraceSegmentReferenceRequest traceSegmentReference = new TraceSegmentReferenceRequest();
            if (_type == Reference.CrossProcess)
            {
                traceSegmentReference.RefType = (int)Reference.CrossProcess;
                traceSegmentReference.NetworkAddress = new StringOrIntValue(_peerId, _peerHost);
            }
            else
            {
                traceSegmentReference.RefType = (int)Reference.CrossThread;
                traceSegmentReference.NetworkAddress = new StringOrIntValue();
            }

            traceSegmentReference.ParentApplicationInstanceId = _parentApplicationInstanceId;
            traceSegmentReference.EntryApplicationInstanceId = _entryApplicationInstanceId;
            traceSegmentReference.ParentTraceSegmentId = _traceSegmentId.Transform();
            traceSegmentReference.ParentSpanId = _spanId;

            traceSegmentReference.EntryServiceName = new StringOrIntValue(_entryOperationId, _entryOperationName);

            traceSegmentReference.ParentServiceName = new StringOrIntValue(_parentOperationId, _parentOperationName);

            return traceSegmentReference;
        }
    }
}
