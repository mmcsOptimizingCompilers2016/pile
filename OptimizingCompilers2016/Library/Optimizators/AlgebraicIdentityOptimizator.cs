using OptimizingCompilers2016.Library.ThreeAddressCode.Values;

namespace OptimizingCompilers2016.Library.Optimizators
{
    public class AlgebraicIdentityOptimizator
    {
        public void Optimize(BaseBlock.BaseBlock block)
        {
            for (var i = 0; i < block.Commands.Count; ++i)
            {
                var command = block.Commands[i];
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
                            }
                            else if (rightValue?.Value == 0)
                            {
                                command.RightOperand = null;
                                command.Operation = ThreeAddressCode.Operation.Assign;
                            }
                            break;
                        case ThreeAddressCode.Operation.Minus:
                            if (leftValue?.Value == 0)
                            {
                                command.LeftOperand = null;
                                command.Operation = ThreeAddressCode.Operation.Assign;
                                command.RightOperand = new NumericValue(rightValue.Value * (-1));
                            }
                            else if (rightValue?.Value == 0)
                            {
                                command.RightOperand = null;
                                command.Operation = ThreeAddressCode.Operation.Assign;
                            }
                            else if (rightValue?.Value == leftValue?.Value)
                            {
                                command.RightOperand = null;
                                command.LeftOperand = new NumericValue(0);
                                command.Operation = ThreeAddressCode.Operation.Assign;
                            }
                            break;
                        case ThreeAddressCode.Operation.Mult:
                            if (leftValue?.Value == 1)
                            {
                                command.LeftOperand = null;
                                command.Operation = ThreeAddressCode.Operation.Assign;
                            }
                            else if (rightValue?.Value == 1)
                            {
                                command.RightOperand = null;
                                command.Operation = ThreeAddressCode.Operation.Assign;
                            }
                            break;
                        case ThreeAddressCode.Operation.Div:
                            if (rightValue?.Value == 1)
                            {
                                command.RightOperand = null;
                                command.Operation = ThreeAddressCode.Operation.Assign;
                            }
                            break;
                    }
                }

            }
        }
    }
}
