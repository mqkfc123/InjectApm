using System.Diagnostics;
using System.Reflection;

namespace CInject.Injections.Library
{
    internal class ReflectionHelper
    {
        internal static string GetMethodName()
        {
            var stackTrace = new StackTrace();

            if (stackTrace.FrameCount > 2)
                return stackTrace.GetFrame(2).GetMethod().Name;
            else
                return stackTrace.GetFrame(1).GetMethod().Name;
        }

        internal static MethodBase GetCallInformation()
        {
            var stackTrace = new StackTrace();

            if (stackTrace.FrameCount > 2)
                return stackTrace.GetFrame(2).GetMethod();
            else
                return stackTrace.GetFrame(1).GetMethod();
        }
    }
}