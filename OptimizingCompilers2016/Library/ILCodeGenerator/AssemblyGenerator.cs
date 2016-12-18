using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;
using OptimizingCompilers2016.Library.ThreeAddressCode;
using OptimizingCompilers2016.Library.ThreeAddressCode.Values;
using System.Diagnostics;

namespace OptimizingCompilers2016.Library.ILCodeGenerator
{
    /// <summary>
    /// Generates .NET assembly from base blocks
    /// </summary>
    public static class AssemblyGenerator
    {

        private static Dictionary<string, Label> labels = new Dictionary<string, Label>();
        private static Dictionary<string, LocalBuilder> variables = new Dictionary<string, LocalBuilder>();
        private static Dictionary<string, IThreeAddressCode> conditions = new Dictionary<string, IThreeAddressCode>();

        private static readonly Dictionary<Operation, OpCode> threeAddressCodeOpsToIL = new Dictionary<Operation, OpCode>
        {
            {Operation.Plus, OpCodes.Add },
            {Operation.Minus, OpCodes.Sub },
            {Operation.Mult, OpCodes.Mul },
            {Operation.Div, OpCodes.Div }
        };

        private static readonly Dictionary<Operation, Tuple<OpCode, bool>> threeAddressCodeCondToIL =
            new Dictionary<Operation, Tuple<OpCode, bool>>
            {
                {Operation.Eq, Tuple.Create(OpCodes.Ceq, false)},
                {Operation.NotEq, Tuple.Create(OpCodes.Ceq, true)},
                {Operation.Less, Tuple.Create(OpCodes.Clt, false)},
                {Operation.Great, Tuple.Create(OpCodes.Cgt, false)},
                {Operation.LessOrEq, Tuple.Create(OpCodes.Cgt, true)},
                {Operation.GreatOrEq, Tuple.Create(OpCodes.Clt, true)}
            };

        private static void GenCondition(ILGenerator ilGen, Operation op, IValue lOperand, IValue rOperand, IValue lvalue)
        {
            //LoadRval(ilGen, lOperand);
            //LoadRval(ilGen, rOperand);
            
            //switch(op)
            //{
            //    case Operation.Eq:

            //}
        }

        private static void LoadRval(ILGenerator ilGen, IValue val)
        {
            if (val is IdentificatorValue)
            {
                string localName = (val as IdentificatorValue).Value;
                if (!variables.ContainsKey(localName))
                {
                    Console.WriteLine("Error: expected local variable {0} to be defined earlier. Exiting...", val.Value);
                    Environment.Exit(1);
                }
                var declaredLocal = variables[localName];
                ilGen.Emit(OpCodes.Ldloc, declaredLocal);
            }
            else if (val is NumericValue)
            {
                int value = (val as NumericValue).Value;
                ilGen.Emit(OpCodes.Ldc_I4, value);
            }
            else
            {
                Console.WriteLine("Error: expected operand to be IdentificatorValue or NumericalValue in {0}. Closing...", val.ToString());
                Console.ReadLine();
                Environment.Exit(1);
            }
        }

