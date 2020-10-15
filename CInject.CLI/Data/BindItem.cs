using CInject.Engine.Resolvers;
using Mono.Cecil;

namespace CInject.CLI.Data
{
    internal class BindItem
    {
        public MethodDefinition Method { get; set; }
        public MonoAssemblyResolver Assembly { get; set; }
    }
}