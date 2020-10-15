using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyApm.Abstractions.Config
{
    public class ConfigAttribute : Attribute
    {
        public string[] Sections { get; }

        public ConfigAttribute(params string[] sections)
        {
            Sections = sections;
        }
    }

}
