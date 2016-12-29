using System;
using System.Collections.Generic;
using System.Linq;

// <номер строки, номер позиции в операторе, переменная>
using Occurrence = System.Tuple<int, int, OptimizingCompilers2016.Library.ThreeAddressCode.Values.IdentificatorValue>;
// <блок, <номер строки, номер позиции в операторе, переменная> >
using IntraOccurence = System.Tuple<OptimizingCompilers2016.Library.BaseBlock, System.Tuple<int, int, OptimizingCompilers2016.Library.ThreeAddressCode.Values.IdentificatorValue>>;

namespace OptimizingCompilers2016.Library.Analysis.DefUse
{
    public class GlobalDefUse : BaseIterationAlgorithm<EqualsBitArray>
    {
        private Dictionary<IntraOccurence, HashSet<IntraOccurence>> defUses = new Dictionary<IntraOccurence, HashSet<IntraOccurence>>();
        private Dictionary<IntraOccurence, HashSet<IntraOccurence>> useDefs = new Dictionary<IntraOccurence, HashSet<IntraOccurence>>();

        protected Dictionary<BaseBlock, InblockDefUse> localDefUses = new Dictionary<BaseBlock, InblockDefUse>();
        protected override void FillGeneratorsAndKillers(List<BaseBlock> blocks)
        {
            foreach (var block in blocks)
            {
                var inBlockDefUse = new InblockDefUse(block);
                //Console.WriteLine("Local def-uses: ");
                //Console.WriteLine(block.Name + " : " + inBlockDefUse.ToString());
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
                    generators[block].Set(occToBitNumber[ldur.Key], true);

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
                //Console.WriteLine(block.Name);
                //Console.WriteLine(" Gen: " + generators[block].ToString());
                //PrintKillOfGen(generators[block]);
                //Console.WriteLine("Kill: " + killers[block].ToString());
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
            defUses = new Dictionary<IntraOccurence, HashSet<IntraOccurence>>();

            foreach (var block in localDefUses.Keys)
            {
                var inBlockDefUse = new InblockDefUse(block, outs[block], occToBitNumber, defUses);

                //foreach (var occ in inBlockDefUse.defUses)
                //{
                //    var inOccHS = new HashSet<IntraOccurence>();
                //    foreach (var locOcc in occ.Value)
                //    {
                //        inOccHS.Add(locOcc);
                //    }
                //    defUses.Add(occ.Key, inOccHS);
                //}
                
                ///Console.WriteLine(block.Name + " : " + inBlockDefUse.ToString());
            }
            //foreach (var res in outs)
            //{
            //    Console.WriteLine(res.Key.Name + " outs: ");
            //    PrintKillOfGen(res.Value);
            //}
            return defUses;
        }

        public Dictionary<IntraOccurence, HashSet<IntraOccurence>> GetUseDefs()
        {
            useDefs = new Dictionary<IntraOccurence, HashSet<IntraOccurence>>();

            foreach (var defUse in defUses)
            {
                foreach (var use in defUse.Value)
                {
                    if (!useDefs.ContainsKey(use))
                    {
                        useDefs.Add(use, new HashSet<IntraOccurence>());
                    }

                    useDefs[use].Add(defUse.Key);
                }
            }

            return useDefs;
        }

        public override string ToString()
        {
            var res = "";

            var defUses = GetDefUses();
            var useDefs = GetUseDefs();

            var defUseString = defUses.Select(item => item.Key.Item1.Name + "#" + item.Key.Item2 + " => {" + String.Join(", ", item.Value.Select(occ => occ.Item1.Name + "#" + occ.Item2)) + "}");
            var sdu = "defUse: " + String.Join("\n", defUseString) + "\n";
            var useDefString = useDefs.Select(item => item.Key.Item1.Name + "#" + item.Key.Item2 + " => {" + String.Join(", ", item.Value.Select(occ => occ.Item1.Name + "#" + occ.Item2)) + "}");
            var sud = "useDef: " + String.Join("\n", useDefString) + "\n";



            //res += .Name + ": \n" + sud + sdu;
            //foreach (var occp in defUses)
            //{
            //    //res += occp.Key.Item1.Name + ": ";
            //    //foreach (var right in occp.Value)
            //    //{
            //    //    res += right.Item2.Item3.Value + ", ";
            //    //}
            //    //res += "\n---\n";
            //}

            return res;
        }
    }
}
