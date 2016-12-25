using OptimizingCompilers2016.Library.Visitors;

namespace OptimizingCompilers2016.Library.Nodes
{
    public class RepUntNode : StatementNode 
    {
        public StatementNode StNode;
        public ExprNode UntilExpr { get; set; }
        
        public RepUntNode(StatementNode stList, BinExprNode untilExpr)
        {
            StNode = stList;
            UntilExpr = untilExpr;
        }

        public override void Accept(IVisitor v) { v.Visit(this); }
    }
}