
using System.Diagnostics;
using System.Reflection;

namespace CInject.Injections.Models
{
    public class CallInfo
    {
        public MethodBase CallingMethod { get; set; }
        public MethodBase CurrentMethod { get; set; }
        public StackFrame Stack { get; set; }
    }
}