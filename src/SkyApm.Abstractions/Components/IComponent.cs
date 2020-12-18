using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Abstractions.Components
{
    public interface IComponent
    {
        int Id { get; }

        string Name { get; }
    }
}
