using System;
using System.Collections.Generic;
using OptimizingCompilers2016.Library.ThreeAddressCode.Values;

namespace OptimizingCompilers2016.Library.Analysis.ConstantPropagation
{
    public enum VariableValueType
    {
        UNDEF,
        NAC,
        CONSTANT
    };

    public struct VariableValue
    {
        public VariableValueType type;
        public int constantValue;
    }


    public class VariableMap : ICloneable
    {

        public Dictionary<IdentificatorValue, VariableValue> variableTable { get; }

        public VariableMap()
        {
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