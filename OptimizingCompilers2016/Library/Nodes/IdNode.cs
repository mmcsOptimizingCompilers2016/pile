using OptimizingCompilers2016.Library.Visitors;

namespace OptimizingCompilers2016.Library.Nodes
{
    public class IdNode : ExprNode
    {
        public string Name { get; set; }

        public IdNode(string name) { Name = name; }

        public override void Accept(IVisitor v) { v.Visit(this); }
    }
}