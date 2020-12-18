using SkyApm.Abstractions.Context.Tag;
using SkyApm.Abstractions.Context.Trace;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Core.Context.Tag
{
    public class StringTag : AbstractTag<string>
    {
        public StringTag(string tagKey) : base(tagKey)
        {
        }

        public override void Set(ISpan span, string tagValue)
        {
            span.Tag(Key, tagValue);
        }
    }
}
