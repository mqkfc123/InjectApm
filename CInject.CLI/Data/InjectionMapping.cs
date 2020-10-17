using System;
using CInject.Engine.Resolvers;
using Mono.Cecil;
using CInject.Engine.Extensions;
using CInject.Engine.Utils;

namespace CInject.CLI.Data
{
    [Serializable]
    internal sealed class InjectionMapping
    {
        public InjectionMapping(MonoAssemblyResolver assembly,
                                MethodDefinition method, Type injector)
        {
            Assembly = assembly;
            Method = method;
            Injector = injector;

            MethodName = method.Name;
            InjectorName = injector.FullName;
            AssemblyName = assembly.Assembly.FullName;
        }

        public MonoAssemblyResolver Assembly { get; private set; }
        public MethodDefinition Method { get; private set; }
        public Type Injector { get; private set; }

        public string MethodName { get; private set; }
        public string AssemblyName { get; private set; }
        public string InjectorName { get; private set; }

        public override int GetHashCode()
        {
            return Method.GetHashCode() + Assembly.GetHashCode() + Injector.GetHashCode();
        }

        internal static InjectionMapping FromProjectInjectionMapping(ProjectInjectionMapping projMapping)
        {
            MonoAssemblyResolver targetAssembly = null;
            TypeDefinition type = null;
            MethodDefinition method = null;
            Type injector = null;
            if (CacheStore.Exists<MonoAssemblyResolver>(projMapping.TargetAssemblyPath))
            {
                targetAssembly = CacheStore.Get<MonoAssemblyResolver>(projMapping.TargetAssemblyPath);
            }
            else
            {
                targetAssembly = new MonoAssemblyResolver(projMapping.TargetAssemblyPath);
                CacheStore.Add<MonoAssemblyResolver>(projMapping.TargetAssemblyPath, targetAssembly);
            }

            string classNameKey = targetAssembly.Assembly.Name.Name + "." + projMapping.ClassName;

            if (CacheStore.Exists<TypeDefinition>(classNameKey))
            {
                type = CacheStore.Get<TypeDefinition>(classNameKey);
            }
            else
            {
                type = targetAssembly.Assembly.MainModule.GetType(classNameKey);
                CacheStore.Add<TypeDefinition>(classNameKey, type);
            }

            if (CacheStore.Exists<MethodDefinition>(classNameKey + projMapping.MethodName))
            {
                method = CacheStore.Get<MethodDefinition>(classNameKey + projMapping.MethodName);
            }
            else
            {
                method = MonoExtensions.GetMethodDefinition(type,projMapping.MethodName, projMapping.MethodParameters);
                CacheStore.Add<MethodDefinition>(classNameKey + projMapping.MethodName, method);
            }

            if (CacheStore.Exists<Type>(projMapping.InjectorType))
            {
                injector = CacheStore.Get<Type>(projMapping.InjectorType);
            }
            else
            {
                injector = Type.GetType(projMapping.InjectorType);
                CacheStore.Add<Type>(projMapping.InjectorType, injector);
            }

            return new InjectionMapping(targetAssembly, method, injector);
        }
    }
}