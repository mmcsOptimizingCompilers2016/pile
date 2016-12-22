using System;
using System.Collections.Generic;
using System.Linq;
using OptimizingCompilers2016.Library.ThreeAddressCode.Values;
using OptimizingCompilers2016.Library.ThreeAddressCode;
using OptimizingCompilers2016.Library.LinearCode;
using OptimizingCompilers2016.Library.Nodes;

namespace OptimizingCompilers2016.Library.Visitors
{
    // NoOp: 
    // Assign: destination := leftOperand
    // Plus: destination := leftOperand + rightOperand
    // Minus, mult, div ...
    // goto destination
    // condGoto: if leftOperand goto destination

    public class LinearCodeVisitor : IVisitor
    {
        private static readonly string s_constantPrefix = "$t";
        private static readonly string s_labelPrefix = "%l";

        private static readonly Dictionary<BinSign, Operation> s_binSignToOpDic = new Dictionary<BinSign, Operation>
        {
            { BinSign.PLUS, Operation.Plus },
            { BinSign.MINUS, Operation.Minus },
            { BinSign.MULT, Operation.Mult },
            { BinSign.DIV, Operation.Div },
            { BinSign.LS, Operation.Less },
            { BinSign.LE, Operation.LessOrEq },
            { BinSign.EQ, Operation.Eq },
            { BinSign.NE, Operation.NotEq },
            { BinSign.GE, Operation.GreatOrEq },
            { BinSign.GT, Operation.Great },
        };

        private int valueCounter = 0;
        private int labelCounter = 0;

        private IValue idOrNum; // LinearCode saves result in this value

        public List<LinearRepresentation> code = new List<LinearRepresentation>();
        private List<LinearRepresentation> evaluatedExpression = new List<LinearRepresentation>();

        private static Operation binSignToOp(BinSign bs)
        {
            return s_binSignToOpDic[bs];
        }

        private void moveExpressionToCode()
        {
            code.AddRange(evaluatedExpression);
            evaluatedExpression.Clear();
        }

        // combines if and loop statements
        private void branchCondition(ExprNode condition, StatementNode trueBranch, StatementNode falseBranch,
            List<LinearRepresentation> addBeforeEndLabel = null)
        {
            condition.Accept(this);
            LabelValue trueCond = new LabelValue(s_labelPrefix + labelCounter++);
            LabelValue endCond = new LabelValue(s_labelPrefix + labelCounter++);

            LinearRepresentation gotoCond = new LinearRepresentation(Operation.CondGoto, trueCond, idOrNum);
            evaluatedExpression.Add(gotoCond);
            moveExpressionToCode();
            if (falseBranch != null)
            {
                falseBranch.Accept(this);
            }
            evaluatedExpression.Add(new LinearRepresentation(Operation.Goto, endCond));
            evaluatedExpression.Add(new LinearRepresentation(trueCond, Operation.NoOp));
            moveExpressionToCode();

            trueBranch.Accept(this);
            if (addBeforeEndLabel != null)
            {
                evaluatedExpression.AddRange(addBeforeEndLabel);
            }
            evaluatedExpression.Add(new LinearRepresentation(endCond, Operation.NoOp));
            moveExpressionToCode();
        }

