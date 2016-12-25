using OptimizingCompilers2016.Library.Visitors;

namespace OptimizingCompilers2016.Library.Nodes.Base
{
    public abstract class BaseNode
    {
        public abstract void Accept(IVisitor v);
    }

    // auxiliary Node for code gen
}
