using SkyApm.Abstractions.Common;
using SkyApm.Abstractions.Context;
using SkyApm.Abstractions.Context.Ids;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Core.Context
{

    public class ContextSnapshot : IContextSnapshot
    {
        /// <summary>
        /// Trace Segment Id of the parent trace segment
        /// </summary>
        private readonly ID _traceSegmentId;

        /// <summary>
        /// span id of the parent span , in parent trace segment
        /// </summary>
        private readonly int _spanId = -1;

        private string _entryOperationName;
        private string _parentOperationName;

        private readonly DistributedTraceId _primaryDistributedTraceId;
        private NullableValue _entryApplicationInstanceId = NullableValue.Null;

        public ContextSnapshot(ID traceSegmentId, int spanId, List<DistributedTraceId> distributedTraceIds)
        {
            _traceSegmentId = traceSegmentId;
            _spanId = spanId;
            _primaryDistributedTraceId = distributedTraceIds.Count > 0 ? distributedTraceIds.ToArray()[0] : null;
        }

        public string EntryOperationName
        {
            get { return _entryOperationName; }
            set { _entryOperationName = "#" + value; }
        }

        public string ParentOperationName
        {
            get { return _parentOperationName; }
            set
            {
                _parentOperationName = "#" + value;
            }
        }
        public DistributedTraceId DistributedTraceId => _primaryDistributedTraceId;

        public int EntryApplicationInstanceId
        {
            get { return _entryApplicationInstanceId.Value; }
            set
            {
                _entryApplicationInstanceId = new NullableValue(value);
            }
        }

        public int SpanId => _spanId;

        public bool IsFromCurrent => _traceSegmentId.Equals(ContextManager.Capture.TraceSegmentId);

        public bool IsValid => _traceSegmentId != null
                               && _spanId > -1
                               && _entryApplicationInstanceId.HasValue
                               && _primaryDistributedTraceId != null
                               && string.IsNullOrEmpty(_entryOperationName)
                               && string.IsNullOrEmpty(_parentOperationName);

        public ID TraceSegmentId => _traceSegmentId;

        public int EntryOperationId
        {
            set
            {
                _entryOperationName = value + "";
            }
        }

        public int ParentOperationId
        {
            set
            {
                _parentOperationName = value + "";
            }
        }
    }
}
