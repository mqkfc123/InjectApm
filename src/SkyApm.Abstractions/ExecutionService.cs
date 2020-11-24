using SkyApm.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SkyApm.Abstractions
{

    public abstract class ExecutionService : IExecutionService, IDisposable
    {
        private Timer _timer;

        protected readonly ILogger Logger;
        protected readonly IRuntimeEnvironment RuntimeEnvironment;

        protected ExecutionService(IRuntimeEnvironment runtimeEnvironment, ILoggerFactory loggerFactory)
        {
            RuntimeEnvironment = runtimeEnvironment;
            Logger = loggerFactory.CreateLogger(NLog.LogManager.GetCurrentClassLogger());
        }

        public void StartAsync()
        {
            //source
            _timer = new Timer(Callback, null, DueTime, Period);
            Logger.Information($"Loaded instrument service [{GetType().FullName}].");

        }

        public void StopAsync()
        {
            //await Stopping(cancellationToken);
            Logger.Information($"Stopped instrument service {GetType().Name}.");
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        private void Callback(object state)
        {
            try
            {
                ExecuteAsync();
            }
            catch (Exception ex)
            {
                Logger.Error(GetType().FullName + ".ExecuteAsync(token.Token) fail", ex);
            }
        }

        protected virtual bool CanExecute() => RuntimeEnvironment.Initialized;

        protected virtual void Stopping() { }

        protected abstract TimeSpan DueTime { get; }

        protected abstract TimeSpan Period { get; }

        protected abstract void ExecuteAsync();
    }
}
