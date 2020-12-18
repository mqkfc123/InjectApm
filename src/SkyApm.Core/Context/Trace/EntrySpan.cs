using SkyApm.Abstractions.Components;
using SkyApm.Abstractions.Context.Trace;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Core.Context.Trace
{

    public class EntrySpan : StackBasedTracingSpan
    {
        private int _currentMaxDepth;

        public EntrySpan(int spanId, int parentSpanId, string operationName)
            : base(spanId, parentSpanId, operationName)
        {
            _stackDepth = 0;
        }

        public EntrySpan(int spanId, int parentSpanId, int operationId)
            : base(spanId, parentSpanId, operationId)
        {
            _stackDepth = 0;
        }

        public override bool IsEntry => true;

        public override bool IsExit => false;

        public override ISpan Start()
        {
            if ((_currentMaxDepth = ++_stackDepth) == 1)
            {
                base.Start();
            }
            ClearWhenRestart();
            return this;
        }

        public override ISpan Tag(string key, string value)
        {
            if (_stackDepth == _currentMaxDepth)
            {
                base.Tag(key, value);
            }
            return this;
        }

        public override ISpan SetLayer(SpanLayer layer)
        {
            if (_stackDepth == _currentMaxDepth)
            {
                return base.SetLayer(layer);
            }
            return this;
        }

        public override ISpan SetComponent(IComponent component)
        {
            if (_stackDepth == _currentMaxDepth)
            {
                return base.SetComponent(component);
            }
            return this;
        }

        public override ISpan SetComponent(string componentName)
        {
            if (_stackDepth == _currentMaxDepth)
            {
                return base.SetComponent(componentName);
            }
            return this;
        }

        public override string OperationName
        {
            get
            {
                return base.OperationName;
            }
            set
            {
                if (_stackDepth == _currentMaxDepth)
                {
                    base.OperationName = value;
                }
            }
        }

        public override int OperationId
        {
            get
            {
                return base.OperationId;
            }
            set
            {
                if (_stackDepth == _currentMaxDepth)
                {
                    base.OperationId = value;
                }
            }
        }

        private void ClearWhenRestart()
        {
            _componentId = 0;
            _componentName = null;
            _layer = null;
            _logs = null;
            _tags = null;
        }
    }
}
