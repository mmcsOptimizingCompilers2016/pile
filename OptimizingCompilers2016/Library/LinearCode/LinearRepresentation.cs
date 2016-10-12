using System;
using System.Collections.Generic;
using OptimizingCompilers2016.Library.ThreeAddressCode;
using OptimizingCompilers2016.Library.ThreeAddressCode.Values;

namespace OptimizingCompilers2016.Library.LinearCode
{
    public class LinearRepresentation: ThreeAddressCode.ThreeAddressCode
    {
        private static readonly Dictionary<Operation, String> s_opToStringDic = new Dictionary<Operation, String>
        {
            { Operation.NoOp, "nop" },
            { Operation.Assign, "{0} := {1}" },
            { Operation.Plus, "{0} := {1} + {2}" },
            { Operation.Minus, "{0} := {1} - {2}" },
            { Operation.Mult, "{0} := {1} * {2}" },
            { Operation.Div, "{0} := {1} / {2}" },
            { Operation.Less, "{0} := {1} < {2}" },
            { Operation.LessOrEq, "{0} := {1} <= {2}" },
            { Operation.Eq, "{0} := {1} == {2}" },
            { Operation.NotEq, "{0} := {1} != {2}" },
            { Operation.Great, "{0} := {1} > {2}" },
            { Operation.GreatOrEq, "{0} := {1} >= {2}" },
            { Operation.Goto, "goto {0}" },
            { Operation.CondGoto, "if {1} goto {0}" },
            { Operation.LabelOp, "{0}:" }
        };

        public LinearRepresentation(Operation operation, 
                                    LabelValue label,
                                    IdentificatorValue destination = null,
                                    IValue leftOperand = null, 
                                    IValue rightOperand = null)
        {
            Operation = operation;
            Destination = destination;
            LeftOperand = leftOperand;
            RightOperand = rightOperand;
            Label = label;
        }

        public override String ToString()
        {
            return String.Format(s_opToStringDic[Operation],
                Label == null ? "" : Label.ToString(),
                Destination == null ? "" : Destination.ToString(),
                LeftOperand == null ? "" : LeftOperand.ToString(),
                RightOperand == null ? "" : RightOperand.ToString());
        }
    }
}