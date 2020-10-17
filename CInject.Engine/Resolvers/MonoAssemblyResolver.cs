﻿using System;
using System.Collections.Generic;
using System.IO;
using CInject.Engine.Data;
using CInject.Engine.Extensions;
using CInject.Injections;
using CInject.Engine.Utils;
using Mono.Cecil;
using Mono.Cecil.Cil;
using CInject.Injections.Library;
using System.Diagnostics;

namespace CInject.Engine.Resolvers
{
    public class MonoAssemblyResolver : BaseAssemblyResolver
    {
        private AssemblyDefinition _assembly;

        private TypeReference _cinjection;
        private MethodReference _cinjectionCtor;
        private MethodReference _methodGetCurrentMethod;
        private MethodReference _methodSetMethod;
        private MethodReference _methodGetExecutingAssembly;
        private MethodReference _methodSetExecutingAssembly;

        private MethodReference _methodGetArguments;
        private MethodReference _methodSetArguments;

        public MonoAssemblyResolver(string path)
            : base(path)
        {
            string directory = System.IO.Path.GetDirectoryName(path);

            DefaultAssemblyResolver assemblyResolver = new DefaultAssemblyResolver();
            assemblyResolver.AddSearchDirectory(directory);

            ReaderParameters readerParams = new ReaderParameters { AssemblyResolver = assemblyResolver };

            _assembly = AssemblyDefinition.ReadAssembly(path, readerParams);

            _cinjection = MonoExtensions.ImportType<CInjection>(Assembly);
            _cinjectionCtor = MonoExtensions.ImportConstructor<CInjection>(Assembly);

            _methodGetCurrentMethod = MonoExtensions.ImportMethod<System.Reflection.MethodBase>(Assembly, "GetCurrentMethod");
            _methodSetMethod = MonoExtensions.ImportMethod<CInjection>(Assembly, "set_Method");

            _methodGetExecutingAssembly = MonoExtensions.ImportMethod<System.Reflection.Assembly>(Assembly, "GetExecutingAssembly");
            _methodSetExecutingAssembly = MonoExtensions.ImportMethod<CInjection>(Assembly, "set_ExecutingAssembly");

            _methodGetArguments = MonoExtensions.ImportMethod<CInjection>(Assembly, "get_Arguments");
            _methodSetArguments = MonoExtensions.ImportMethod<CInjection>(Assembly, "set_Arguments");
        }

        public AssemblyDefinition Assembly
        {
            get { return _assembly; }
            internal set { _assembly = value; }
        }

        public List<TypeDefinition> FindTypes<T1>() where T1 : ICInject
        {
            var injectionTypes = new List<TypeDefinition>();

            foreach (var type in _assembly.MainModule.Types)
            {
                //type.Interfaces.Count

                var v1 = false;
                foreach (var item in type.Interfaces)
                {
                    if (item.FullName == "CInject.Injections.Interfaces.ICInject")
                    {
                        v1 = true;
                    }
                }

                if (v1)
                {
                    injectionTypes.Add(type);
                }

            }

            return injectionTypes;
        }

        internal List<TypeDefinition> FindStaticClasses()
        {
            List<TypeDefinition> typeDefinitions = new List<TypeDefinition>();

            foreach (var type in _assembly.MainModule.Types)
            {
                var isStatic = false;
                foreach (var item in type.Methods)
                {
                    if (item.IsStatic)
                        isStatic = true;
                }
                if (isStatic)
                    typeDefinitions.Add(type);
            }

            return typeDefinitions;
        }

        public List<TypeDefinition> FindClasses()
        {
            List<TypeDefinition> typeDefinitions = new List<TypeDefinition>();

            foreach (var type in _assembly.MainModule.Types)
            {
                if (type.IsClass)
                    typeDefinitions.Add(type);
            }

            return typeDefinitions;

        }

        public bool Inject(Type type)
        {
            //if (Assembly.MainModule.GetRuntime() != type.Assembly.GetRuntime())
            //{
            //    SendMessage("Injector and Target Assembly have different CLR versions! Can not proceed.", MessageType.Error);
            //    return false;
            //}

            var injection = CreateInjection(type);

            _assembly.MainModule.Import(injection.InjectionType);

            return PatchModules(_assembly.Modules, injection);
        }