        public void Visit(IdNode id)
        {
            idOrNum = new IdentificatorValue(id.Name);
        }
        public void Visit(IntNumNode num)
        {
            idOrNum = new NumericValue(num.Num);
        }
        public void Visit(BoolNode bNode)
        {
            idOrNum = new BooleanValue(bNode.Bool);
        }
        public void Visit(BinExprNode binop)
        {
            LinearRepresentation result = new LinearRepresentation(binSignToOp(binop.BinSign));
            binop.ExprLeft.Accept(this);
            result.LeftOperand = idOrNum;
            binop.ExprRight.Accept(this);
            result.RightOperand = idOrNum;
            var identificator = new IdentificatorValue(s_constantPrefix + valueCounter++.ToString());
            idOrNum = identificator;
            result.Destination = identificator;
            evaluatedExpression.Add(result);
        }
        public void Visit(AssignNode assNode)
        {
            if (assNode.AssOp != AssignType.Assign)
            {
                throw new NotImplementedException();
            }
            LinearRepresentation resultantAssign;
            assNode.Expr.Accept(this);
            if (evaluatedExpression.Any())
            {
                // remove last statement
                resultantAssign = evaluatedExpression.Last();
                evaluatedExpression.RemoveAt(evaluatedExpression.Count - 1);
                --valueCounter;
            }
            else
            {
                resultantAssign = new LinearRepresentation(Operation.Assign, null, idOrNum);
            }

            assNode.Id.Accept(this);
            resultantAssign.Destination = (IdentificatorValue)idOrNum;
            evaluatedExpression.Add(resultantAssign);

            moveExpressionToCode();
        }
        public void Visit(CycleNode cycNode)
        {
            // t1:=1
            var varIdent = new IdentificatorValue(s_constantPrefix + valueCounter++.ToString());
            code.Add(new LinearRepresentation(Operation.Assign, varIdent, new NumericValue(1)));

            // l1:  
            var conditionLabel = new LabelValue(s_labelPrefix + labelCounter++);
            code.Add(new LinearRepresentation(conditionLabel, Operation.NoOp));

            // t2 := t1 GT cycNode.Expr
            var varIdentNode = new IdNode(varIdent.ToString());
            var condition = new BinExprNode(varIdentNode, BinSign.LE, cycNode.Expr);

            // t1 := t1 + 1
            var beforeEnd = new List<LinearRepresentation>();
            beforeEnd.Add(new LinearRepresentation(Operation.Plus, varIdent, varIdent, new NumericValue(1)));
            beforeEnd.Add(new LinearRepresentation(Operation.Goto, conditionLabel));

            branchCondition(condition, cycNode.Stat, null, beforeEnd);
        }
        public void Visit(BlockNode blNode) 
        {
            for (var i = 0; i < blNode.StList.Count; ++i)
            {
                blNode.StList[i].Accept(this);
            }
        }
        public void Visit(IfNode ifNode)
        {
            branchCondition(ifNode.Condition, ifNode.TrueBranch, ifNode.ElseBranch);
        }
        public void Visit(ForNode forNode)
        {
            LabelValue beginLabel = new LabelValue(s_labelPrefix + labelCounter++);
            var beforeEnd = new List<LinearRepresentation>();
            forNode.LeftLimit.Id.Accept(this);
            beforeEnd.Add(new LinearRepresentation(Operation.Plus, 
                (IdentificatorValue)idOrNum, idOrNum, new NumericValue(1)));
            beforeEnd.Add(new LinearRepresentation(Operation.Goto, beginLabel));

            // for initialization
            forNode.LeftLimit.Accept(this);
            code.Add(new LinearRepresentation(beginLabel, Operation.NoOp));
            ExprNode condition = new BinExprNode(forNode.LeftLimit.Id, BinSign.LS, forNode.RightLimit);
            
            branchCondition(condition, forNode.BodyStatement, null, beforeEnd);
        }
        public void Visit(RepUntNode ruNode)
        {
            var beginLabel = new LabelValue(s_labelPrefix + labelCounter++);
            code.Add(new LinearRepresentation(beginLabel, Operation.NoOp));

            ruNode.StNode.Accept(this);
            ruNode.UntilExpr.Accept(this);
            // idOrNum ~ condition
            var gotoCond = new LinearRepresentation(Operation.CondGoto, beginLabel, idOrNum);
            evaluatedExpression.Add(gotoCond);
            moveExpressionToCode();
        }
        public void Visit(WhileNode whNode) 
        {
            LabelValue beginLabel = new LabelValue(s_labelPrefix + labelCounter++);
            code.Add(new LinearRepresentation(beginLabel, Operation.NoOp));
            var beforeEnd = new List<LinearRepresentation>();
            beforeEnd.Add(new LinearRepresentation(Operation.Goto, beginLabel));
            branchCondition(whNode.Condition, whNode.Stat, null, beforeEnd);
        }

        public override String ToString()
        {
            String text = "";
            foreach (LinearRepresentation lr in code)
            {
                text += lr.ToString() + Environment.NewLine;
            }
            return text;
        }
    }
}
