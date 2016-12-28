using System;
using System.Collections.Generic;
using Occurrence = System.Tuple<int, OptimizingCompilers2016.Library.ThreeAddressCode.Values.IdentificatorValue>;
using OptimizingCompilers2016.Library.ThreeAddressCode.Values;
using System.Collections;
using OptimizingCompilers2016.Library.Semilattice;
using OptimizingCompilers2016.Library;

namespace OptimizingCompilers2016.Library.Analysis
{
    public abstract class ImprovedBaseIterationAlgorithm<T> : BaseIterationAlgorithm<T>
        where T : ICloneable
    {
        private Dictionary<BaseBlock, List<BaseBlock>> domRelations;

        protected override abstract void FillGeneratorsAndKillers(List<BaseBlock> blocks);

        protected override abstract T SetStartingSet();

        public override abstract T Collect(T x, T y);

        protected override abstract T Transfer(T x, BaseBlock b);

        protected override void FillSupportingStructures(List<BaseBlock> blocks)
        {
            base.FillSupportingStructures(blocks);
        }

        //maybe it should implement Semilattice interface
        protected override void IterationAlgorithm(List<BaseBlock> blocks)
        {
            ControlFlowGraph cfg = new ControlFlowGraph(blocks);
            if (!cfg.CheckReducibility())
            {
                Console.WriteLine("CFG is not reduced!");
            }
            else
            {
                foreach (var block in blocks)
                {
                    outs.Add(block, SetStartingSet());
                    ins.Add(block, SetStartingSet());
                }

                bool areDifferent = true;
                int count = 0;

                domRelations = DOM.DOM_CREAT(blocks, blocks[0]);

                blocks.Sort((b1, b2) => CompareBlocks(b1, b2));
                Console.WriteLine("Sorted: ");
                foreach (var bl in blocks) {
                    Console.WriteLine(bl.Name);
                }

                while (areDifferent)
                {
                    count++;
                    areDifferent = false;

                    

                    foreach (var block in blocks)
                    {
                        var predecessors = block.Predecessors;
                        foreach (var pred in predecessors)
                        {
                            ins[block] = Collect(ins[block], outs[pred]);
                        }

                        var prevOut = outs[block].Clone();

                        outs[block] = Transfer(ins[block], block);
                        if (prevOut.Equals(outs[block]) && !areDifferent)
                            areDifferent = false;
                        else
                            areDifferent = true;
                    }
                }

                Console.WriteLine("COUNT OF ITERATIONS: " + count);
            }
        }

        private int CompareBlocks(BaseBlock b1, BaseBlock b2)
        {
            if (b1 == b2) return 0;
            return domRelations[b1].Contains(b2) ? 1 : -1;
        }

        public override abstract void RunAnalysis(List<BaseBlock> blocks);
    }
}
