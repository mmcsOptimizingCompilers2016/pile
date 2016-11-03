namespace OptimizingCompilers2016.Library.Optimizators
{
    using BaseBlock;
    public interface IOptimizator
    {
        void Optimize(BaseBlock baseBlock);
    }
}
