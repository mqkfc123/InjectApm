﻿using SkyApm.Abstractions;
using SkyApm.Abstractions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyApm.Core
{
    public class RuntimeEnvironment : IRuntimeEnvironment
    {
        public static IRuntimeEnvironment Instance { get; } = new RuntimeEnvironment();

        public NullableValue ServiceId { get; internal set; }

        public NullableValue ServiceInstanceId { get; internal set; }

        public bool Initialized => ServiceId.HasValue && ServiceInstanceId.HasValue;

        public Guid InstanceId { get; } = Guid.NewGuid();

        public IEnvironmentProvider Environment { get; set; }
    }
}