        private Injection CreateInjection(Type type)
        {
            var injection = new Injection
            {
                InjectionType = type,
                OnInvoke = MonoExtensions.ImportMethod(Assembly, type, "OnInvoke"),
                Constructor = MonoExtensions.ImportConstructor(Assembly, type),
                TypeReference = MonoExtensions.ImportType(Assembly, type),
                OnComplete = MonoExtensions.ImportMethod(Assembly, type, "OnComplete"),
            };
            return injection;
        }

        private bool PatchModules(IEnumerable<ModuleDefinition> collection, Injection injection)
        {
            bool success = true;
            foreach (var module in collection)
            {
                success &= PatchTypes(module.Types, injection);
            }
            return success;
        }

        private bool PatchTypes(IEnumerable<TypeDefinition> collection, Injection injection)
        {
            bool success = true;
            foreach (var type in collection)
            {
                success &= PatchMethods(type.Methods, injection);
            }
            return success;
        }

        public bool Inject(MethodDefinition methodDefinition, Type type)
        {
            bool success = true;
            //if (Assembly.MainModule.GetRuntime() != type.Assembly.GetRuntime())
            //{
            //    SendMessage("Injector and Target Assembly have different CLR versions! Can not proceed.", MessageType.Error);
            //    return false;
            //}

            var injection = CreateInjection(type);

            try
            {
                _assembly.MainModule.Import(injection.InjectionType);
            }
            catch
            {
            }

            if (injection.InjectionType.Name == "Startup")
            {
                return PatchMethod2(methodDefinition, injection);
            }
            else
            {
                return PatchMethod(methodDefinition, injection);
            }
        }

        private bool PatchMethods(IEnumerable<MethodDefinition> collection, Injection injection)
        {
            bool success = true;
            foreach (var method in collection)
            {
                success &= PatchMethod(method, injection);
            }
            return success;
        }

