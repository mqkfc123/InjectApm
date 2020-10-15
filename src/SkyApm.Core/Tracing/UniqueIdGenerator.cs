using SkyApm.Abstractions;
using SkyApm.Abstractions.Tracing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SkyApm.Core.Tracing
{

    public class UniqueIdGenerator : IUniqueIdGenerator
    {
        private readonly ThreadLocal<long> sequence = new ThreadLocal<long>(() => 0);
        private readonly IRuntimeEnvironment _runtimeEnvironment;

        public UniqueIdGenerator(IRuntimeEnvironment runtimeEnvironment)
        {
            _runtimeEnvironment = runtimeEnvironment;
        }

        public UniqueId Generate()
        {
            return new UniqueId(_runtimeEnvironment.ServiceInstanceId.Value,
                Thread.CurrentThread.ManagedThreadId,
                DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() * 10000 + GetSequence());
        }

        private long GetSequence()
        {
            if (sequence.Value++ >= 9999)
            {
                sequence.Value = 0;
            }

            return sequence.Value;
        }
    }
}
