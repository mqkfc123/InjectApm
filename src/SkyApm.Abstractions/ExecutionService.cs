using SkyApm.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SkyApm.Abstractions
{

    public abstract class ExecutionService : IExecutionService, IDisposable
    {
        private Timer _timer;
        private CancellationTokenSource _cancellationTokenSource;

        protected readonly ILogger Logger;
        protected readonly IRuntimeEnvironment RuntimeEnvironment;

        protected ExecutionService(IRuntimeEnvironment runtimeEnvironment, ILoggerFactory loggerFactory)
        {
            RuntimeEnvironment = runtimeEnvironment;
            Logger = loggerFactory.CreateLogger(GetType());
        }

        public Task StartAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            _cancellationTokenSource = new CancellationTokenSource();
            var source = CancellationTokenSource.CreateLinkedTokenSource(_cancellationTokenSource.Token, cancellationToken);
            _timer = new Timer(Callback, source, DueTime, Period);
            Logger.Information($"Loaded instrument service [{GetType().FullName}].");
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            _cancellationTokenSource?.Cancel();
            await Stopping(cancellationToken);
            Logger.Information($"Stopped instrument service {GetType().Name}.");
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        private async void Callback(object state)
        {

            if (!(state is CancellationTokenSource))
                return;

            var token = (CancellationTokenSource)state;

            var f = !CanExecute();
            if (token.IsCancellationRequested || !CanExecute())
                return;

            try
            {
                await ExecuteAsync(token.Token);
            }
            catch (Exception ex)
            {
                Logger.Error(GetType().FullName + ".ExecuteAsync(token.Token) fail", ex);
            }
        }

        protected virtual bool CanExecute() => RuntimeEnvironment.Initialized;

        protected virtual Task Stopping(CancellationToken cancellationToke) => Task.CompletedTask;

        protected abstract TimeSpan DueTime { get; }

        protected abstract TimeSpan Period { get; }

        protected abstract Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