        public bool PatchMethod(MethodDefinition method, Injection injection)
        {
            bool success = true;
            if (method.IsConstructor ||
                method.IsAbstract ||
                method.IsSetter ||
                (method.IsSpecialName && !method.IsGetter) || // to allow getter methods
                method.IsGenericInstance ||
                method.IsManaged == false ||
                method.Body == null)
            {
                SendMessage("Ignored method: " + method.Name, MessageType.Warning);
                return true;
            }

            try
            {
                var constructor = injection.Constructor;
                constructor.Resolve();

                bool isInjected = false;
                foreach (var x in method.Body.Variables)
                {
                    if (x.VariableType.Scope == injection.TypeReference.Scope
                        && x.VariableType.FullName == injection.TypeReference.FullName
                        && x.VariableType.Namespace == injection.TypeReference.Namespace)
                    {
                        isInjected = true;
                    }
                }
               
                if (isInjected) // already injected
                {
                    SendMessage("Already injected method " + method.Name + " with " + injection.TypeReference.Name, MessageType.Warning);
                    return true;
                }

                MethodInjector editor = new MethodInjector(method);

                VariableDefinition vInject = editor.AddVariable(injection.TypeReference);
                VariableDefinition vInjection = editor.AddVariable(_cinjection);
                VariableDefinition vObjectArray = editor.AddVariable(MonoExtensions.ImportType<object[]>(Assembly));

                Instruction firstExistingInstruction = method.Body.Instructions[0];

                // create constructor of Injector
                editor.InsertBefore(firstExistingInstruction, editor.Create(OpCodes.Newobj, constructor));
                editor.InsertBefore(firstExistingInstruction, editor.Create(OpCodes.Stloc_S, vInject));

                #region OnInvoke without Param
                //editor.InsertBefore(firstExistingInstruction, editor.Create(OpCodes.Ldloc_S, vInject));
                //editor.InsertBefore(firstExistingInstruction, editor.Create(OpCodes.Callvirt, injection.OnInvoke));
                #endregion

                #region OnInvoke with Param
                // create constructor of CInjection
                editor.InsertBefore(firstExistingInstruction, editor.Create(OpCodes.Newobj, _cinjectionCtor));
                editor.InsertBefore(firstExistingInstruction, editor.Create(OpCodes.Stloc_S, vInjection));

                // create parameter of GetCurrentMethod
                editor.InsertBefore(firstExistingInstruction, editor.Create(OpCodes.Ldloc_S, vInjection));
                editor.InsertBefore(firstExistingInstruction, editor.Create(OpCodes.Call, _methodGetCurrentMethod));
                editor.InsertBefore(firstExistingInstruction, editor.Create(OpCodes.Callvirt, _methodSetMethod));
                editor.InsertBefore(firstExistingInstruction, editor.Create(OpCodes.Nop));

                // create parameter of GetExecutingAssembly
                editor.InsertBefore(firstExistingInstruction, editor.Create(OpCodes.Ldloc_S, vInjection));
                editor.InsertBefore(firstExistingInstruction, editor.Create(OpCodes.Call, _methodGetExecutingAssembly));
                editor.InsertBefore(firstExistingInstruction, editor.Create(OpCodes.Callvirt, _methodSetExecutingAssembly));
                editor.InsertBefore(firstExistingInstruction, editor.Create(OpCodes.Nop));

                if (method.Parameters.Count > 0)
                {
                    // create array of object (arguments)
                    editor.InsertBefore(firstExistingInstruction, editor.Create(OpCodes.Ldloc_S, vInjection));
                    editor.InsertBefore(firstExistingInstruction, editor.Create(OpCodes.Ldc_I4, method.Parameters.Count));
                    editor.InsertBefore(firstExistingInstruction, editor.Create(OpCodes.Newarr, MonoExtensions.ImportType<object>(Assembly)));
                    editor.InsertBefore(firstExistingInstruction, editor.Create(OpCodes.Stloc_S, vObjectArray));

                    for (int i = 0; i < method.Parameters.Count; i++)
                    {
                        bool processAsNormal = true;

                        if (method.Parameters[i].ParameterType.IsByReference)
                        {
                            /* Sample Instruction set:
                             * L_002a: ldloc.2 
                             * L_002b: ldc.i4.0 
                             * L_002c: ldarg.1 
                             * L_002d: ldind.ref 
                             * L_002e: stelem.ref 
                             * */

                            editor.InsertBefore(firstExistingInstruction, editor.Create(OpCodes.Ldloc_S, vObjectArray));
                            editor.InsertBefore(firstExistingInstruction, editor.Create(OpCodes.Ldc_I4, i));
                            editor.InsertBefore(firstExistingInstruction, editor.Create(OpCodes.Ldarg, method.Parameters[i]));
                            editor.InsertBefore(firstExistingInstruction, editor.Create(OpCodes.Ldind_Ref));
                            editor.InsertBefore(firstExistingInstruction, editor.Create(OpCodes.Stelem_Ref));

                            processAsNormal = false;
                        }
                        //else if (method.Parameters[i].ParameterType.IsArray)
                        //{

                        //}
                        //else if (method.Parameters[i].ParameterType.IsDefinition) // delegate needs no seperate handling
                        //{

                        //}
                        else if (method.Parameters[i].ParameterType.IsFunctionPointer)
                        {

                        }
                        //else if (method.Parameters[i].ParameterType.IsOptionalModifier)
                        //{

                        //}
                        else if (method.Parameters[i].ParameterType.IsPointer)
                        {

                        }
                        else
                        {
                            processAsNormal = true;
                        }

                        if (processAsNormal)
                        {
                            /* Sample Instruction set: for simple PARAMETER
                             * L_0036: ldloc.s objArray
                             * L_0038: ldc.i4 0
                             * L_003d: ldarg array
                             * L_0041: box Int32    <-------------- anything can be here
                             * L_0046: stelem.ref 
                             * */

                            /* Sample Instruction set: for ARRAY
                             * L_0036: ldloc.s objArray
                             * L_0038: ldc.i4 0
                             * L_003d: ldarg array
                             * L_0041: box string[]
                             * L_0046: stelem.ref 
                             * */

                            editor.InsertBefore(firstExistingInstruction, editor.Create(OpCodes.Ldloc_S, vObjectArray));
                            editor.InsertBefore(firstExistingInstruction, editor.Create(OpCodes.Ldc_I4, i));
                            editor.InsertBefore(firstExistingInstruction, editor.Create(OpCodes.Ldarg, method.Parameters[i]));
                            editor.InsertBefore(firstExistingInstruction, editor.Create(OpCodes.Box, method.Parameters[i].ParameterType));
                            editor.InsertBefore(firstExistingInstruction, editor.Create(OpCodes.Stelem_Ref));
                        }
                    }

                    editor.InsertBefore(firstExistingInstruction, editor.Create(OpCodes.Ldloc_S, vObjectArray));
                    editor.InsertBefore(firstExistingInstruction, editor.Create(OpCodes.Callvirt, _methodSetArguments));
                }

                // call OnInvoke with appropriate parameters
                editor.InsertBefore(firstExistingInstruction, editor.Create(OpCodes.Ldloc_S, vInject));
                editor.InsertBefore(firstExistingInstruction, editor.Create(OpCodes.Ldloc_S, vInjection));
                editor.InsertBefore(firstExistingInstruction, editor.Create(OpCodes.Callvirt, injection.OnInvoke));
                editor.InsertBefore(firstExistingInstruction, editor.Create(OpCodes.Nop));

                #endregion

                #region OnComplete
                var exitInstruction = new List<Instruction>();
                foreach (var x in method.Body.Instructions)
                {
                    if (x.OpCode == OpCodes.Ret)
                    {
                        exitInstruction.Add(x);
                    }
                }

                Instruction[] exitInstructions = exitInstruction.ToArray();

                for (int i = 0; i < exitInstructions.Length; i++)
                {
                    var previous = exitInstructions[i].Previous; // most likely previous statement will be NOP, LDLOC.0 (pop, or load from stack)
                    editor.InsertBefore(previous, editor.Create(OpCodes.Ldloc_S, vInject));
                    editor.InsertBefore(previous, editor.Create(OpCodes.Callvirt, injection.OnComplete));

                    Debug.WriteLine(method.Name + " " + method.ReturnType.Name + " " + previous.OpCode);
                }
                #endregion

                method.Resolve();

                SendMessage("Injected method " + method, MessageType.Output);
            }
            catch (Exception ex)
            {
                SendMessage(ex.Message, MessageType.Error);
                return false;
            }

            return true;
        }

