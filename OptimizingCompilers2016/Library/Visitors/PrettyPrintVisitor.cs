﻿using System;
using OptimizingCompilers2016.Library.Helpers;
using OptimizingCompilers2016.Library.Nodes;

namespace OptimizingCompilers2016.Library.Visitors
{
    public class PrettyPrintVisitor : IVisitor
    {
        public string Text = "";
        private int Indent = 0;

        private string IndentStr() { return new string(' ', Indent); }
        private void IndentPlus() { Indent += 2; }
        private void IndentMinus() { Indent -= 2; }
        public void Visit(IdNode id) { Text += id.Name; }
        public void Visit(IntNumNode num) { Text += num.Num.ToString(); }
        public void Visit(BoolNode binop) { Text += binop.Bool.ToString(); }

        public void Visit(BinExprNode binop)
        {
            Text += "(";
            binop.ExprLeft.Accept(this);
            Text += " " + binop.BinSign.Description() + " ";
            binop.ExprRight.Accept(this);
            Text += ")";
        }

        public void Visit(AssignNode a)
        {
            Text += IndentStr();
            a.Id.Accept(this);
            Text += " = ";
            a.Expr.Accept(this);
            Text += ";";
        }

        public void Visit(CycleNode c)
        {
            Text += Environment.NewLine + IndentStr() + "cycle ";
            c.Expr.Accept(this);
            Text += Environment.NewLine;
            c.Stat.Accept(this);
        }

        public void Visit(BlockNode bl)
        {
            Text += IndentStr() + "{" + Environment.NewLine;
            IndentPlus();

            var Count = bl.StList.Count;

            if (Count > 0)
                bl.StList[0].Accept(this);
            for (var i = 1; i < Count; i++)
            {
                if (!(bl.StList[i] is EmptyNode))
                    Text += Environment.NewLine;
                
                bl.StList[i].Accept(this);
            }
            IndentMinus();
            Text += Environment.NewLine + IndentStr() + "}";
        }

        public void Visit(IfNode iNode)
        {
            Text += Environment.NewLine + IndentStr() + "if ";
            iNode.Condition.Accept(this);
            Text += " " + Environment.NewLine;
            IndentPlus();
            iNode.TrueBranch.Accept(this);
            IndentMinus();
            if (iNode.ElseBranch != null)
            {
                Text += Environment.NewLine + IndentStr() + "else" + Environment.NewLine;
                IndentPlus();
                iNode.ElseBranch.Accept(this);
                IndentMinus();
            }
        }

        public void Visit(ForNode forNode)
        {
            Text += Environment.NewLine + IndentStr() + "for";
            forNode.LeftLimit.Accept(this);
            Text += " to ";
            forNode.RightLimit.Accept(this);

            Text += Environment.NewLine;
            IndentPlus();
            forNode.BodyStatement.Accept(this);
            IndentMinus();

        }

        public void Visit(RepUntNode ruNode)
        {
            Text += Environment.NewLine + IndentStr() + "repeat" + Environment.NewLine;
            IndentPlus();
            ruNode.StNode.Accept(this);
            IndentMinus();
            Text += Environment.NewLine + IndentStr() + "until ";
            ruNode.UntilExpr.Accept(this);
        }

        public void Visit(WhileNode whNode)
        {
            Text += Environment.NewLine + IndentStr() + "while ";
            whNode.Condition.Accept(this);

            Text += Environment.NewLine;
            IndentPlus();
            whNode.Stat.Accept(this);
            IndentMinus();

        }
    }

}
