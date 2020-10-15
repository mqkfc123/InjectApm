using Grpc.Core;
using SkyWalking.NetworkProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CInject.SampleWinform
{
    public class Polly
    {
        public string PollyName { get; } = "default";
        //public static async Task<NullableValue> Polling(int retry, Func<Task<NullableValue>> execute)
        //{
        //    var index = 0;
        //    while (index++ < retry)
        //    {
        //        var value = await execute();
        //        if (value.HasValue)
        //        {
        //            return value;
        //        }

        //        await Task.Delay(500);
        //    }

        //    return NullableValue.Null;
        //}

        public static async Task<int> Polling(int retry, Func<AsyncUnaryCall<ServiceInstanceRegisterMapping>> execute)
        {
            var index = 0;
            while (index++ < retry)
            {
                var value = await execute();
                foreach (var serviceInstance in value.ServiceInstances)
                {
                   return serviceInstance.Value;
                }

                await Task.Delay(500);
            }

            return 0;
        }


        

    }
}
