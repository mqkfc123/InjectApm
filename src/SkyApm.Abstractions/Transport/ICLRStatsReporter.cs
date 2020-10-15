using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SkyApm.Abstractions.Transport
{
    public interface ICLRStatsReporter
    {
        Task ReportAsync(CLRStatsRequest statsRequest,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}
