using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimizingCompilers2016.Library.ThreeAddressCode.Values;
using OptimizingCompilers2016.Library.ThreeAddressCode;
using OptimizingCompilers2016.Library.ThreeAddressCode.Values;

namespace OptimizingCompilers2016.Library.Analysis
{
    enum VariableValueType {
        UNDEF,
        NAC,
        CONSTANT
    };

    struct VariableValue {
        public VariableValueType type;
        public int constantValue;
    }

    class GlobalConstantPropagation : BaseIterationAlgorithm<Dictionary<String, VariableValue>>
    {
        List<BaseBlock> blocks = new List<BaseBlock>();
        public GlobalConstantPropagation(List<BaseBlock> _blocks) {
            blocks = _blocks;
        }

        protected override Dictionary<String, VariableValue> SetStartingSet()
        {
            Dictionary<String, VariableValue> variableTable = new Dictionary<String, VariableValue>();

            foreach (var block in blocks) {
                for (int i = 0; i < block.Commands.Count; i++)
                {
                    if (block.Commands[i].Operation == Operation.Assign) {
                        VariableValue newVar = new VariableValue();
                        newVar.type = VariableValueType.UNDEF;
                        var operand = (block.Commands[i].LeftOperand as IdentificatorValue).Value;
                        if (!variableTable.ContainsKey(operand))
                        {
                            variableTable.Add(operand, newVar);
                        }
                        
                    }

                }

            }

            return variableTable;
        }

        public override Dictionary<String, VariableValue> Collect(Dictionary<String, VariableValue> x, Dictionary<String, VariableValue> y)
        {
            Dictionary<String, VariableValue> variableTable = new Dictionary<String, VariableValue>(x.Count);
            foreach (KeyValuePair<String, VariableValue> variable in x) {
                if (!variableTable.ContainsKey(variable.Key)) {
                    variableTable.Add(variable.Key, VariableCollect(variable.Value, y[variable.Key]));
                }
                //TODO else 
            }

            return variableTable;
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

        protected override Dictionary<String, VariableValue> Transfer(Dictionary<String, VariableValue> x, BaseBlock b)
        {
            Dictionary<String, VariableValue> variableTable = new Dictionary<String, VariableValue>(x.Count);
            return variableTable;
        }


    }
}