        private void FindExitStatements(MethodInjector editor, Injection injection)
        {
            //TODO: Find Exit Statements 
        }

        public bool PatchMethod2(MethodDefinition method, Injection injection)
        {
            bool success = true;
            if (method.IsConstructor ||
                method.IsAbstract ||
                method.IsSetter ||
                (method.IsSpecialName && !method.IsGetter) || // to allow getter methods
                method.IsGenericInstance ||
                method.IsManaged == false ||
                method.Body == null)
            {
                SendMessage("Ignored method: " + method.Name, MessageType.Warning);
                return true;
            }

            try
            {
                var constructor = injection.Constructor;
                constructor.Resolve();

                bool isInjected = false;
                foreach (var x in method.Body.Variables)
                {
                    if (x.VariableType.Scope == injection.TypeReference.Scope
                        && x.VariableType.FullName == injection.TypeReference.FullName
                        && x.VariableType.Namespace == injection.TypeReference.Namespace)
                    {
                        isInjected = true;
                    }
                }


                MethodInjector editor = new MethodInjector(method);

                VariableDefinition vInject = editor.AddVariable(injection.TypeReference);
                VariableDefinition vInjection = editor.AddVariable(_cinjection);
                VariableDefinition vObjectArray = editor.AddVariable(MonoExtensions.ImportType<object[]>(Assembly));

                Instruction firstExistingInstruction = method.Body.Instructions[0];

                // create constructor of Injector
                editor.InsertBefore(firstExistingInstruction, editor.Create(OpCodes.Newobj, constructor));
                editor.InsertBefore(firstExistingInstruction, editor.Create(OpCodes.Stloc_S, vInject));

                #region OnInvoke without Param
                editor.InsertBefore(firstExistingInstruction, editor.Create(OpCodes.Ldloc_S, vInject));
                editor.InsertBefore(firstExistingInstruction, editor.Create(OpCodes.Callvirt, injection.OnInvoke));
                #endregion


                method.Resolve();

                SendMessage("Injected method " + method, MessageType.Output);
            }
            catch (Exception ex)
            {
                SendMessage(ex.Message, MessageType.Error);
                return false;
            }

            return true;
        }



        public bool Save()
        {
            try
            {
                _assembly.Write(Path, new WriterParameters { WriteSymbols = false });
                SendMessage("New assembly saved " + Path, MessageType.Output);
                return true;
            }
            catch (Exception ex)
            {
                SendMessage(ex.Message, MessageType.Error);
                return false;
            }
        }
    }
}