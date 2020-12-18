using SkyApm.Abstractions.Common;
using SkyApm.Abstractions.Components;
using SkyApm.Abstractions.Context.Trace;
using SkyApm.Abstractions.Transport;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Core.Context.Trace
{

    public class ExitSpan : StackBasedTracingSpan, IWithPeerInfo
    {
        private readonly string _peer;
        private readonly int _peerId;

        public ExitSpan(int spanId, int parentSpanId, String operationName, String peer)
            : base(spanId, parentSpanId, operationName)
        {
            _peer = peer;
            _peerId = 0;
        }

        public ExitSpan(int spanId, int parentSpanId, int operationId, int peerId)
            : base(spanId, parentSpanId, operationId)
        {
            _peer = null;
            _peerId = peerId;
        }

        public ExitSpan(int spanId, int parentSpanId, int operationId, String peer)
            : base(spanId, parentSpanId, operationId)
        {
            _peer = peer;
            _peerId = 0;
        }

        public ExitSpan(int spanId, int parentSpanId, String operationName, int peerId)
            : base(spanId, parentSpanId, operationName)
        {
            _peer = null;
            _peerId = peerId;
        }

        public override bool IsEntry => false;

        public override bool IsExit => true;

        public int PeerId => _peerId;

        public string Peer => _peer;

        public override ISpan Start()
        {
            if (++_stackDepth == 1)
            {
                base.Start();
            }

            return base.Start();
        }

        public override ISpan Tag(string key, string value)
        {
            if (_stackDepth == 1)
            {
                base.Tag(key, value);
            }

            return this;
        }

        public override ISpan SetLayer(SpanLayer layer)
        {
            if (_stackDepth == 1)
            {
                return base.SetLayer(layer);
            }

            return this;
        }

        public override ISpan SetComponent(IComponent component)
        {
            if (_stackDepth == 1)
            {
                return base.SetComponent(component);
            }

            return this;
        }

        public override ISpan SetComponent(string componentName)
        {
            return _stackDepth == 1 ? base.SetComponent(componentName) : this;
        }

        public override string OperationName
        {
            get { return base.OperationName; }
            set
            {
                if (_stackDepth == 1)
                {
                    base.OperationName = value;
                }
            }
            }

        public override int OperationId
        {
            get { return base.OperationId; }
            set
            {
                if (_stackDepth == 1)
                {
                    base.OperationId = value;
                }
            }
        }

        public override SpanRequest Transform()
        {
            var spanObject = base.Transform();

            spanObject.Peer = new StringOrIntValue(_peerId, _peer);

            return spanObject;
        }
    }
}
