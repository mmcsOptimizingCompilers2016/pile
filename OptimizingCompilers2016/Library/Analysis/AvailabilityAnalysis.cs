using OptimizingCompilers2016.Library.Analysis.DefUse;
using System;
using System.Collections;
using System.Collections.Generic;
using IntraOccurence = System.Tuple<OptimizingCompilers2016.Library.BaseBlock, System.Tuple<int, OptimizingCompilers2016.Library.ThreeAddressCode.Values.IdentificatorValue>>;
using Occurrence = System.Tuple<int, OptimizingCompilers2016.Library.ThreeAddressCode.Values.IdentificatorValue>;

namespace OptimizingCompilers2016.Library.Analysis
{
    public class AvailabilityAnalysis : BaseIterationAlgorithm<BitArray>
    {
        protected override void FillGeneratorsAndKillers(List<BaseBlock> blocks)
        {
            foreach (var block in blocks)
                localDefUses.Add(block, new InblockDefUse(block));

            foreach (var block in blocks)
            {
                generators.Add(block, new BitArray(occToBitNumber.Count, false));
                killers.Add(block, new BitArray(occToBitNumber.Count, false));
                foreach (var ldur in localDefUses[block].defUses)
                {
                    generators[block].Set(occToBitNumber[new Tuple<BaseBlock, Occurrence>(block, ldur.Key)], true);
                    var variable = ldur.Key.Item2;
                    foreach (var e in occToBitNumber)
                        if (e.Key.Item2.Item2.Equals(ldur.Key.Item2))
                            killers[block].Set(occToBitNumber[e.Key], true);
                }
            }
        }

        protected override BitArray SetStartingSet()
        {
            return new BitArray(occToBitNumber.Count, false);
        }

        public override BitArray Collect(BitArray x, BitArray y)
        {
            return x.Or(y);
        }

        protected override BitArray Transfer(BitArray x, BaseBlock b)
        {
            return generators[b].Or(SubstractSets(x, killers[b]));
        }

        protected BitArray SubstractSets(BitArray firstSet, BitArray secondSet)
        {
            return firstSet.And(secondSet.Not());
        }

        private string BitArrayToString(BitArray arr)
        {
            var fullString = "";
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
          
        public Dictionary<IntraOccurence, HashSet<IntraOccurence>> GetDefUses()
        {
            Dictionary<IntraOccurence, HashSet<IntraOccurence>> defUses = new Dictionary<IntraOccurence, HashSet<IntraOccurence>>();
            foreach (var res in outs)
            {
                Console.WriteLine(res.Key.Name + " outs: ");
                PrintKillOfGen(res.Value);
            }

            return null;
        }
    }
}
