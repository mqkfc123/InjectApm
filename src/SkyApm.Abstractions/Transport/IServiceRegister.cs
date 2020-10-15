using SkyApm.Abstractions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SkyApm.Abstractions.Transport
{
    public interface IServiceRegister
    {
        Task<NullableValue> RegisterServiceAsync(ServiceRequest serviceRequest,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<NullableValue> RegisterServiceInstanceAsync(ServiceInstanceRequest serviceInstanceRequest,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}
