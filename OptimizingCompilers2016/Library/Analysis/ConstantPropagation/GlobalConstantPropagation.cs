using System;
using System.Collections.Generic;
using OptimizingCompilers2016.Library.ThreeAddressCode.Values;
using OptimizingCompilers2016.Library.ThreeAddressCode;
using OptimizingCompilers2016.Library.Optimizators;

namespace OptimizingCompilers2016.Library.Analysis.ConstantPropagation
{
    public class GlobalConstantPropagation : BaseIterationAlgorithm<VariableMap>
    {
        List<BaseBlock> _blocks = new List<BaseBlock>();

        public override void RunAnalysis(List<BaseBlock> blocks)
        {
            _blocks = blocks;
            base.IterationAlgorithm(blocks);
            foreach (var block in blocks)
            {
                Dictionary<IdentificatorValue, int> constants = new Dictionary<IdentificatorValue, int>();
                foreach (var val in ins[block].variableTable) {
                    if (val.Value.type.Equals(VariableValueType.CONSTANT)) {
                        constants.Add(val.Key, val.Value.constantValue);
                    }
                }

                ConstantPropagationOptimizator cpo = new ConstantPropagationOptimizator(constants);
                cpo.Optimize(block);

                //Console.WriteLine("Block " + block.Name);
                //Console.WriteLine(outs[block].ToString());
                //Console.WriteLine("---------------------------------");
            }
        }

        protected override VariableMap SetStartingSet()
        {
            var currentMap = new VariableMap();

            foreach (var block in _blocks)
            {
                for (int i = 0; i < block.Commands.Count; i++)
                {
                    var command = block.Commands[i];
                    if (isAssignment(block.Commands[i].Operation))
                    {
                        VariableValue newVar = new VariableValue();
                        newVar.type = VariableValueType.UNDEF;
                        var operand = (block.Commands[i].Destination as IdentificatorValue);
                        if (!currentMap.variableTable.ContainsKey(operand))
                        {
                            currentMap.variableTable.Add(operand, newVar);
                        }
                    }
                }
            }

            //Console.WriteLine(currentMap.ToString());

            return currentMap;
        }

        private bool isAssignment(Operation op) {
            return (op.Equals(Operation.Assign)) || (op.Equals(Operation.Plus)) ||
                (op.Equals(Operation.Minus)) || (op.Equals(Operation.Mult))  || (op.Equals(Operation.Div));
        }

        public override VariableMap Collect(VariableMap x, VariableMap y)
        {
            Dictionary<IdentificatorValue, VariableValue> variableTable = new Dictionary<IdentificatorValue, VariableValue>(x.variableTable.Count);
            foreach (KeyValuePair<IdentificatorValue, VariableValue> variable in x.variableTable)
            {
                if (!variableTable.ContainsKey(variable.Key))
                {
                    variableTable.Add(variable.Key, VariableCollect(variable.Value, y.variableTable[variable.Key]));
                }
                //TODO else 
            }

            return new VariableMap(variableTable);
        }

        private VariableValue VariableCollect(VariableValue x, VariableValue y)
        {
            VariableValue newValue = new VariableValue();
            if (x.type.Equals(VariableValueType.UNDEF))
            {
                newValue = y;
            }
            else
            if (y.type.Equals(VariableValueType.UNDEF))
            {
                newValue = x;
            }
            else
            if (x.type.Equals(VariableValueType.NAC) || y.type.Equals(VariableValueType.NAC))
            {
                newValue.type = VariableValueType.NAC;
            }
            else
            if (x.constantValue == y.constantValue)
            {
                newValue.type = VariableValueType.CONSTANT;
                newValue.constantValue = x.constantValue;
            }
            else
            {
                newValue.type = VariableValueType.NAC;
            }
            return newValue;
        }

        protected override VariableMap Transfer(VariableMap x, BaseBlock b)
        {
            VariableMap newM = x.Clone() as VariableMap;

            foreach (var line in b.Commands)
            {
                if (isAssignment(line.Operation))
                {
                    if (line.RightOperand == null)
                    {
                        //it is constant
                        if (line.LeftOperand is NumericValue)
                        {
                            VariableValue newValue = new VariableValue();
                            newValue.type = VariableValueType.CONSTANT;
                            newValue.constantValue = (line.LeftOperand as NumericValue).Value;
                            newM.variableTable[line.Destination as IdentificatorValue] = newValue;
                        }
                        //it is one variable
                        else {
                            newM.variableTable[line.Destination as IdentificatorValue] =
                            Calculate(line, x);
                        }


                    }
                    else
                    {
                        newM.variableTable[line.Destination as IdentificatorValue] =
                            Calculate(line, x);
                    }
                }

                x = newM;
            }
            return newM;
        }

        private VariableValue Calculate(IThreeAddressCode line, VariableMap currentTable)
        {
            VariableValue newValue = new VariableValue();

            if ( line.LeftOperand is NumericValue )
            {
                newValue.type = VariableValueType.CONSTANT;
                newValue.constantValue = (line.LeftOperand as NumericValue).Value;
            }
            else
            {
                if (!currentTable.variableTable.ContainsKey(line.LeftOperand as IdentificatorValue)) {
                    newValue.type = VariableValueType.NAC;
                    return newValue;
                }

                VariableValue x = currentTable.variableTable[line.LeftOperand as IdentificatorValue];
                if (x.type.Equals(VariableValueType.CONSTANT))
                {
                    newValue.type = VariableValueType.CONSTANT;
                    newValue.constantValue = x.constantValue;
                }
                else if (x.type.Equals(VariableValueType.NAC))
                {
                    newValue.type = VariableValueType.NAC;
                }
                else
                {
                    newValue.type = VariableValueType.UNDEF;
                }
            }

            if (line.RightOperand == null || newValue.type == VariableValueType.NAC)
            {
                return newValue;
            }

            if (line.RightOperand is NumericValue) {
                newValue.constantValue = CalculateConstant(line.Operation, newValue.constantValue, (line.RightOperand as NumericValue).Value);
                return newValue;
            }

            if (!currentTable.variableTable.ContainsKey(line.RightOperand as IdentificatorValue))
            {
                newValue.type = VariableValueType.NAC;
                return newValue;
            }

            VariableValue y = currentTable.variableTable[line.RightOperand as IdentificatorValue];

            if (y.type.Equals(VariableValueType.CONSTANT))
            {
                newValue.constantValue = CalculateConstant(line.Operation, newValue.constantValue, y.constantValue);
                return newValue;
            }

            if (y.type.Equals(VariableValueType.NAC))
            {
                newValue.type = VariableValueType.NAC;
            }
            else
            {
                newValue.type = VariableValueType.UNDEF;
            }
            return newValue;
        }

        public static int CalculateConstant(Operation op, int x, int y)
        {
            switch (op)
            {
                case Operation.Plus:
                    return x + y;
                case Operation.Minus:
                    return x - y;
                case Operation.Mult:
                    return x * y;
                case Operation.Div:
                    return x / y;
                default:
                    return 0;
            }
        }

        protected override void FillGeneratorsAndKillers(List<BaseBlock> blocks)
        {
            // no realization
        }
    }
}
