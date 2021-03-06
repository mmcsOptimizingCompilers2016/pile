﻿using OptimizingCompilers2016.Library;
using OptimizingCompilers2016.Library.LinearCode;
using OptimizingCompilers2016.Library.ThreeAddressCode;
using OptimizingCompilers2016.Library.ThreeAddressCode.Values;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DefsMap = System.Collections.Generic.Dictionary<OptimizingCompilers2016.Library.ThreeAddressCode.Values.IdentificatorValue, System.Tuple<int, OptimizingCompilers2016.Library.ThreeAddressCode.Values.IdentificatorValue>>;
using IntraOccurence = System.Tuple<OptimizingCompilers2016.Library.BaseBlock, System.Tuple<int, OptimizingCompilers2016.Library.ThreeAddressCode.Values.IdentificatorValue>>;
using Occurrence = System.Tuple<int, OptimizingCompilers2016.Library.ThreeAddressCode.Values.IdentificatorValue>;
using OptimizingCompilers2016.Library.Semilattice;

namespace OptimizingCompilers2016.Library.Analysis
{

    public class InblockDefUse
    {
        public Dictionary<Occurrence, HashSet<Occurrence>> defUses { get; set; } = new Dictionary<Occurrence, HashSet<Occurrence>>();
        public Dictionary<Occurrence, HashSet<Occurrence>> useDefs { get; set; } = new Dictionary<Occurrence, HashSet<Occurrence>>();
        DefsMap lastDef = new DefsMap();

        private void setLastDef(IdentificatorValue variable, Occurrence occurrence)
        {
            if ( !lastDef.ContainsKey(variable) )
            {
                lastDef.Add(variable, occurrence);
            }
            else
            {
                lastDef[variable] = occurrence;
            }

            defUses.Add(lastDef[variable], new HashSet<Occurrence>());
        }

        private bool checkLastDefs(IdentificatorValue occ)
        {
            if (!lastDef.ContainsKey(occ))
            {
                Console.WriteLine("Warning: There isn't such variable: " + occ.ToString());
                return false;
            }
            else
                return true;
        }

        private void addUse(IValue term, int index)
        {
            if (!(term is IdentificatorValue))
                return;

            var variable = term as IdentificatorValue;

            if ( checkLastDefs(variable))
            {
                defUses[lastDef[variable]].Add(new Occurrence(index, variable));
            }
        }
        
        private void fillDefUses(List<IThreeAddressCode> code)
        {
            //defUses.Clear();
            int index = 0;
            foreach ( var line in code )
            {
                addUse(line.LeftOperand, index);
                addUse(line.RightOperand, index);

                if (line.Destination is IdentificatorValue)
                    setLastDef(line.Destination as IdentificatorValue, new Occurrence(index, line.Destination as IdentificatorValue));

                index++;
            }

            //return defUses;
        }

        private void fillUseDefs() 
        {
            foreach (var defUse in defUses) {
                foreach (var use in defUse.Value) {
                    if (!useDefs.ContainsKey(use))
                    {
                        HashSet<Occurrence> defs = new HashSet<Occurrence>();
                        defs.Add(defUse.Key);
                        useDefs.Add(use, defs);
                    }
                    else {
                        useDefs[use].Add(defUse.Key);
                    }      
                }
            }
        }

        public InblockDefUse(BaseBlock block)
        {
            fillDefUses(block.Commands);
            fillUseDefs();
        }

        public override string ToString()
        {
            var defUseString = defUses.Select(item => item.Key + " => {" + String.Join(", ", item.Value) + "}");
            var sdu = "defUse: " + String.Join("\n", defUseString) + "\n";
            var useDefString = useDefs.Select(item => item.Key + " => {" + String.Join(", ", item.Value) + "}");
            var sud = "useDef: " + String.Join("\n", useDefString) + "\n";

            return sud + sdu;
        }
    }

    public class GlobalDefUse : BaseIterationAlgorithm<EqualsBitArray>
    {
        Dictionary<BaseBlock, InblockDefUse> localDefUses = new Dictionary<BaseBlock, InblockDefUse>();
        protected Dictionary<BaseBlock, EqualsBitArray> generators = new Dictionary<BaseBlock, EqualsBitArray>();
        protected Dictionary<BaseBlock, EqualsBitArray> killers = new Dictionary<BaseBlock, EqualsBitArray>();

        protected void FillGeneratorsAndKillers(List<BaseBlock> blocks)
        {
            foreach (var block in blocks)
            {
                //Console.WriteLine("Local def-uses: ");
                //Console.WriteLine(block.Name + " : " + new InblockDefUse(block).ToString());
                localDefUses.Add(block, new InblockDefUse(block));
            }

            foreach (var block in blocks)
            {
                generators.Add(block, new EqualsBitArray(occToBitNumber.Count, false));
                killers.Add(block, new EqualsBitArray(occToBitNumber.Count, false));
                foreach (var ldur in localDefUses[block].defUses)
                {
                    foreach(var e in occToBitNumber)
                    {
                        if ( e.Key.Item2.Item2.Equals(ldur.Key.Item2) )
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
                //Console.WriteLine(block.Name);
                //Console.WriteLine(" Gen: " + EqualsBitArrayToString(generators[block]));
                //PrintKillOfGen(generators[block]);
                //Console.WriteLine("Kill: " + EqualsBitArrayToString(killers[block]));
            }
        }

        protected override EqualsBitArray SetStartingSet() {
            return new EqualsBitArray(occToBitNumber.Count, false);
        }

        protected EqualsBitArray SubstractSets(EqualsBitArray firstSet, EqualsBitArray secondSet) {
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

        public override EqualsBitArray Collect(EqualsBitArray x, EqualsBitArray y) {
            return x.Or(y);
        }

        protected override EqualsBitArray Transfer(EqualsBitArray x, BaseBlock b)
        {
            return generators[b].Or(SubstractSets(x, killers[b]));
        }

        private Dictionary<BaseBlock, EqualsBitArray> iterationAlgorithm(List<BaseBlock> blocks) {
            base.IterationAlgorithm(blocks);
            return base.outs;
        }

        public void runAnalys(List<BaseBlock> blocks) {
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
