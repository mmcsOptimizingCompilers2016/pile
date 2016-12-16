using System;
using System.Collections;
using System.Collections.Generic;
using IntraOccurence = System.Tuple<OptimizingCompilers2016.Library.BaseBlock, System.Tuple<int, OptimizingCompilers2016.Library.ThreeAddressCode.Values.IdentificatorValue>>;
using Occurrence = System.Tuple<int, OptimizingCompilers2016.Library.ThreeAddressCode.Values.IdentificatorValue>;

namespace OptimizingCompilers2016.Library.Analysis
{
    public class AvailabilityAnalysis : BaseIterationAlgorithm<BitArray>
    {
        Dictionary<BaseBlock, InblockDefUse> localDefUses = new Dictionary<BaseBlock, InblockDefUse>();

        protected override void FillGeneratorsAndKillers(List<BaseBlock> blocks)
        {
            foreach (var block in blocks)
            {
                Console.WriteLine("Local def-uses: ");
                Console.WriteLine(block.Name + " : " + new InblockDefUse(block).ToString());
                localDefUses.Add(block, new InblockDefUse(block));
            }

            foreach (var block in blocks)
            {
                generators.Add(block, new BitArray(occToBitNumber.Count, false));
                killers.Add(block, new BitArray(occToBitNumber.Count, false));
                foreach (var ldur in localDefUses[block].defUses)
                {
                    generators[block].Set(occToBitNumber[new Tuple<BaseBlock, Occurrence>(block, ldur.Key)], true);
                    var variable = ldur.Key.Item2;
                    foreach (var e in occToBitNumber)
                    {
                        if (e.Key.Item2.Item2.Equals(ldur.Key.Item2))
                        {
                            killers[block].Set(occToBitNumber[e.Key], true);
                        }
                    }
                }
                Console.WriteLine(block.Name);
                Console.WriteLine(" Gen: " + BitArrayToString(generators[block]));
                PrintKillOfGen(generators[block]);
                Console.WriteLine("Kill: " + BitArrayToString(killers[block]));
            }
        }

        protected override BitArray SetStartingSet()
        {
            return new BitArray(occToBitNumber.Count, false);
        }

        protected override BitArray SubstractSets(BitArray firstSet, BitArray secondSet)
        {
            return firstSet.And(secondSet.Not());
        }
        private String BitArrayToString(BitArray arr)
        {
            String fullString = "";
            foreach (var bit in arr)
            {
                fullString += bit.ToString() == "True" ? "1" : "0";
            }
            return fullString;
        }

        private void PrintKillOfGen(BitArray something)
        {
            foreach (var e in occToBitNumber)
            {
                if (something.Get(e.Value))
                {
                    Console.Write(e.Key.Item2.Item2.ToString() + ", ");
                }
            }
            Console.WriteLine();
        }

        public override BitArray Collect(BitArray x, BitArray y)
        {
            return x.Or(y);
        }
          
        private BitArray transferFunction(BitArray x, BitArray y)
        {
            return x.Or(y);
        }

        private Dictionary<BaseBlock, BitArray> iterationAlgorithm(List<BaseBlock> blocks)
        {
            base.IterationAlgorithm(blocks, transferFunction);
            return base.outs;
        }

        public void runAnalys(List<BaseBlock> blocks)
        {
            FillSupportingStructures(blocks);
            FillGeneratorsAndKillers(blocks);
            outs = iterationAlgorithm(blocks);
        }
        public Dictionary<IntraOccurence, HashSet<IntraOccurence>> getDefUses()
        {
            Dictionary<IntraOccurence, HashSet<IntraOccurence>> defUses = new Dictionary<IntraOccurence, HashSet<IntraOccurence>>();
            foreach (var res in outs)
            {
                Console.WriteLine(res.Key.Name + " outs: ");
                PrintKillOfGen(res.Value);
            }

            return null;
        }

        public Dictionary<IntraOccurence, HashSet<IntraOccurence>> getUseDefs()
        {
            return null;
        }
    }
}
