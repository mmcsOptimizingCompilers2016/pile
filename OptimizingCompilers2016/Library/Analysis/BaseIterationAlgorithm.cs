using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Occurrence = System.Tuple<int, OptimizingCompilers2016.Library.ThreeAddressCode.Values.IdentificatorValue>;
using OptimizingCompilers2016.Library.LinearCode;
using OptimizingCompilers2016.Library.ThreeAddressCode.Values;
using System.Collections;
using OptimizingCompilers2016.Library.Semilattice;

namespace OptimizingCompilers2016.Library.Analysis
{
    /// <typeparam name="T">
    /// Множество, для которого ипользуется итерационный алгоритм
    /// (например: BitArray)
    /// </typeparam>
    public abstract class BaseIterationAlgorithm<T> : Semilattice<T>
        where T : IEnumerable, ICloneable
    {
        protected Dictionary<Tuple<BaseBlock, Occurrence>, int> occToBitNumber = new Dictionary<Tuple<BaseBlock, Occurrence>, int>();
        protected Dictionary<BaseBlock, T> generators = new Dictionary<BaseBlock, T>();
        protected Dictionary<BaseBlock, T> killers = new Dictionary<BaseBlock, T>();

        protected Dictionary<BaseBlock, T> outs = new Dictionary<BaseBlock, T>();
        protected Dictionary<BaseBlock, T> ins = new Dictionary<BaseBlock, T>();

        protected void FillSupportingStructures(List<BaseBlock> blocks)
        {
            int counter = 0;
            foreach (var block in blocks)
            {
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

        protected abstract void FillGeneratorsAndKillers(List<BaseBlock> blocks);
        protected abstract T SetStartingSet();
        protected abstract T SubstractSets(T firstSet, T secondSet);

        public virtual T Collect(T x, T y) { return SetStartingSet(); }

        //protected abstract T cloneSet(T set);

        //maybe it should implement Semilattice interface
        public void IterationAlgorithm(List<BaseBlock> blocks, Func<T, T, T> transferFunction)
        {
            foreach (var block in blocks)
            {
                outs.Add(block, SetStartingSet());
                ins.Add(block, SetStartingSet());
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
                        ins[block] = Collect(ins[block], outs[pred]);
                    }

                    var prevOut = outs[block].Clone();
                    //var prevOut = cloneSet(outs[block]);

                    outs[block] = transferFunction(generators[block], SubstractSets(ins[block], killers[block]));
                    if (prevOut.Equals(outs[block]))
                    {
                        areDifferent = areDifferent || false;
                    }
                }
            }
        }
    }
}
