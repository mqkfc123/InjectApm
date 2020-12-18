using SkyApm.Abstractions.Context.Trace;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Abstractions.Context.Utils
{

    public static class SpanLayerExtensions
    {
        public static void AsDB(this ISpan span)
        {
            span.SetLayer(SpanLayer.DB);
        }

        public static void AsCache(this ISpan span)
        {
            span.SetLayer(SpanLayer.CACHE);
        }

        public static void AsRPCFramework(this ISpan span)
        {
            span.SetLayer(SpanLayer.RPC_FRAMEWORK);
        }

        public static void AsHttp(this ISpan span)
        {
            span.SetLayer(SpanLayer.HTTP);
        }

        public static void AsMQ(this ISpan span)
        {
            span.SetLayer(SpanLayer.MQ);
        }
    }
}
