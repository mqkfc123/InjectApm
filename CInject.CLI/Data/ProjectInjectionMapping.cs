using System;
using System.Collections.Generic;
using System.Text;
using CInject.Engine.Extensions;

namespace CInject.CLI.Data
{
    public class ProjectInjectionMapping
    {
        public string TargetAssemblyPath { get; set; }
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public int MethodParameters { get; set; }

        public string InjectorAssemblyPath { get; set; }
        public string InjectorType { get; set; }

        public ProjectInjectionMapping()
        {

        }

        internal static ProjectInjectionMapping FromInjectionMapping(InjectionMapping mapping)
        {
            ProjectInjectionMapping projMapping = new ProjectInjectionMapping();

            projMapping.ClassName = mapping.Method.DeclaringType.Name;
            projMapping.TargetAssemblyPath = mapping.Assembly.Path;
            projMapping.MethodName = mapping.Method.Name;
            projMapping.MethodParameters = mapping.Method.Parameters.Count;

            projMapping.InjectorAssemblyPath = ReflectionExtensions.GetPath(mapping.Injector.Assembly);
            projMapping.InjectorType = mapping.Injector.AssemblyQualifiedName;
            
            return projMapping;
        }
    }
}
