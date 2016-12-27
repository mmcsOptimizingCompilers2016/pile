using OptimizingCompilers2016.Library.ThreeAddressCode;
using OptimizingCompilers2016.Library.ThreeAddressCode.Values;
using OptimizingCompilers2016.Library.Analysis.ConstantPropagation;
using System.Collections.Generic;

namespace OptimizingCompilers2016.Library.Optimizators
{
    public class ConstantPropagationOptimizator : IOptimizator
    {
        private Dictionary<IdentificatorValue, int> additionalConstants = new Dictionary<IdentificatorValue, int>();
        public ConstantPropagationOptimizator(Dictionary<IdentificatorValue, int> additionalConstants)
        {
            this.additionalConstants = additionalConstants;
        }

        public ConstantPropagationOptimizator(){}

        private void saveConstant(IThreeAddressCode line)
        {
            if (additionalConstants.ContainsKey(line.Destination as IdentificatorValue))
            {
                additionalConstants[line.Destination as IdentificatorValue] = (line.LeftOperand as NumericValue).Value;
            }
            else
            {
                additionalConstants.Add(line.Destination as IdentificatorValue, (line.LeftOperand as NumericValue).Value);
            }
        }

        public bool Optimize(BaseBlock baseBlock)
        {
            bool changed = false;
            IdentificatorValue identificatorValue;
            for (int i = 0; i < baseBlock.Commands.Count; i++)
            {
                var line = baseBlock.Commands[i];
                if (line.Operation == Operation.Assign &&
                    line.LeftOperand is NumericValue)
                {
                    saveConstant(line);
                }

                if (additionalConstants.Count > 0) {
                    if (line.LeftOperand != null && line.LeftOperand is IdentificatorValue)
                    {
                        identificatorValue = line.LeftOperand as IdentificatorValue;
                        if (additionalConstants.ContainsKey(identificatorValue))
                        {
                            line.LeftOperand = new NumericValue(additionalConstants[identificatorValue]);
                            changed = true;
                        }
                    }

                    if (line.RightOperand != null && line.RightOperand is IdentificatorValue) 
                    {
                        identificatorValue = line.RightOperand as IdentificatorValue;
                        if (additionalConstants.ContainsKey(identificatorValue))
                        {
                            line.RightOperand = new NumericValue(additionalConstants[identificatorValue]);
                            changed = true;
                        }
                    }
                }

                if ((line.LeftOperand != null && line.LeftOperand is NumericValue) && (line.RightOperand != null && line.RightOperand is NumericValue))
                {
                    int x = (line.LeftOperand as NumericValue).Value;
                    int y = (line.RightOperand as NumericValue).Value;
                    int val = GlobalConstantPropagation.CalculateConstant(line.Operation, x, y);

                    line.Operation = Operation.Assign;
                    line.LeftOperand = new NumericValue(val);
                    line.RightOperand = null;

                    saveConstant(line);

                    changed = true;
                }
                
            }
            return changed;
        }
    }
}
