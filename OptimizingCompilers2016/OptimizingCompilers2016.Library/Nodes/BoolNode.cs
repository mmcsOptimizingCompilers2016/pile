using OptimizingCompilers2016.Library.Visitors;

namespace OptimizingCompilers2016.Library.Nodes
{
    public class BoolNode : ExprNode
    {
        public bool Bool { get; set; }

        public BoolNode(bool flag) { Bool = flag; }

        public override void Accept(IVisitor v) { v.Visit(this); }
    }
}