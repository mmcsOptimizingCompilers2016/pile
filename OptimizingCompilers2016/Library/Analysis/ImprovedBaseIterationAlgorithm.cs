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
        private List<DOM.link> directDominators;

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
                directDominators = DOM.get_direct_dominators(domRelations, blocks[0]);

                foreach (var link in directDominators) {
                    Console.WriteLine(link.ToString());
                }

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
            if (b1.Name == b2.Name)
            {
                return 0;
            }

            BaseBlock directDom;
            var dd = directDominators.Find(x => x.root.Name == b1.Name);
            if (dd == null)
            {
                var dd2 = directDominators.Find(x => x.root.Name == b2.Name);
                if (dd2 == null)
                {
                    return 0;
                }
                else {
                    return -1;
                }
            }
                
            directDom = dd.root;

            
                var dch = directDominators.Find(x => x.child.Name == directDom.Name);
                if (dch != null)
                {
                    var rootDom = dch.root;
                    return CompareBlocks(rootDom, b2);
                }
                else {
                    return 1;
                }

        }

        public override abstract void RunAnalysis(List<BaseBlock> blocks);
    }
}
