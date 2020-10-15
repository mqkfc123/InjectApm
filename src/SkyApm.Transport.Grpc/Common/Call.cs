using SkyApm.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyApm.Transport.Grpc.Common
{

    internal class Call
    {
        private readonly ILogger _logger;
        private readonly ConnectionManager _connectionManager;

        public Call(ILogger logger, ConnectionManager connectionManager)
        {
            _logger = logger;
            _connectionManager = connectionManager;
        }

        public async Task Execute(Func<Task> task, Func<string> errMessage)
        {
            try
            {
                await task();
            }
            catch (Exception ex)
            {
                _logger.Error(errMessage(), ex);
                _connectionManager.Failure(ex);
            }
        }

        public async Task<T> Execute<T>(Func<Task<T>> task, Func<T> errCallback, Func<string> errMessage)
        {
            try
            {
                return await task();
            }
            catch (Exception ex)
            {
                _logger.Error(errMessage(), ex);
                _connectionManager.Failure(ex);
                return errCallback();
            }
        }
    }
}
