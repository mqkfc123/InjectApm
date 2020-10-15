using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SkyApm.Transport.Grpc.Common
{

    internal class AsyncLock
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);
        private readonly Release _release;
        private readonly Task<Release> _releaseTask;

        public AsyncLock()
        {
            _release = new Release(this);
            _releaseTask = Task.FromResult(_release);
        }

        public Task<Release> LockAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var wait = _semaphore.WaitAsync(cancellationToken);

            return wait.IsCompleted
                ? _releaseTask
                : wait.ContinueWith(
                    (_, state) => ((AsyncLock)state)._release,
                    this, CancellationToken.None,
                    TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
        }

        public Release Lock()
        {
            _semaphore.Wait();

            return _release;
        }

        public struct Release : IDisposable
        {
            private readonly AsyncLock _toRelease;

            internal Release(AsyncLock toRelease)
            {
                _toRelease = toRelease;
            }

            public void Dispose()
                => _toRelease._semaphore.Release();
        }
    }
}
