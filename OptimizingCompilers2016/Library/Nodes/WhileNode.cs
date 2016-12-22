using OptimizingCompilers2016.Library.Visitors;

namespace OptimizingCompilers2016.Library.Nodes
{
    public class WhileNode : StatementNode
    {
        public ExprNode Condition { get; set; }
        public StatementNode Stat { get; set; }

        public WhileNode(ExprNode condition, StatementNode stat) 
        {
            Condition = condition;
            Stat = stat;
        }

        public override void Accept(IVisitor v) { v.Visit(this); }
    }
}