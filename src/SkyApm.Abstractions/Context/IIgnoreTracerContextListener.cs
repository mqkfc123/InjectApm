using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Abstractions.Context
{
    public interface IIgnoreTracerContextListener
    {
        void AfterFinish(ITracerContext tracerContext);
    }
}
