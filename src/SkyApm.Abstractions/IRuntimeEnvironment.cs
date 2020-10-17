using SkyApm.Abstractions.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Abstractions
{
    public interface IRuntimeEnvironment
    {
        NullableValue ServiceId { get; }

        NullableValue ServiceInstanceId { get; }

        bool Initialized { get; }

        Guid InstanceId { get; }

        IEnvironmentProvider Environment { get; }
    }
}
