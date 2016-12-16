using System.Collections;
using System.Collections.Generic;

namespace OptimizingCompilers2016.Library.Analysis
{
    public class AvailabilityAnalysis : BaseIterationAlgorithm<BitArray>
    {
        protected override void FillGeneratorsAndKillers(List<BaseBlock> blocks)
        {

        }

        protected override BitArray SetStartingSet()
        {
        }

        protected override BitArray SubstractSets(BitArray firstSet, BitArray secondSet)
        {
        }
    }
}
