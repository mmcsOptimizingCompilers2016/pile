using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace SimpleLang
{
    public enum AssignType { Assign, AssignPlus, AssignMinus, AssignMult, AssignDivide };
    
    public enum BinSign
    {
        [Description("<")]
        LS,
        [Description(">")]
        GT,
        [Description("<=")]
        LE,
        [Description(">=")]
        GE,
        [Description("==")]
        EQ,
        [Description("!=")]
        NE,
        [Description("+")]
        PLUS,
        [Description("-")]
        MINUS,
        [Description("*")]
        MULT,
        [Description("/")]
        DIV
    }

    public abstract class Node
    {
        public abstract void Accept(Visitor v);
    }

    public abstract class ExprNode : Node 
    {
    }

    public class IdNode : ExprNode
    {
        public string Name { get; set; }

        public IdNode(string name) { Name = name; }

        public override void Accept(Visitor v) { v.Visit(this); }
    }

    public class IntNumNode : ExprNode
    {
        public int Num { get; set; }

        public IntNumNode(int num) { Num = num; }

        public override void Accept(Visitor v) { v.Visit(this); }
    }

    public class BoolNode : ExprNode
    {
        public bool Bool { get; set; }

        public BoolNode(bool flag) { Bool = flag; }

        public override void Accept(Visitor v) { v.Visit(this); }
    }

    public class BinExprNode : ExprNode
    {
        public ExprNode ExprLeft { get; set; }
        public BinSign BinSign { get; set; }
        public ExprNode ExprRight { get; set; }

        public BinExprNode(ExprNode binExpr, BinSign binSign, ExprNode expr)
        {
            ExprLeft = binExpr;
            BinSign = binSign;
            ExprRight = expr;
        }

        public override void Accept(Visitor v) { v.Visit(this); }
    }

    public abstract class StatementNode : Node
    { }

    // auxiliary Node for code gen
    public class EmptyNode : StatementNode 
    {
        public override void Accept(Visitor v) { /* do nothing */ }
    }

    public class AssignNode : StatementNode
    {
        public IdNode Id { get; set; }
        public ExprNode Expr { get; set; }
        public AssignType AssOp { get; set; }

        public AssignNode(IdNode id, ExprNode expr, AssignType assop = AssignType.Assign)
        {
            Id = id;
            Expr = expr;
            AssOp = assop;
        }

        public override void Accept(Visitor v) { v.Visit(this); }
    }

    public class CycleNode : StatementNode
    {
        public ExprNode Expr { get; set; }
        public StatementNode Stat { get; set; }

        public CycleNode(ExprNode expr, StatementNode stat)
        {
            Expr = expr;
            Stat = stat;
        }

        public override void Accept(Visitor v) { v.Visit(this); }
    }

    public class BlockNode : StatementNode
    {
        public List<StatementNode> StList = new List<StatementNode>();

        public BlockNode(StatementNode stat)
        {
            Add(stat);
        }

        public BlockNode(List<StatementNode> statList)
        {
            StList = statList;
        }

        public void Add(StatementNode stat)
        {
            StList.Add(stat);
        }

        public override void Accept(Visitor v) { v.Visit(this); }
    }

    public class IfNode : StatementNode
    {
        public ExprNode Condition { get; set; }
        public StatementNode TrueBranch { get; set; }
        public StatementNode ElseBranch { get; set; }

        public IfNode(ExprNode condition, StatementNode trueBranch) 
        {
            Condition = condition;
            TrueBranch = trueBranch;
            ElseBranch = null;
        }

        public IfNode(ExprNode condition, StatementNode trueBranch, StatementNode elseStatement)
            : this (condition, trueBranch)
        {
            ElseBranch = elseStatement;
        }

        public override void Accept(Visitor v) { v.Visit(this); }
    }

    public class ForNode : StatementNode 
    {
        public AssignNode LeftLimit { get; set; }
        public ExprNode RightLimit { get; set; }
        public StatementNode BodyStatement { get; set; }

        public ForNode(AssignNode leftLimit, ExprNode rightLimit, StatementNode doStat ) 
        {
            LeftLimit = leftLimit;
            RightLimit = rightLimit;
            BodyStatement = doStat;
        }

        public override void Accept(Visitor v) { v.Visit(this); }
    }

    public class RepUntNode : StatementNode 
    {
        public StatementNode StNode;
        public ExprNode UntilExpr { get; set; }
        
        public RepUntNode(StatementNode stList, BinExprNode untilExpr)
        {
            StNode = stList;
            UntilExpr = untilExpr;
        }

        public override void Accept(Visitor v) { v.Visit(this); }
    }

    public class WhileNode : StatementNode
    {
        public ExprNode Condition { get; set; }
        public StatementNode Stat { get; set; }

        public WhileNode(ExprNode condition, StatementNode stat) 
        {
            Condition = condition;
            Stat = stat;
        }

        public override void Accept(Visitor v) { v.Visit(this); }
    }
}
