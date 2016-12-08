using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Occurrence = System.Tuple<int, OptimizingCompilers2016.Library.ThreeAddressCode.Values.IdentificatorValue>;
using OptimizingCompilers2016.Library.LinearCode;
using OptimizingCompilers2016.Library.ThreeAddressCode.Values;
using System.Collections;

namespace OptimizingCompilers2016.Library.Analysis
{
    /// <typeparam name="T">
    /// Множество, для которого ипользуется итерационный алгоритм
    /// (например: BitArray)
    /// </typeparam>
    public abstract class BaseIterationAlgorithm<T>
    {
        protected Dictionary<Tuple<BaseBlock, Occurrence>, int> occToBitNumber = new Dictionary<Tuple<BaseBlock, Occurrence>, int>();
        protected Dictionary<BaseBlock, T> generators = new Dictionary<BaseBlock, T>();
        protected Dictionary<BaseBlock, T> killers = new Dictionary<BaseBlock, T>();
        protected Dictionary<BaseBlock, InblockDefUse> localDefUses = new Dictionary<BaseBlock, InblockDefUse>();

        protected void fillSupportingStructures(List<BaseBlock> blocks)
        {
            int counter = 0;
            foreach (var block in blocks)
            {
                Console.WriteLine(block.Name + " : " + new InblockDefUse(block).ToString());
                localDefUses.Add(block, new InblockDefUse(block));
                for (int i = 0; i < block.Commands.Count; ++i)
                {
                    var line = block.Commands[i];
                    if (line.Destination is IdentificatorValue)
                    {
                        occToBitNumber.Add(new Tuple<BaseBlock, Tuple<int, IdentificatorValue>>(block, new Tuple<int, IdentificatorValue>(i, line.Destination as IdentificatorValue)), counter++);
                    }
                }
            }
        }

        protected abstract void fillGeneratorsAndKillers(List<BaseBlock> blocks);
        protected abstract T setStartingSet();
        protected abstract T substractSets(T firstSet, T secondSet);
        //TODO maybe T should implement IClonable
        protected abstract T cloneSet(T set);

        //maybe it should implement Semilattice interface
        public Dictionary<BaseBlock, T> iterationAlgorithm(List<BaseBlock> blocks, Func<T, T, T> collectFunction, Func<T, T, T> transferFunction)
        {
            Dictionary<BaseBlock, T> outs = new Dictionary<BaseBlock, T>();
            Dictionary<BaseBlock, T> ins = new Dictionary<BaseBlock, T>(); 
            foreach (var block in blocks)
            {
                outs.Add(block, setStartingSet());
                ins.Add(block, setStartingSet());
            }

            bool areDifferent = true;
            while (areDifferent)
            {
                areDifferent = false;
                foreach (var block in blocks)
                {
                    var predecessors = block.Predecessors;
                    foreach (var pred in predecessors)
                    {
                        ins[block] = collectFunction(ins[block], outs[pred]);
                    }

                    var prevOut = cloneSet(outs[block]);

                    outs[block] = transferFunction(generators[block], substractSets(ins[block], killers[block]));
                    if (prevOut.Equals(outs[block]))
                    {
                        areDifferent = areDifferent || false;
                    }
                }
            }
            return outs;
        }
    }
}
