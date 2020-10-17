using SkyApm.Abstractions.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SkyApm.Abstractions.Transport
{
    public interface IServiceRegister
    {
        NullableValue RegisterServiceAsync(ServiceRequest serviceRequest);

        NullableValue RegisterServiceInstanceAsync(ServiceInstanceRequest serviceInstanceRequest);
    }
}
