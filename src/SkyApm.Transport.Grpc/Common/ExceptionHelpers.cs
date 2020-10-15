﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyApm.Transport.Grpc.Common
{
    internal static class ExceptionHelpers
    {
        public static readonly string RegisterApplicationError = "Register application fail.";
        public static readonly string RegisterApplicationInstanceError = "Register application instance fail.";
        public static readonly string HeartbeatError = "Heartbeat fail.";
        public static readonly string CollectError = "Send trace segment fail.";

        public static readonly string RegisterServiceError = "Register service fail.";
        public static readonly string RegisterServiceInstanceError = "Register service instance fail.";
        public static readonly string PingError = "Ping server fail.";
    }
}
