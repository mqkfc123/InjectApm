using System;
using Mono.Cecil;

namespace CInject.Engine.Data
{
    public class Injection
    {
        public MethodReference Constructor { get; set; }

        public Type InjectionType { get; set; }

        public MethodReference OnInvoke { get; set; }

        public TypeReference TypeReference { get; set; }

        public MethodReference OnComplete { get; set; }
    }
}