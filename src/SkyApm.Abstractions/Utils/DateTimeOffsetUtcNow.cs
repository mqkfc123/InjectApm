using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Abstractions.Utils
{
    public class DateTimeOffsetUtcNow
    {
        public static long ToUnixTimeMilliseconds()
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var t3 = Convert.ToInt64((DateTime.Now - epoch).TotalMilliseconds);
            return t3;
        }

    }
}
