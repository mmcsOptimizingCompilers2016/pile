using OptimizingCompilers2016.Library.Visitors;

namespace OptimizingCompilers2016.Library.Nodes
{
    public class EmptyNode : StatementNode 
    {
        public override void Accept(IVisitor v) { /* do nothing */ }
    }
}