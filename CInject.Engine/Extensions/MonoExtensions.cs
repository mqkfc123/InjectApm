using System;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using Mono.Cecil;
using CInject.Engine.Data;
using System.Reflection;

namespace CInject.Engine.Extensions
{

    public static class MonoExtensions
    {
        public static MethodReference ImportMethod<T>(AssemblyDefinition assembly, string methodName, BindingFlags bindingFlags)
        {
            return assembly.MainModule.Import(typeof(T).GetMethod(methodName, bindingFlags));
        }

        public static MethodReference ImportMethod<T>(AssemblyDefinition assembly, string methodName, Type[] types)
        {
            return assembly.MainModule.Import(typeof(T).GetMethod(methodName, types));
        }

        public static MethodReference ImportMethod(AssemblyDefinition assembly, Type type, string methodName, Type[] types)
        {
            var methods = new List<MethodInfo>();
            foreach (var method in type.GetMethods())
            {
                if (method.Name == methodName)
                    methods.Add(method);
            }

            if (methods.Count > 1)
                throw new AmbiguousMatchException("More than one method with name " + methodName + " found in " + type.Name);
            else
                return assembly.MainModule.Import(type.GetMethod(methodName, types));
        }

        public static MethodReference ImportMethod<T>(AssemblyDefinition assembly, string methodName)
        {
            return assembly.MainModule.Import(typeof(T).GetMethod(methodName));
        }

        public static MethodReference ImportPropertyGetter<T>(AssemblyDefinition assembly, string propertyName)
        {
            return assembly.MainModule.Import(typeof(T).GetProperty(propertyName).GetGetMethod());
        }

        public static MethodReference ImportMethod(AssemblyDefinition assembly, Type type, string methodName)
        {
            var input = type.GetMethod(methodName);
            return assembly.MainModule.Import(input);
        }

        public static TypeReference ImportType<T>(AssemblyDefinition assembly)
        {
            return assembly.MainModule.Import(typeof(T));
        }

        public static TypeReference ImportType(AssemblyDefinition assembly, Type type)
        {
            return assembly.MainModule.Import(type);
        }

        public static MethodReference ImportConstructor<T>(AssemblyDefinition assembly, params Type[] types)
        {
            return assembly.MainModule.Import(typeof(T).GetConstructor(types));
        }

        public static MethodReference ImportConstructor<T>(AssemblyDefinition assembly)
        {
            Type inputType = typeof(T);
            var ConstructorInfos = new List<ConstructorInfo>();

            foreach (var item in inputType.GetConstructors())
            {
                if (!item.IsStatic)
                    ConstructorInfos.Add(item);
            }
            return assembly.MainModule.Import(ConstructorInfos[0]);
        }

        public static MethodReference ImportConstructor(AssemblyDefinition assembly, Type inputType)
        {
            //var method = inputType.GetConstructors().First(c => !c.IsStatic);

            var ConstructorInfos = new List<ConstructorInfo>();
            foreach (var item in inputType.GetConstructors())
            {
                if (!item.IsStatic)
                    ConstructorInfos.Add(item);
            }
            return assembly.MainModule.Import(ConstructorInfos[0]);
        }

        public static MethodDefinition GetMethodDefinition(TypeDefinition typeDefinition, string name,
                                                           int paramcount)
        {
            foreach (MethodDefinition mdef in typeDefinition.Methods)
            {
                // demo purpose only
                if ((mdef.Name == name) && (paramcount == mdef.Parameters.Count))
                    return mdef;
            }
            throw new ArgumentException("Unable to find this method!");
        }

        public static List<MethodDefinition> GetMethods(TypeDefinition typeDefinition, bool showConstructor)
        {
            List<MethodDefinition> methodDefinitions = new List<MethodDefinition>();

            foreach (var item in typeDefinition.Methods)
            {
                methodDefinitions.Add(item);
            }

            if (showConstructor)
                return methodDefinitions;

            else
            {
                List<MethodDefinition> actualMethods = methodDefinitions;

                for (int i = actualMethods.Count - 1; i != 0; i--)
                {
                    if (actualMethods[i].IsConstructor)
                    {
                        actualMethods.Remove(actualMethods[i]);
                    }
                }

                return actualMethods;
            }
        }

        public static ParameterDefinition GetParameter(Mono.Cecil.Cil.MethodBody inputMethod, int index)
        {
            MethodDefinition method = inputMethod.Method;
            if (method.HasThis)
            {
                if (index == 0)
                    return inputMethod.ThisParameter;
                index--;
            }
            return method.Parameters[index];
        }

        public static void UpdateReferences(IEnumerable<Instruction> collection, Instruction oldTarget, Instruction newTarget)
        {
            foreach (var currentInstruction in collection)
            {
                if (currentInstruction.OpCode == OpCodes.Switch)
                {
                    var labels = (Instruction[])currentInstruction.Operand;
                    //for (var i=0;i<labels.Length;i++)
                    //{
                    //    if (Object.ReferenceEquals(labels[i], oldTarget))
                    //    {
                    //        labels[i] = newTarget;
                    //    }
                    //}
                    ReplaceAll(labels, oldTarget, newTarget);
                }
                else if (currentInstruction.Operand == oldTarget)
                {
                    currentInstruction.Operand = newTarget;
                }
            }
        }

        public static void UpdateReferences(IEnumerable<ExceptionHandler> handlers, Instruction oldTarget, Instruction newTarget)
        {
            foreach (var handler in handlers)
            {
                if (handler.TryEnd == oldTarget)
                    handler.TryEnd = newTarget;
                if (handler.TryStart == oldTarget)
                    handler.TryStart = newTarget;
                if (handler.HandlerStart == oldTarget)
                    handler.HandlerStart = newTarget;
                if (handler.FilterStart == oldTarget)
                    handler.FilterStart = newTarget;
            }
        }

        public static void ReplaceAll(Instruction[] labels, Instruction oldTarget, Instruction newTarget)
        {
            for (int i = 0; i < labels.Length; ++i)
            {
                if (labels[i] == oldTarget)
                {
                    labels[i] = newTarget;
                }
            }
        }

        public static Runtime GetRuntime(ModuleDefinition module)
        {
            switch (module.Runtime)
            {
                case TargetRuntime.Net_1_0:
                    return Runtime.Net_1_0;

                case TargetRuntime.Net_1_1:
                    return Runtime.Net_1_1;

                case TargetRuntime.Net_2_0:
                    return Runtime.Net_2_0;

                default:
                case TargetRuntime.Net_4_0:
                    return Runtime.Net_4_0;
            }
        }

    }
}