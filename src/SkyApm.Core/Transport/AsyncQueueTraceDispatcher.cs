using SkyApm.Abstractions;
using SkyApm.Abstractions.Config;
using SkyApm.Abstractions.Transport;
using SkyApm.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SkyApm.Core.Transport
{

    public class AsyncQueueTraceDispatcher : ITraceDispatcher
    {
        private readonly ILogger _logger;
        private readonly TransportConfig _config;
        private readonly ISkyWalkingClient _skyWalkingClient;
        private readonly Queue<TraceSegmentRequest> _segmentQueue;
        private int _offset;

        public AsyncQueueTraceDispatcher(IConfigAccessor configAccessor, ISkyWalkingClient client, ILoggerFactory loggerFactory)
        {
            _skyWalkingClient = client;
            _logger = loggerFactory.CreateLogger(NLog.LogManager.GetCurrentClassLogger());
            _config = configAccessor.Get<TransportConfig>();
            _segmentQueue = new Queue<TraceSegmentRequest>();
        }

        public bool Dispatch(TraceSegmentRequest segment)
        {
            // todo performance optimization for ConcurrentQueue
            if (_config.BatchSize < _segmentQueue.Count)
            {
                return false;
            }
            _segmentQueue.Enqueue(segment);

            _logger.Debug($"Dispatch trace segment. [SegmentId]={segment.Segment.SegmentId}.");
            return true;
        }

        public void Flush()
        {
            // todo performance optimization for ConcurrentQueue
            //var queued = _segmentQueue.Count;
            //var limit = queued <= _config.PendingSegmentLimit ? queued : _config.PendingSegmentLimit;
            var limit = _config.BatchSize;
            var index = 0;
            var segments = new List<TraceSegmentRequest>(limit);

            while (index++ < limit)
            {
                if (_segmentQueue.Count <= 0)
                    continue;

                segments.Add(_segmentQueue.Dequeue());
                Interlocked.Decrement(ref _offset);
            }
            // send async
            if (segments.Count > 0)
                _skyWalkingClient.CollectAsync(segments);
        }

        public void Close()
        {

        }

    }
}
