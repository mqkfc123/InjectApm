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

            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var t3 = Convert.ToInt64((DateTime.Now - epoch).TotalMilliseconds);

            return new UniqueId(_runtimeEnvironment.ServiceInstanceId.Value,
                Thread.CurrentThread.ManagedThreadId,
                t3 * 10000 + GetSequence());
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
