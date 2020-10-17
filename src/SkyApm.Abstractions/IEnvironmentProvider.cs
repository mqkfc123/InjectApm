using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Abstractions
{
    public interface IEnvironmentProvider
    {
        string EnvironmentName { get; }
    }
}
