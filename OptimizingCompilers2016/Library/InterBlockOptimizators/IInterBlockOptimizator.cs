namespace OptimizingCompilers2016.Library.InterBlockOptimizators
{
    public interface IInterBlockOptimizator
    {
        bool Optimize(ControlFlowGraph Blocks);
    }
}
