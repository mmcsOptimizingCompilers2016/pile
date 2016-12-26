using System;
using System.Collections.Generic;
using Occurrence = System.Tuple<int, OptimizingCompilers2016.Library.ThreeAddressCode.Values.IdentificatorValue>;
using OptimizingCompilers2016.Library.ThreeAddressCode.Values;
using System.Collections;
using OptimizingCompilers2016.Library.Semilattice;
using OptimizingCompilers2016.Library.Analysis.DefUse;

namespace OptimizingCompilers2016.Library.Analysis
{
    /// <typeparam name="T">
    /// Множество, для которого ипользуется итерационный алгоритм
    /// (например: BitArray)
    /// </typeparam>
    public abstract class BaseIterationAlgorithm<T> : Semilattice<T>
           where T : ICloneable
    {
        protected Dictionary<Tuple<BaseBlock, Occurrence>, int> occToBitNumber = new Dictionary<Tuple<BaseBlock, Occurrence>, int>();
       
        protected Dictionary<BaseBlock, T> outs = new Dictionary<BaseBlock, T>();
        protected Dictionary<BaseBlock, T> ins = new Dictionary<BaseBlock, T>();

        protected Dictionary<BaseBlock, InblockDefUse> localDefUses = new Dictionary<BaseBlock, InblockDefUse>();
        protected Dictionary<BaseBlock, T> generators = new Dictionary<BaseBlock, T>();
        protected Dictionary<BaseBlock, T> killers = new Dictionary<BaseBlock, T>();

        protected abstract void FillGeneratorsAndKillers(List<BaseBlock> blocks);

        protected abstract T SetStartingSet();

        public abstract T Collect(T x, T y);

        protected abstract T Transfer(T x, BaseBlock b);

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
                        occToBitNumber.Add(new Tuple<BaseBlock, Tuple<int, IdentificatorValue>>(
                            block,
                            new Tuple<int, IdentificatorValue>(i, line.Destination as IdentificatorValue)),
                            counter++);
                    }
                }
            }
        }

        //maybe it should implement Semilattice interface
        protected void IterationAlgorithm(List<BaseBlock> blocks)
        {
            foreach (var block in blocks)
            {
                outs.Add(block, SetStartingSet());
                ins.Add(block, SetStartingSet());
            }

            bool areDifferent = true;
            int count = 0;
            String lines = "";
            while (areDifferent)
            {
                lines += "##########################";
                lines += "Iteration " + count + "\n";
                foreach (var bl in blocks) {
                    lines += bl.Name + "\n";
                    lines += "In: " + ins[bl].ToString() + "\n";
                    lines += "Out: " + outs[bl].ToString() + "\n";
                }
                lines += "\n\n\n";

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

            System.IO.StreamWriter file = new System.IO.StreamWriter("iterations.txt");
            file.WriteLine(lines);
            file.Close();


            Console.WriteLine("COUNT OF ITERATIONS: " + count);
        }
        public abstract void RunAnalysis(List<BaseBlock> blocks);

    }
}
