using OptimizingCompilers2016.Library.Visitors;

namespace OptimizingCompilers2016.Library.Nodes
{
    public class ForNode : StatementNode 
    {
        public AssignNode LeftLimit { get; set; }
        public ExprNode RightLimit { get; set; }
        public StatementNode BodyStatement { get; set; }

        public ForNode(AssignNode leftLimit, ExprNode rightLimit, StatementNode doStat ) 
        {
            LeftLimit = leftLimit;
            RightLimit = rightLimit;
            BodyStatement = doStat;
        }

        public override void Accept(IVisitor v) { v.Visit(this); }
    }
}