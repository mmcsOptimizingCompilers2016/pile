using System;
using System.Collections.Generic;
using OptimizingCompilers2016.Library.LinearCode.Base;

namespace OptimizingCompilers2016.Library.LinearCode
{
    public struct LinearRepresentation
    {
        private static readonly Dictionary<Operation, string> s_opToStringDic = new Dictionary<Operation, string>
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

        public Operation operation;
        public InstructionTerm destination;
        public Value leftOperand;
        public Value rightOperand;

        public LinearRepresentation(Operation op, InstructionTerm dst = null,
            Value lOp = null, Value rOp = null)
        {
            operation = op;
            destination = dst;
            leftOperand = lOp;
            rightOperand = rOp;
        }

        public String ToString()
        {
            return String.Format(s_opToStringDic[operation],
                destination == null ? "" : destination.ToString(),
                leftOperand == null ? "" : leftOperand.ToString(),
                rightOperand == null ? "" : rightOperand.ToString());
        }
    }
}