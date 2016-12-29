using OptimizingCompilers2016.Library.ThreeAddressCode.Values;

namespace OptimizingCompilers2016.Library.Optimizators
{
    public class AlgebraicIdentityOptimizator : IOptimizator
    {
        public bool Optimize(BaseBlock baseBlock)
        {
            var changed = false;
            for (var i = 0; i < baseBlock.Commands.Count; ++i)
            {
                var command = baseBlock.Commands[i];
                if (command == null) continue;
                var leftValue = command.LeftOperand as NumericValue;
                var rightValue = command.RightOperand as NumericValue;
                if (command.LeftOperand != null && command.RightOperand != null)
                {
                    switch (command.Operation)
                    {
                        case ThreeAddressCode.Operation.Plus:
                            if (leftValue?.Value == 0)
                            {
                                command.LeftOperand = null;
                                command.Operation = ThreeAddressCode.Operation.Assign;
                                changed = true;
                            }
                            else if (rightValue?.Value == 0)
                            {
                                command.RightOperand = null;
                                command.Operation = ThreeAddressCode.Operation.Assign;
                                changed = true;
                            }
                            break;
                        case ThreeAddressCode.Operation.Minus:
                            if (leftValue?.Value == 0)
                            {
                                command.LeftOperand = null;
                                command.Operation = ThreeAddressCode.Operation.Assign;
                                command.RightOperand = new NumericValue(rightValue.Value * (-1));
                                changed = true;
                            }
                            else if (rightValue?.Value == 0)
                            {
                                command.RightOperand = null;
                                command.Operation = ThreeAddressCode.Operation.Assign;
                                changed = true;
                            }
                            else if (rightValue != null && leftValue != null && rightValue.Value == leftValue.Value)
                            {
                                command.RightOperand = null;
                                command.LeftOperand = new NumericValue(0);
                                command.Operation = ThreeAddressCode.Operation.Assign;
                                changed = true;
                            }
                            break;
                        case ThreeAddressCode.Operation.Mult:
                            if (leftValue?.Value == 1)
                            {
                                command.LeftOperand = null;
                                command.Operation = ThreeAddressCode.Operation.Assign;
                                changed = true;
                            }
                            else if (rightValue?.Value == 1)
                            {
                                command.RightOperand = null;
                                command.Operation = ThreeAddressCode.Operation.Assign;
                                changed = true;
                            }
                            else if (leftValue?.Value == 0)
                            {
                                command.RightOperand = new NumericValue(0);
                                command.Operation = ThreeAddressCode.Operation.Assign;
                                changed = true;
                            }
                            else if (rightValue?.Value == 0)
                            {
                                command.LeftOperand = new NumericValue(0);
                                command.Operation = ThreeAddressCode.Operation.Assign;
                                changed = true;
                            }
                            break;
                        case ThreeAddressCode.Operation.Div:
                            if (rightValue?.Value == 1)
                            {
                                command.RightOperand = null;
                                command.Operation = ThreeAddressCode.Operation.Assign;
                                changed = true;
                            }
                            break;
                    }
                }
            }
            return changed;
        }
    }
}
