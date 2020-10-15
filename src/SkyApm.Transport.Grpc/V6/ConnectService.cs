using SkyApm.Abstractions;
using SkyApm.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SkyApm.Transport.Grpc.V6
{

    public class ConnectService : ExecutionService
    {
        private readonly ConnectionManager _connectionManager;

        public ConnectService(ConnectionManager connectionManager,
            IRuntimeEnvironment runtimeEnvironment,
            ILoggerFactory loggerFactory) : base(runtimeEnvironment, loggerFactory)
        {
            _connectionManager = connectionManager;
        }

        protected override TimeSpan DueTime { get; } = TimeSpan.Zero;
        protected override TimeSpan Period { get; } = TimeSpan.FromSeconds(15);

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            if (!_connectionManager.Ready)
            {
                await _connectionManager.ConnectAsync();
            }
        }

        protected override bool CanExecute() => !_connectionManager.Ready;
    }
}
