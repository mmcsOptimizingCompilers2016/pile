using OptimizingCompilers2016.Library.Visitors;

namespace OptimizingCompilers2016.Library.Nodes
{
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

        public override void Accept(IVisitor v) { v.Visit(this); }
    }
}