using System;
using System.Collections.Generic;
using OptimizingCompilers2016.Library.ThreeAddressCode.Values;
using System.Collections;
using OptimizingCompilers2016.Library.Semilattice;
using OptimizingCompilers2016.Library.Analysis.DefUse;

using Occurrence = System.Tuple<int, OptimizingCompilers2016.Library.ThreeAddressCode.Values.IdentificatorValue>;
using IntraOccurence = System.Tuple<OptimizingCompilers2016.Library.BaseBlock, System.Tuple<int, OptimizingCompilers2016.Library.ThreeAddressCode.Values.IdentificatorValue>>;

namespace OptimizingCompilers2016.Library.Analysis
{
    /// <typeparam name="T">
    /// Множество, для которого ипользуется итерационный алгоритм
    /// (например: BitArray)
    /// </typeparam>
    public abstract class BaseIterationAlgorithm<T> : Semilattice<T>
           where T : ICloneable
    {
        protected Dictionary<IntraOccurence, int> occToBitNumber = new Dictionary<IntraOccurence, int>();
       
        protected Dictionary<BaseBlock, T> outs = new Dictionary<BaseBlock, T>();
        protected Dictionary<BaseBlock, T> ins = new Dictionary<BaseBlock, T>();

        protected Dictionary<BaseBlock, T> generators = new Dictionary<BaseBlock, T>();
        protected Dictionary<BaseBlock, T> killers = new Dictionary<BaseBlock, T>();

        protected abstract void FillGeneratorsAndKillers(List<BaseBlock> blocks);

        protected abstract T SetStartingSet();

        public abstract T Collect(T x, T y);

        protected abstract T Transfer(T x, BaseBlock b);

        private void addOccurenceFromOperand(BaseBlock block, Object operand, int lineN)
        {
            if (operand is IdentificatorValue)
            {
                var occ = new IntraOccurence(block, new Occurrence(lineN, operand as IdentificatorValue));
                if (!occToBitNumber.ContainsKey(occ))
                {
                    occToBitNumber.Add(occ, occToBitNumber.Count);
                }
            }
        }

        protected virtual void FillSupportingStructures(List<BaseBlock> blocks)
        {
            foreach (var block in blocks)
            {
                for (int i = 0; i < block.Commands.Count; ++i)
                {
                    var line = block.Commands[i];
                    addOccurenceFromOperand(block, line.Destination, i);
                    addOccurenceFromOperand(block, line.LeftOperand, i);
                    addOccurenceFromOperand(block, line.RightOperand, i);
                }
            }
        }

        //maybe it should implement Semilattice interface
        protected virtual void IterationAlgorithm(List<BaseBlock> blocks)
        {
            foreach (var block in blocks)
            {
                outs.Add(block, SetStartingSet());
                ins.Add(block, SetStartingSet());
            }

            bool areDifferent = true;
            int count = 0;
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
                    //outs[block] = transferFunction(generators[block], SubstractSets(ins[block], killers[block]));
                    if (prevOut.Equals(outs[block]) && !areDifferent)
                        areDifferent = false;
                    else
                        areDifferent = true;
                }
            }
            Console.WriteLine("COUNT OF ITERATIONS: " + count);
        }
        public abstract void RunAnalysis(List<BaseBlock> blocks);

    }
}
