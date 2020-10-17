using SkyApm.Abstractions;
using SkyApm.Abstractions.Tracing;
using System;
using System.Threading;

namespace SkyApm.Core.Tracing
{

    public class UniqueIdGenerator : IUniqueIdGenerator
    {
        private static long  sequence =0;
        private readonly IRuntimeEnvironment _runtimeEnvironment;

        public UniqueIdGenerator(IRuntimeEnvironment runtimeEnvironment)
        {
            _runtimeEnvironment = runtimeEnvironment;
        }

        public UniqueId Generate()
        {
            //ToUnixTimeMilliseconds
            return new UniqueId(_runtimeEnvironment.ServiceInstanceId.Value,
                Thread.CurrentThread.ManagedThreadId,
                DateTimeOffset.UtcNow.UtcTicks * 10000 + GetSequence());
        }

        private long GetSequence()
        {
            if (sequence++ >= 9999)
            {
                sequence = 0;
            }

            return sequence;
        }
    }
}
