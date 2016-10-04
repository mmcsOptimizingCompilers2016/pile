using OptimizingCompilers2016.Library.Visitors;

namespace OptimizingCompilers2016.Library.Nodes
{
    public class IntNumNode : ExprNode
    {
        public int Num { get; set; }

        public IntNumNode(int num) { Num = num; }

        public override void Accept(IVisitor v) { v.Visit(this); }
    }
}