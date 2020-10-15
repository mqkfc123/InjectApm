
using System;
using System.Collections.Generic;
using System.Reflection;
using CInject.Engine.Extensions;
using System.IO;

namespace CInject.Engine.Resolvers
{
    public class ReflectionAssemblyResolver : BaseAssemblyResolver
    {
        private Assembly _assembly;

        public ReflectionAssemblyResolver(string path)
            : base(path)
        {
            _assembly = Assembly.LoadFrom(path);
        }

        public Assembly Assembly
        {
            get { return _assembly; }
            private set { _assembly = value; }
        }

        public List<Type> FindTypes<T1>()
        {
            return _assembly.GetType<T1>();
        }
    }
}