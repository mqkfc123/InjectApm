using SkyApm.Abstractions.Context.Ids;
using SkyApm.Abstractions.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SkyApm.Core.Context.Ids
{

    public static class GlobalIdGenerator
    {
        private static readonly IDContext threadIdSequence =   new IDContext(DateTimeOffsetUtcNow.ToUnixTimeMilliseconds(), 0) ;

        public static ID Generate()
        {
            if (!WorkContext.RuntimeEnvironment.ServiceInstanceId.HasValue)
            {
                throw new InvalidOperationException();
            }

            IDContext context = threadIdSequence;

            return new ID(
                WorkContext.RuntimeEnvironment.ServiceInstanceId.Value,
                Thread.CurrentThread.ManagedThreadId,
                context.NextSeq()
            );
        }

        private class IDContext
        {
            private long _lastTimestamp;
            private short _threadSeq;

            // Just for considering time-shift-back only.
            private long _runRandomTimestamp;
            private int _lastRandomValue;
            private readonly Random _random;

            public IDContext(long lastTimestamp, short threadSeq)
            {
                _lastTimestamp = lastTimestamp;
                _threadSeq = threadSeq;
                _random = new Random();
            }

            public long NextSeq()
            {
                return GetTimestamp() * 10000 + NextThreadSeq();
            }

            private long GetTimestamp()
            {
                long currentTimeMillis = DateTimeOffsetUtcNow.ToUnixTimeMilliseconds();

                if (currentTimeMillis < _lastTimestamp)
                {
                    // Just for considering time-shift-back by Ops or OS. @hanahmily 's suggestion.
                    if (_runRandomTimestamp != currentTimeMillis)
                    {
                        _lastRandomValue = _random.Next();
                        _runRandomTimestamp = currentTimeMillis;
                    }

                    return _lastRandomValue;
                }
                else
                {
                    _lastTimestamp = currentTimeMillis;
                    return _lastTimestamp;
                }
            }

            private short NextThreadSeq()
            {
                if (_threadSeq == 10000)
                {
                    _threadSeq = 0;
                }

                return _threadSeq++;
            }
        }
    }
}
