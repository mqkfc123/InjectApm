using System;
using System.Collections.Generic;
using System.Reflection;
using CInject.Engine.Data;
using System.Linq;

namespace CInject.Engine.Extensions
{
    public static class ReflectionExtensions
    {
        public static List<Type> GetType<T>(Assembly assembly)
        {
            //TODO: Optimize this...
            Type[] types = assembly.GetTypes();
            Type interfaceType = typeof(T);

            //does not work!
            //return types.Where(x => x.IsAssignableFrom(interfaceType)).ToList();

            var selected = new List<Type>();

            for (int x = 0; x < types.Length; x++)
            {
                var s1 = types[x].GetInterfaces();
                var counter = s1.Count(y => y.FullName == interfaceType.FullName);

                if (counter > 0)
                    selected.Add(types[x]);
            }

            return selected;
        }

        public static Runtime GetRuntime(Assembly assembly)
        {
            if (assembly.ImageRuntimeVersion.Contains("v1.0"))
                return Runtime.Net_1_0;
            else if (assembly.ImageRuntimeVersion.Contains("v1.1"))
                return Runtime.Net_1_1;
            else if (assembly.ImageRuntimeVersion.Contains("v2.0")
                    || assembly.ImageRuntimeVersion.Contains("v3.0")
                    || assembly.ImageRuntimeVersion.Contains("v3.5"))
                return Runtime.Net_2_0;
            else
                return Runtime.Net_4_0;
        }

        public static string GetPath(Assembly assembly)
        {
            return new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath;
        }
    }
}