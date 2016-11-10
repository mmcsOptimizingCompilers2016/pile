using OptimizingCompilers2016.Library.ThreeAddressCode;
using OptimizingCompilers2016.Library.ThreeAddressCode.Values;

namespace OptimizingCompilers2016.Library.Optimizators
{
    using BaseBlock;

    public class ConstantPropagationOptimizator : IOptimizator
    {
        public void Optimize(BaseBlock baseBlock)
        {
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

                        IdentificatorValue identificatorValue;
                        if (baseBlock.Commands[j].LeftOperand != null)
                        {
                            identificatorValue = baseBlock.Commands[j].LeftOperand as IdentificatorValue;
                            if (identificatorValue != null &&
                                identificatorValue.Value == baseBlock.Commands[i].Destination.Value)
                            {
                                baseBlock.Commands[j].LeftOperand = baseBlock.Commands[i].LeftOperand;
                            }
                        }

                        if (baseBlock.Commands[j].RightOperand != null)
                        {
                            identificatorValue = baseBlock.Commands[j].RightOperand as IdentificatorValue;
                            if (identificatorValue != null &&
                                identificatorValue.Value == baseBlock.Commands[i].Destination.Value)
                            {
                                baseBlock.Commands[j].RightOperand = baseBlock.Commands[i].LeftOperand;
                            }
                        }
                    }
                }
            }
        }
    }
}
