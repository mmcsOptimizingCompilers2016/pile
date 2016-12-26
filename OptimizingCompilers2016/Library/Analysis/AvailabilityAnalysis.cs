using OptimizingCompilers2016.Library.Analysis.DefUse;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IntraOccurence = System.Tuple<OptimizingCompilers2016.Library.BaseBlock, System.Tuple<int, OptimizingCompilers2016.Library.ThreeAddressCode.Values.IdentificatorValue>>;
using Occurrence = System.Tuple<int, OptimizingCompilers2016.Library.ThreeAddressCode.Values.IdentificatorValue>;

namespace OptimizingCompilers2016.Library.Analysis
{
    public class AvailabilityAnalysis : BaseIterationAlgorithm<EqualsBitArray>
    {
        protected Dictionary<BaseBlock, InblockDefUse> localDefUses = new Dictionary<BaseBlock, InblockDefUse>();
        protected override void FillGeneratorsAndKillers(List<BaseBlock> blocks)
        {
            foreach (var block in blocks)
                localDefUses.Add(block, new InblockDefUse(block));

            foreach (var block in blocks)
            {
                generators.Add(block, new EqualsBitArray(occToBitNumber.Count, false));
                killers.Add(block, new EqualsBitArray(occToBitNumber.Count, false));
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

        protected override EqualsBitArray SetStartingSet()
        {
            return new EqualsBitArray(occToBitNumber.Count, false);
        }

        public override EqualsBitArray Collect(EqualsBitArray x, EqualsBitArray y)
        {
            return x.Or(y);
        }

        protected override EqualsBitArray Transfer(EqualsBitArray x, BaseBlock b)
        {
            return generators[b].Or(SubstractSets(x, killers[b]));
        }

        public override void RunAnalysis(List<BaseBlock> blocks)
        {
            FillSupportingStructures(blocks);
            FillGeneratorsAndKillers(blocks);
            IterationAlgorithm(blocks);
        }

        protected EqualsBitArray SubstractSets(EqualsBitArray firstSet, EqualsBitArray secondSet)
        {
            return firstSet.And(secondSet.Not());
        }

        private void PrintKillOfGen(EqualsBitArray something)
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

        public override string ToString()
        {
            var result = string.Empty;
            var counter = 0;
            var numbers = new HashSet<int>();
            foreach (var pair in outs)
            {
                var bits = pair.Value.Bits;
                result += "In Block " + counter++ + " terms are available: ";
                for (var i = 0; i < bits.Count; ++i)
                {
                    if (bits[i])
                    {
                        result += "v" + i + " ";
                        numbers.Add(i);
                    }
                }
                result += "\n";
            }

            foreach (var num in numbers)
            {
                var pair = occToBitNumber.FirstOrDefault(x => x.Value == num).Key;
                var bb = pair.Item1;
                var line = bb.Commands[pair.Item2.Item1];
                result += "v" + num + ": " + line + "\n"; 
            }

            return result;
        }
    }
}
