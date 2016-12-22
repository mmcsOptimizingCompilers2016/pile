using OptimizingCompilers2016.Library.Visitors;

namespace OptimizingCompilers2016.Library.Nodes
{
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

        public override void Accept(IVisitor v) { v.Visit(this); }
    }
}