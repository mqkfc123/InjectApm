using SkyApm.Abstractions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
