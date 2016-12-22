using OptimizingCompilers2016.Library.Visitors;

namespace OptimizingCompilers2016.Library.Nodes
{
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

        public override void Accept(IVisitor v) { v.Visit(this); }
    }
}