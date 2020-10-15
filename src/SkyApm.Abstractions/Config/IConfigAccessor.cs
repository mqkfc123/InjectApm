using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyApm.Abstractions.Config
{

    public interface IConfigAccessor
    {
        T Get<T>() where T : class, new();

        T Value<T>(string key, params string[] sections);
    }
}
