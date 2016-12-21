﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimizingCompilers2016.Library.ThreeAddressCode.Values;
using OptimizingCompilers2016.Library.ThreeAddressCode;
using OptimizingCompilers2016.Library.ThreeAddressCode.Values;

namespace OptimizingCompilers2016.Library.Analysis
{
    public enum VariableValueType {
        UNDEF,
        NAC,
        CONSTANT
    };

    public struct VariableValue {
        public VariableValueType type;
        public int constantValue;
    }

    public class GlobalConstantPropagation : BaseIterationAlgorithm<VariableMap>
    {
        List<BaseBlock> blocks = new List<BaseBlock>();
        public GlobalConstantPropagation(List<BaseBlock> _blocks) {
            blocks = _blocks;
        }

        public void runAnalys()
        {
            base.IterationAlgorithm(blocks);
            foreach (var block in blocks){
                Console.WriteLine("Block " + block.Name);
                Console.WriteLine(outs[block].ToString());
                Console.WriteLine("---------------------------------");
            }

        }

        protected override VariableMap SetStartingSet()
        {
            var currentMap = new VariableMap();

            foreach (var block in blocks) {
                for (int i = 0; i < block.Commands.Count; i++)
                {
                    if (block.Commands[i].Operation.Equals(Operation.Assign)) {
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

        public override VariableMap Collect(VariableMap x, VariableMap y)
        {
            Dictionary<IdentificatorValue, VariableValue> variableTable = new Dictionary<IdentificatorValue, VariableValue>(x.variableTable.Count);
            foreach (KeyValuePair<IdentificatorValue, VariableValue> variable in x.variableTable) {
                if (!variableTable.ContainsKey(variable.Key)) {
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
            else {
                newValue.type = VariableValueType.NAC;
            }
            return newValue;
        }

        protected override VariableMap Transfer(VariableMap x, BaseBlock b)
        {
            VariableMap newM = x.Clone() as VariableMap;

            foreach (var line in b.Commands) {
                if (line.Operation.Equals(Operation.Assign))
                {
                    if (line.RightOperand == null)
                    {
                        if (line.LeftOperand is NumericValue)
                        {
                            VariableValue newValue = new VariableValue();
                            newValue.type = VariableValueType.CONSTANT;
                            newValue.constantValue = (line.LeftOperand as NumericValue).Value;
                            newM.variableTable[line.Destination as IdentificatorValue] = newValue;
                        }
                        
                    }
                    else
                    {
                        newM.variableTable[line.Destination as IdentificatorValue] =
                            Calculate(line, x);
                    }
                }
            }
            return newM;
        }

        private VariableValue Calculate(IThreeAddressCode line, VariableMap currentTable) {
            VariableValue newValue = new VariableValue();
            VariableValue x = currentTable.variableTable[line.LeftOperand as IdentificatorValue];
            VariableValue y = currentTable.variableTable[line.RightOperand as IdentificatorValue];
            Operation op = line.Operation;

            if (x.type.Equals(VariableValueType.CONSTANT) &&
               (y.type.Equals(VariableValueType.CONSTANT)))
            {
                newValue.type = VariableValueType.CONSTANT;
                newValue.constantValue = CalculateConstant(op, x.constantValue, y.constantValue);
            }
            else
            if (x.type.Equals(VariableValueType.NAC) &&
               (y.type.Equals(VariableValueType.NAC))) {
                newValue.type = VariableValueType.NAC;
            }
            else
            {
                newValue.type = VariableValueType.UNDEF;
            }
            return newValue;
        }

        //TODO
        private int CalculateConstant(Operation op, int x, int y) {
            switch (op) {
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

    }


    public class VariableMap : ICloneable {

        public Dictionary<IdentificatorValue, VariableValue> variableTable { get; }

        public VariableMap() {
            variableTable = new Dictionary<IdentificatorValue, VariableValue>();
        }

        public VariableMap(Dictionary<IdentificatorValue, VariableValue> newDictionary)
        {
            variableTable = newDictionary;
        }

        public object Clone()
        {
            Dictionary<IdentificatorValue, VariableValue> clonedDictionary = new Dictionary<IdentificatorValue, VariableValue>(variableTable.Count);

            foreach (KeyValuePair<IdentificatorValue, VariableValue> entry in variableTable)
            {
                VariableValue variable = new VariableValue();
                variable.type = entry.Value.type;
                variable.constantValue = entry.Value.constantValue;
                clonedDictionary.Add(entry.Key, variable);
            }
            return new VariableMap(clonedDictionary);
        }

        public override bool Equals(object obj)
        {
            var secondMap = obj as VariableMap;
            bool isEqual = true;
            foreach (KeyValuePair<IdentificatorValue, VariableValue> entry in variableTable)
            {
                if (secondMap.variableTable.ContainsKey(entry.Key))
                {
                    var secondEntry = secondMap.variableTable[entry.Key];
                    isEqual = isEqual && (secondEntry.type.Equals(entry.Value.type) 
                        && secondEntry.constantValue == entry.Value.constantValue);
                }
                else
                {
                    return false;
                }
            }
            return isEqual;
        }


        public override string ToString()
        {
            String result = "";

            foreach (KeyValuePair<IdentificatorValue, VariableValue> entry in variableTable)
            {
                result += entry.Key + "     |     " 
                    + entry.Value.type
                    + (entry.Value.type.Equals(VariableValueType.CONSTANT) ? " : " + entry.Value.constantValue.ToString() : "") 
                    + "\n";
            }

            return result;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}
