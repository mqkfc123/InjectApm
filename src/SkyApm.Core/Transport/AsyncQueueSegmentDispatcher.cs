using SkyApm.Abstractions;
using SkyApm.Abstractions.Config;
using SkyApm.Abstractions.Tracing.Segments;
using SkyApm.Abstractions.Transport;
using SkyApm.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SkyApm.Core.Transport
{

    public class AsyncQueueSegmentDispatcher : ISegmentDispatcher
    {
        private readonly ILogger _logger;
        private readonly TransportConfig _config;
        private readonly ISegmentReporter _segmentReporter;
        private readonly ISegmentContextMapper _segmentContextMapper;
        private readonly ConcurrentQueue<SegmentRequest> _segmentQueue;
        private readonly IRuntimeEnvironment _runtimeEnvironment;
        private readonly CancellationTokenSource _cancellation;
        private int _offset;

        public AsyncQueueSegmentDispatcher(IConfigAccessor configAccessor,
            ISegmentReporter segmentReporter, IRuntimeEnvironment runtimeEnvironment,
            ISegmentContextMapper segmentContextMapper, ILoggerFactory loggerFactory)
        {
            _segmentReporter = segmentReporter;
            _segmentContextMapper = segmentContextMapper;
            _runtimeEnvironment = runtimeEnvironment;
            _logger = loggerFactory.CreateLogger(typeof(AsyncQueueSegmentDispatcher));
            _config = configAccessor.Get<TransportConfig>();
            _segmentQueue = new ConcurrentQueue<SegmentRequest>();
            _cancellation = new CancellationTokenSource();
        }

        public bool Dispatch(SegmentContext segmentContext)
        {
            if (!_runtimeEnvironment.Initialized || segmentContext == null || !segmentContext.Sampled)
                return false;

            // todo performance optimization for ConcurrentQueue
            if (_config.QueueSize < _offset || _cancellation.IsCancellationRequested)
                return false;

            var segment = _segmentContextMapper.Map(segmentContext);

            if (segment == null)
                return false;

            _segmentQueue.Enqueue(segment);

            Interlocked.Increment(ref _offset);

            _logger.Debug($"Dispatch trace segment. [SegmentId]={segmentContext.SegmentId}.");
            return true;
        }

        public Task Flush(CancellationToken token = default(CancellationToken))
        {
            // todo performance optimization for ConcurrentQueue
            //var queued = _segmentQueue.Count;
            //var limit = queued <= _config.PendingSegmentLimit ? queued : _config.PendingSegmentLimit;
            var limit = _config.BatchSize;
            var index = 0;
            var segments = new List<SegmentRequest>(limit);
            SegmentRequest request = null;

            while (index++ < limit && _segmentQueue.TryDequeue(out request))
            {
                segments.Add(request);
                Interlocked.Decrement(ref _offset);
            }

            // send async
            if (segments.Count > 0)
                _segmentReporter.ReportAsync(segments, token);

            Interlocked.Exchange(ref _offset, _segmentQueue.Count);

            return Task.CompletedTask;
        }

        public void Close()
        {
            _cancellation.Cancel();
        }
    }
}
