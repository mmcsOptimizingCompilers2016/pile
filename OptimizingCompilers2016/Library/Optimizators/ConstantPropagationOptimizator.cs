using OptimizingCompilers2016.Library.ThreeAddressCode;
using OptimizingCompilers2016.Library.ThreeAddressCode.Values;
using OptimizingCompilers2016.Library.Analysis.ConstantPropagation;
using System.Collections.Generic;

namespace OptimizingCompilers2016.Library.Optimizators
{
    public class ConstantPropagationOptimizator : IOptimizator
    {
        private Dictionary<IdentificatorValue, VariableValue> additionalConstants = new Dictionary<IdentificatorValue, VariableValue>();
        public ConstantPropagationOptimizator(Dictionary<IdentificatorValue, VariableValue> additionalConstants)
        {
            this.additionalConstants = additionalConstants;
        }

        public ConstantPropagationOptimizator(){}

        public bool Optimize(BaseBlock baseBlock)
        {
            bool changed = false;
            IdentificatorValue identificatorValue;
            for (int i = 0; i < baseBlock.Commands.Count; i++)
            {
                if (baseBlock.Commands[i].Operation == Operation.Assign &&
                    baseBlock.Commands[i].LeftOperand is NumericValue)
                {
                    for (int j = i + 1; j < baseBlock.Commands.Count; j++)
                    {
                        if (baseBlock.Commands[j].Destination is IdentificatorValue &&
                            baseBlock.Commands[j].Destination.Value == baseBlock.Commands[i].Destination.Value)
                        {
                            break;
                        }

                        if (baseBlock.Commands[j].LeftOperand != null)
                        {
                            identificatorValue = baseBlock.Commands[j].LeftOperand as IdentificatorValue;
                            if (identificatorValue != null &&
                                identificatorValue.Value == baseBlock.Commands[i].Destination.Value)
                            {
                                baseBlock.Commands[j].LeftOperand = baseBlock.Commands[i].LeftOperand;
                                changed = true;
                            }
                        }

                        if (baseBlock.Commands[j].RightOperand != null)
                        {
                            identificatorValue = baseBlock.Commands[j].RightOperand as IdentificatorValue;
                            if (identificatorValue != null &&
                                identificatorValue.Value == baseBlock.Commands[i].Destination.Value)
                            {
                                baseBlock.Commands[j].RightOperand = baseBlock.Commands[i].LeftOperand;
                                changed = true;
                            }
                        }
                    }
                }

                if (additionalConstants.Count > 0) {
                    if (baseBlock.Commands[i].LeftOperand != null)
                    {
                        identificatorValue = baseBlock.Commands[i].LeftOperand as IdentificatorValue;
                        if ((identificatorValue != null && additionalConstants.ContainsKey(identificatorValue))){
                            baseBlock.Commands[i].LeftOperand = new NumericValue(additionalConstants[identificatorValue].constantValue);
                            changed = true;
                        }
                    }

                    if (baseBlock.Commands[i].RightOperand != null) 
                    {
                        identificatorValue = baseBlock.Commands[i].RightOperand as IdentificatorValue;
                        if ((identificatorValue != null && additionalConstants.ContainsKey(identificatorValue)))
                        {
                            baseBlock.Commands[i].RightOperand = new NumericValue(additionalConstants[identificatorValue].constantValue);
                            changed = true;
                        }
                    }
                }
                
            }
            return changed;
        }
    }
}
