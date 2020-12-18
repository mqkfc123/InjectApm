using SkyApm.Abstractions.Context.Trace;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Abstractions.Context.Tag
{

    public abstract class AbstractTag<T>
    {

        public AbstractTag(string tagKey)
        {
            Key = tagKey;
        }

        public abstract void Set(ISpan span, T tagValue);

        public string Key { get; protected set; }
    }
}
