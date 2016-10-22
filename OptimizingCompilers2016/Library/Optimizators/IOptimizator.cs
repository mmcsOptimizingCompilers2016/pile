namespace OptimizingCompilers2016.Library.Optimizators
{
    public interface IOptimizator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseBlock"></param>
        /// <returns>Returns true if BaseBlock was optimized, false otherwise</returns>
        bool Optimize(BaseBlock baseBlock);
    }
}
