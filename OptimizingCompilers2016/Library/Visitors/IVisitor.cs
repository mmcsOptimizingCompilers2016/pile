using OptimizingCompilers2016.Library.Nodes;

namespace OptimizingCompilers2016.Library.Visitors
{
    public interface IVisitor
    {
        void Visit(IdNode id);
        void Visit(IntNumNode num);
        void Visit(BoolNode binop);
        void Visit(BinExprNode binop);
        void Visit(AssignNode assNode);
        void Visit(CycleNode cycNode);
        void Visit(BlockNode blNode);
        void Visit(IfNode ifNode);
        void Visit(ForNode forNode);
        void Visit(RepUntNode ruNode); 
        void Visit(WhileNode whNode); 
    }
}