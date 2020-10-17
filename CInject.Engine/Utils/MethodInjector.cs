using CInject.Engine.Extensions;
using Mono.Cecil.Cil;
using Mono.Cecil;

namespace CInject.Engine.Utils
{
    public class MethodInjector
    {
        private readonly ILProcessor _ilprocessor;
        private readonly MethodBody _methodBody;

        public MethodBody MethodBody
        {
            get { return _methodBody; }
        } 

        public MethodInjector(MethodDefinition method)
        {
            _methodBody = method.Body;
            _ilprocessor = method.Body.GetILProcessor();
        }

        public Instruction Create(OpCode opcode, VariableDefinition variable)
        {
            return _ilprocessor.Create(opcode, variable);
        }

        public void InsertBefore(Instruction target, Instruction instruction)
        {
            _ilprocessor.InsertBefore(target, instruction);
            UpdateReferences(target, instruction); // shift the instructions and offsets & handlers down
        }

        private void UpdateReferences(Instruction instruction, Instruction replaceBy)
        {
            if (_methodBody.Instructions.Count > 0)
                MonoExtensions.UpdateReferences(_methodBody.Instructions,instruction, replaceBy);

            if (_methodBody.ExceptionHandlers.Count > 0)
                MonoExtensions.UpdateReferences(_methodBody.ExceptionHandlers,instruction, replaceBy);
        }

        internal Instruction Create(OpCode opCode, ParameterDefinition parameterDefinition)
        {
            return _ilprocessor.Create(opCode, parameterDefinition);
        }

        internal Instruction Create(OpCode opCode)
        {
            return _ilprocessor.Create(opCode);
        }

        internal Instruction Create(OpCode opCode, int value)
        {
            return _ilprocessor.Create(opCode, value);
        }

        public Instruction Create(OpCode opCode, MethodReference reference)
        {
            return _ilprocessor.Create(opCode, reference);
        }

        public Instruction Create(OpCode opCode, TypeReference reference)
        {
            return _ilprocessor.Create(opCode, reference);
        }

        public Instruction Create(OpCode opCode, FieldReference reference)
        {
            return _ilprocessor.Create(opCode, reference);
        }

        public VariableDefinition AddVariable(TypeReference type)
        {
            _methodBody.InitLocals = true;

            VariableDefinition variable = new VariableDefinition(type);
            _methodBody.Variables.Add(variable);

            return variable;
        }

        //public PropertyDefinition AddProperty(string name, TypeReference type)
        //{
        //    PropertyDefinition property = new PropertyDefinition(name, PropertyAttributes.HasDefault, type);
        //    _ilprocessor.Create(OpCodes.
        //}
    }
}
