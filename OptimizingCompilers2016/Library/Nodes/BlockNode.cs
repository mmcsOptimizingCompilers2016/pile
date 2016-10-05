using System.Collections.Generic;
using OptimizingCompilers2016.Library.Visitors;

namespace OptimizingCompilers2016.Library.Nodes
{
    public class BlockNode : StatementNode
    {
        public List<StatementNode> StList = new List<StatementNode>();

        public BlockNode(StatementNode stat)
        {
            Add(stat);
        }

        public BlockNode(List<StatementNode> statList)
        {
            StList = statList;
        }

        public void Add(StatementNode stat)
        {
            StList.Add(stat);
        }

        public override void Accept(IVisitor v) { v.Visit(this); }
    }
}