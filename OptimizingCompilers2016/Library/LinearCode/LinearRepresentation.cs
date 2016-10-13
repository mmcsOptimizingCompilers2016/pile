using System;
using System.Collections.Generic;
using OptimizingCompilers2016.Library.ThreeAddressCode;
using OptimizingCompilers2016.Library.ThreeAddressCode.Values;

namespace OptimizingCompilers2016.Library.LinearCode
{
    public class LinearRepresentation: ThreeAddressCode.ThreeAddressCode
    {
        private static readonly int s_labelIntentSize = 10;
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
            { Operation.Goto, "goto {1}" },
            { Operation.CondGoto, "if {2} goto {1}" }
        };

        private string labelIntent(string label)
        {
            string auxil = new string(' ', Math.Max(0, s_labelIntentSize - label.Length) - 1);
            string result = label + ':' + auxil;
            return result;
        }

        public LinearRepresentation(LabelValue label,
                                    Operation operation,
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
        public LinearRepresentation(Operation operation,
                                    IdentificatorValue destination = null,
                                    IValue leftOperand = null,
                                    IValue rightOperand = null)
            : this(null, operation, destination, leftOperand, rightOperand)
        { }

        public override String ToString()
        {
            string labelIntentString = Label == null ?
                new string(' ', s_labelIntentSize) :
                labelIntent((string)Label.Value);
            return labelIntentString + String.Format(s_opToStringDic[Operation],
                Destination == null ? "" : Destination.ToString(),
                LeftOperand == null ? "" : LeftOperand.ToString(),
                RightOperand == null ? "" : RightOperand.ToString());
        }
    }
}