        public static void Generate(List<BaseBlock> blocks, string assemblyName)
        {

            var linear = new List<IThreeAddressCode>();
            for(var block = blocks[0]; block != null; block = block.Output)
                linear.AddRange(block.Commands);


            AssemblyName assemblyIdentity = new AssemblyName();
            assemblyIdentity.Name = assemblyName;
            AppDomain appDomain = AppDomain.CurrentDomain;
            AssemblyBuilder bAssembly = appDomain.DefineDynamicAssembly(assemblyIdentity, AssemblyBuilderAccess.Save);

            ModuleBuilder bModel = bAssembly.DefineDynamicModule(assemblyIdentity.Name, 
                string.Format("{0}.exe", assemblyName), 
                true);

            TypeBuilder bType = bModel.DefineType(string.Format("{0}.Program", assemblyName), 
                TypeAttributes.Public | TypeAttributes.Class);

            MethodBuilder bMethod = bType.DefineMethod("Main", MethodAttributes.Public | MethodAttributes.Static);

            ILGenerator ilGen = bMethod.GetILGenerator();

            foreach(var line in linear)
            {
                if (line.Label != null)
                {
                    var labelStr = line.Label.Value;
                    if(!labels.ContainsKey(labelStr))
                        labels[labelStr] = ilGen.DefineLabel();
                    ilGen.MarkLabel(labels[labelStr]);
                }

                switch (line.Operation)
                {
                    case Operation.Assign:
                        {
                            var lval = line.Destination as IdentificatorValue;
                            if (!variables.ContainsKey(lval.Value))
                            {
                                variables[lval.Value] = ilGen.DeclareLocal(typeof(int));
                            }
                            var rval = line.LeftOperand;
                            LoadRval(ilGen, rval);
                            ilGen.Emit(OpCodes.Stloc, variables[lval.Value]);
                            break;
                        }
                    case Operation.Plus:
                    case Operation.Minus:
                    case Operation.Mult:
                    case Operation.Div:
                        {
                            var lval = line.Destination as IdentificatorValue;
                            if (lval == null)
                            {
                                Console.WriteLine("Error: expected lvalue to be IdentificatorValue in {0}. Closing...", line.ToString());
                                Console.ReadLine();
                                Environment.Exit(1);
                            }
                            if (!variables.ContainsKey(lval.Value))
                            {
                                variables[lval.Value] = ilGen.DeclareLocal(typeof(int));
                            }
                            var leftOperand = line.LeftOperand;
                            LoadRval(ilGen, leftOperand);
                            var rightOperand = line.RightOperand;
                            LoadRval(ilGen, rightOperand);
                            var binOpCode = threeAddressCodeOpsToIL[line.Operation];
                            ilGen.Emit(binOpCode);
                            ilGen.Emit(OpCodes.Stloc, variables[lval.Value]);
                            break;
                        }
                    case Operation.Goto:
                        {
                            var dest = line.Destination as LabelValue;
                            if (!labels.ContainsKey(dest.Value))
                            {
                                labels[dest.Value] = ilGen.DefineLabel();
                            }
                            ilGen.Emit(OpCodes.Br_S, labels[dest.Value]);
                            break;
                        }
                    case Operation.CondGoto:
                        {
                            var dest = line.Destination as LabelValue ;
                            if (!labels.ContainsKey(dest.Value))
                            {
                                labels[dest.Value] = ilGen.DefineLabel();
                            }
                            var cond = line.LeftOperand;
                            if (cond is IdentificatorValue)
                            {
                                var condId = cond as IdentificatorValue;
                                if (!variables.ContainsKey(condId.Value))
                                {
                                    Console.WriteLine("Error: exepected condition to be defined earlier in {0}. Exiting...", line.ToString());
                                    Console.ReadLine();
                                    Environment.Exit(1);
                                }
                                ilGen.Emit(OpCodes.Ldloc, variables[condId.Value]);
                                ilGen.Emit(OpCodes.Brtrue_S, labels[dest.Value]);
                            }
                            else if (cond is NumericValue)
                            {
                                int value = (cond as NumericValue).Value;
                                ilGen.Emit(OpCodes.Ldc_I4, value);
                                ilGen.Emit(OpCodes.Brtrue_S, labels[dest.Value]);
                            }
                            else
                            {
                                Console.WriteLine("Error: expected IdentificatorValue or NumrericValue as condition in {0}. Exiting...", line.ToString());
                                Console.ReadLine();
                                Environment.Exit(1);
                            }

                            break;
                        }
                    case Operation.Eq:
                    case Operation.Great:
                    case Operation.GreatOrEq:
                    case Operation.Less:
                    case Operation.LessOrEq:
                    case Operation.NotEq:
                        {
                            var lval = line.Destination as IdentificatorValue;
                            if (lval == null)
                            {
                                Console.WriteLine("Error: expected lvalue to be IdentificatorValue in {0}. Closing...", line.ToString());
                                Console.ReadLine();
                                Environment.Exit(1);
                            }
                            if (!variables.ContainsKey(lval.Value))
                            {
                                variables[lval.Value] = ilGen.DeclareLocal(typeof(int));
                            }
                            LoadRval(ilGen, line.LeftOperand);
                            LoadRval(ilGen, line.RightOperand);
                            var ilCond = threeAddressCodeCondToIL[line.Operation];
                            ilGen.Emit(ilCond.Item1);
                            if (ilCond.Item2) //if should switch 0 to 1 and vice versa
                            {
                                ilGen.Emit(OpCodes.Ldc_I4_1);
                                ilGen.Emit(OpCodes.Xor);
                            }
                            ilGen.Emit(OpCodes.Stloc, variables[lval.Value]);
                            break;
                        }
                    case Operation.NoOp:
                        ilGen.Emit(OpCodes.Nop);
                        break;
                }
            }

            Type t = bType.CreateType();
            bAssembly.SetEntryPoint(bMethod, PEFileKinds.ConsoleApplication);
            bAssembly.Save(string.Format("{0}.exe", assemblyName));
        }
    }
}
