using CInject.CLI.Data;
using CInject.Engine.Extensions;
using CInject.Engine.Resolvers;
using CInject.Injections;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CInject.CLI
{
    class Program
    {
        //设置dll中的方法
        private static List<InjectionMapping> _mapping = new List<InjectionMapping>();

        //cinject 的执行方法
        private static Dictionary<string, Type> _injectTypeDict = new Dictionary<string, Type>();

        private static List<BindItem> _bindItem = new List<BindItem>();

        //设置被注入的目标函数集合
        public static List<string> _methodTargetItem = new List<string>();

        static void Main(string[] args)
        {
            Console.WriteLine("Loading MethodTarget =========");
            LoadingMethodTarget();

            Console.WriteLine("Loading Injection =========");
            LoadInjection();

            Console.WriteLine("Loading Target =========");
            LoadTarget();

            foreach (var item in _mapping)
            {
                item.Assembly.Inject(item.Method, item.Injector);
            }

            Console.WriteLine("SaveAssemblies ===========");
            SaveAssemblies();

            Console.ReadLine();
        }

        private static void LoadingMethodTarget()
        {
            try
            {
                var path = AppDomain.CurrentDomain.BaseDirectory + "methodTarget.txt";

                if (File.Exists(path))
                {
                    var methodTargets = File.ReadAllText(path, Encoding.UTF8);

                    foreach (var methodName in methodTargets.Split(';'))
                    {
                        _methodTargetItem.Add(methodName.ToLower());
                    }
                }
                else
                {
                    Console.WriteLine($"{path}文件不存在");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void LoadInjection()
        {
            try
            {
                var directory = Path.GetDirectoryName(Environment.CurrentDirectory);

                var files = Directory.GetFiles(Environment.CurrentDirectory, "CInject.Injections.dll");
                foreach (var assemblyName in files)
                {
                    var assemblyInjectionCode = new ReflectionAssemblyResolver(assemblyName);
                    List<Type> injectTypes = assemblyInjectionCode.FindTypes<ICInject>();

                    //ICInject 的实现类
                    foreach (var injectType in injectTypes)
                    {
                        _injectTypeDict.Add(injectType.Name, injectType);
                    }

                    //Startup 
                    foreach (Type t in assemblyInjectionCode.Assembly.GetTypes())
                    {
                        if (t.Name.ToString() == "Startup")
                        {
                            _injectTypeDict.Add(t.Name.ToString(), t);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void LoadTarget()
        {
            try
            {
                var path = AppDomain.CurrentDomain.BaseDirectory + "descriptors.txt";

                if (File.Exists(path))
                {
                    var descriptors = File.ReadAllText(path, Encoding.UTF8);

                    foreach (var assemblyName in descriptors.Split(';'))
                    {
                        var files = Directory.GetFiles(Environment.CurrentDirectory, assemblyName);

                        if (files.Length <= 0)
                            continue;

                        //assembly
                        var assemblyTarget = new MonoAssemblyResolver(assemblyName);

                        var text = Path.GetFileName(assemblyName);
                        var tag = new BindItem { Assembly = assemblyTarget, Method = null };

                        //class
                        List<TypeDefinition> types = assemblyTarget.FindClasses();

                        for (int i = 0; i < types.Count; i++)
                        {
                            if (types[i].HasMethods)
                            {
                                //method 
                                var methodDefinitions = MonoExtensions.GetMethods(types[i], false);

                                for (int j = 0; j < methodDefinitions.Count; j++)
                                {

                                    //var var1 = types[i].FullName + "." + methodDefinitions[j].Name;

                                    //if (_methodTargetItem.Count > 0 && !_methodTargetItem.Contains(var1.ToLower()))
                                    //{
                                    //    continue;
                                    //}
                                    Type type = null;
                                    if (types[i].Name == "Program" && methodDefinitions[j].Name == "Main")
                                    {
                                        type = _injectTypeDict["Startup"];
                                    }
                                    else
                                    {
                                        type = _injectTypeDict["ObjectValueInject"];
                                    }

                                    _mapping.Add(new InjectionMapping(assemblyTarget, methodDefinitions[j], type));

                                    //var var1 = types[i].FullName + "." + methodDefinitions[j].Name;
                                    //if (methodDefinitions[j].Name.ToLower().Contains("_click"))
                                    //{
                                    //    if (methodDefinitions[j].Parameters.Count >= 2 &&
                                    //        methodDefinitions[j].Parameters[0].ParameterType.Name == "Object" &&
                                    //        methodDefinitions[j].Parameters[1].ParameterType.Name == "EventArgs")
                                    //    {
                                    //        Type type = _injectTypeDict["ObjectValueInject"];
                                    //        _mapping.Add(new InjectionMapping(assemblyTarget, methodDefinitions[j], type));
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    if (_methodTargetItem.Count > 0 && !_methodTargetItem.Contains(var1.ToLower()))
                                    //    {
                                    //        continue;
                                    //    }

                                    //    Type type = null;
                                    //    if (types[i].Name == "Program" && methodDefinitions[j].Name == "Main")
                                    //    {
                                    //        type = _injectTypeDict["Startup"];
                                    //    }
                                    //    else
                                    //    {
                                    //        type = _injectTypeDict["ObjectValueInject"];
                                    //    }

                                    //    _mapping.Add(new InjectionMapping(assemblyTarget, methodDefinitions[j], type));

                                    //}

                                }
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"{path}文件不存在");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private static void SaveAssemblies()
        {
            try
            {
                List<MonoAssemblyResolver> assemblies = new List<MonoAssemblyResolver>();
                foreach (var x in _mapping)
                {
                    if (!assemblies.Contains(x.Assembly))
                        assemblies.Add(x.Assembly);
                }

                //IEnumerable<MonoAssemblyResolver> assemblies = _mapping.Select(x => x.Assembly).Distinct();

                foreach (MonoAssemblyResolver assembly in assemblies)
                    assembly.Save();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while saving assemblies:{ex.Message}");
            }
        }



    }



}
