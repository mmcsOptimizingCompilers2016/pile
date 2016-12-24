using System;
using System.Collections.Generic;
using IntraOccurence = System.Tuple<OptimizingCompilers2016.Library.BaseBlock, System.Tuple<int, OptimizingCompilers2016.Library.ThreeAddressCode.Values.IdentificatorValue>>;
using Occurrence = System.Tuple<int, OptimizingCompilers2016.Library.ThreeAddressCode.Values.IdentificatorValue>;

namespace OptimizingCompilers2016.Library.Analysis.DefUse
{
    public class GlobalDefUse : BaseIterationAlgorithm<EqualsBitArray>
    {
        protected override void FillGeneratorsAndKillers(List<BaseBlock> blocks)
        {
            foreach (var block in blocks)
            {
                var inBlockDefUse = new InblockDefUse(block);
                Console.WriteLine("Local def-uses: ");
                Console.WriteLine(block.Name + " : " + inBlockDefUse.ToString());
                localDefUses.Add(block, inBlockDefUse);
            }

            foreach (var block in blocks)
            {
                generators.Add(block, new EqualsBitArray(occToBitNumber.Count, false));
                killers.Add(block, new EqualsBitArray(occToBitNumber.Count, false));
                foreach (var ldur in localDefUses[block].defUses)
                {
                    foreach (var e in occToBitNumber)
                    {
                        if (e.Key.Item2.Item2.Equals(ldur.Key.Item2))
                        {
                            generators[block].Set(occToBitNumber[e.Key], false);
                        }
                    }
                    generators[block].Set(occToBitNumber[new Tuple<BaseBlock, Occurrence>(block, ldur.Key)], true);

                    var variable = ldur.Key.Item2;
                    //check if variable is used somewhere in another block
                    foreach (var e in occToBitNumber)
                    {
                        if (e.Key.Item2.Item2.Equals(ldur.Key.Item2))
                        {
                            killers[block].Set(occToBitNumber[e.Key], true);
                        }
                    }
                }
                Console.WriteLine(block.Name);
                Console.WriteLine(" Gen: " + generators[block].ToString());
                PrintKillOfGen(generators[block]);
                Console.WriteLine("Kill: " + killers[block].ToString());
            }
        }

        protected override EqualsBitArray SetStartingSet()
        {
            return new EqualsBitArray(occToBitNumber.Count, false);
        }

        protected override EqualsBitArray Transfer(EqualsBitArray x, BaseBlock b)
        {
            return generators[b].Or(SubstractSets(x, killers[b]));
        }

        public override EqualsBitArray Collect(EqualsBitArray x, EqualsBitArray y)
        {
            return x.Or(y);
        }

        public override void RunAnalysis(List<BaseBlock> blocks)
        {
            FillSupportingStructures(blocks);
            FillGeneratorsAndKillers(blocks);
            IterationAlgorithm(blocks);
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

        private EqualsBitArray SubstractSets(EqualsBitArray firstSet, EqualsBitArray secondSet)
        {
            return firstSet.And(secondSet.Not());
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
