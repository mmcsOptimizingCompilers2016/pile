using OptimizingCompilers2016.Library.ThreeAddressCode.Values;
using System;
using System.Collections.Generic;

namespace OptimizingCompilers2016.Library.Transformations
{
    public class ConstantFolding
    {
        public static List<BaseBlock> transform(List<BaseBlock> blocks)
        {
            foreach (var block in blocks)
                ConstantFolding.transform(block);
            return blocks;
        }

        public static void transform(BaseBlock block)
        {
            var codeForTransform = block.Commands;
            for (int i = 0; i < codeForTransform.Count; i++)
            {
                var temp = codeForTransform[i];
                if (temp.LeftOperand?.Value is int && temp.RightOperand?.Value is int)
                {
                    switch (temp.Operation)
                    {
                        case ThreeAddressCode.Operation.Plus:
                            {
                                int res = Convert.ToInt32(temp.LeftOperand.Value) + Convert.ToInt32(temp.RightOperand.Value);
                                temp.LeftOperand = new NumericValue(res);
                                temp.Operation = ThreeAddressCode.Operation.Assign;
                                temp.RightOperand = null;
                                break;
                            }
                        case ThreeAddressCode.Operation.Minus:
                            {
                                int res = Convert.ToInt32(temp.LeftOperand.Value) - Convert.ToInt32(temp.RightOperand.Value);
                                temp.LeftOperand = new NumericValue(res);
                                temp.Operation = ThreeAddressCode.Operation.Assign;
                                temp.RightOperand = null;
                                break;
                            }
                        case ThreeAddressCode.Operation.Mult:
                            {
                                int res = Convert.ToInt32(temp.LeftOperand.Value) * Convert.ToInt32(temp.RightOperand.Value);
                                temp.LeftOperand = new NumericValue(res);
                                temp.Operation = ThreeAddressCode.Operation.Assign;
                                temp.RightOperand = null;
                                break;
                            }
                        case ThreeAddressCode.Operation.Div:
                            {
                                int res = Convert.ToInt32(temp.LeftOperand.Value) / Convert.ToInt32(temp.RightOperand.Value);
                                temp.LeftOperand = new NumericValue(res);
                                temp.Operation = ThreeAddressCode.Operation.Assign;
                                temp.RightOperand = null;
                                break;
                            }
                        default: break;
                    }
                    codeForTransform[i] = temp;
                }
            }
        }
    }
}
