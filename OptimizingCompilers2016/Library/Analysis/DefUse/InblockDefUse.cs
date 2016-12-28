using OptimizingCompilers2016.Library.ThreeAddressCode;
using OptimizingCompilers2016.Library.ThreeAddressCode.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using DefsMap = System.Collections.Generic.Dictionary<OptimizingCompilers2016.Library.ThreeAddressCode.Values.IdentificatorValue, System.Tuple<int, OptimizingCompilers2016.Library.ThreeAddressCode.Values.IdentificatorValue>>;
using DefsMultiMap = System.Collections.Generic.Dictionary<OptimizingCompilers2016.Library.ThreeAddressCode.Values.IdentificatorValue
                                                         , System.Collections.Generic.HashSet<System.Tuple<OptimizingCompilers2016.Library.BaseBlock
                                                                                                         , System.Tuple<int
                                                                                                                      , OptimizingCompilers2016.Library.ThreeAddressCode.Values.IdentificatorValue>>>>;
using Occurrence = System.Tuple<int, OptimizingCompilers2016.Library.ThreeAddressCode.Values.IdentificatorValue>;
using IntraOccurence = System.Tuple<OptimizingCompilers2016.Library.BaseBlock, System.Tuple<int, OptimizingCompilers2016.Library.ThreeAddressCode.Values.IdentificatorValue>>;

namespace OptimizingCompilers2016.Library.Analysis.DefUse
{
    public class InblockDefUse
    {
        private BaseBlock block;
        public Dictionary<IntraOccurence, HashSet<IntraOccurence>> defUses { get; set; } = new Dictionary<IntraOccurence, HashSet<IntraOccurence>>();
        public Dictionary<IntraOccurence, HashSet<IntraOccurence>> useDefs { get; set; } = new Dictionary<IntraOccurence, HashSet<IntraOccurence>>();

        private EqualsBitArray ins = null;
        private Dictionary<IntraOccurence, int> occToBitNumber = null;
        private Dictionary<IntraOccurence, HashSet<IntraOccurence>> globalDefUses = null;

        // Отображение из переменной во множество её вхождений, последних перед анализируемой строкой.
        // Перед началом анализа блока заполняется по IN из результата работы интерационного алгоритма, если ins != null
        DefsMultiMap lastDef = new DefsMultiMap();

        private void setLastDef(IdentificatorValue variable, IntraOccurence occurrence)
        {
            if (!lastDef.ContainsKey(variable))
            {
                lastDef.Add(variable, new HashSet<IntraOccurence>());
            }
            else
            {
                lastDef[variable] = new HashSet<IntraOccurence>();
            }
            lastDef[variable].Add(occurrence);

            if (!defUses.Keys.Contains(occurrence))
            {
                defUses.Add(occurrence, new HashSet<IntraOccurence>());
            }

            if (globalDefUses != null && !globalDefUses.Keys.Contains(occurrence))
            {
                    globalDefUses.Add(occurrence, new HashSet<Tuple<BaseBlock, Tuple<int, IdentificatorValue>>>());
            }
        }

        private void addLastDef(IdentificatorValue variable, IntraOccurence occurrence)
        {
            if (!lastDef.ContainsKey(variable))
            {
                lastDef.Add(variable, new HashSet<IntraOccurence>());
            }
            lastDef[variable].Add(occurrence);

            if (!defUses.Keys.Contains(occurrence))
            {
                defUses.Add(occurrence, new HashSet<IntraOccurence>());
            }

            if (globalDefUses != null && !globalDefUses.Keys.Contains(occurrence))
            {
                globalDefUses.Add(occurrence, new HashSet<Tuple<BaseBlock, Tuple<int, IdentificatorValue>>>());
            }
        }

        private bool checkDefInIns(IdentificatorValue occ)
        {
            if (occToBitNumber == null)
                return false;

            foreach (var occs in occToBitNumber)
            {
                if (ins.Bits[occs.Value] && occs.Key.Item2.Item2.Equals(occ))
                {
                    return true;
                }
            }

            return false;
        }

        //private HashSet<Occurrence> getDefInIns(IdentificatorValue occ)
        //{
        //    var res = new HashSet<Occurrence>();
        //    foreach (var occs in occToBitNumber)
        //    {
        //        if (ins.Bits[occs.Value] && occs.Key.Item2.Item2.Equals(occ))
        //        {
        //            res.Add(occs.Key.Item2);
        //        }
        //    }

        //    return res;
        //}

        private void fillLastDefFromIns()
        {
            if (occToBitNumber == null)
                return;

            foreach (var occs in occToBitNumber)
            {
                if (ins.Bits[occs.Value])
                {
                    addLastDef(occs.Key.Item2.Item2, occs.Key);
                }
            }
        }

        private bool checkLastDefs(IdentificatorValue occ)
        {
            if (!lastDef.ContainsKey(occ) && !checkDefInIns(occ))
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

            if (checkLastDefs(variable))
            {
                foreach (var ld in lastDef)
                {
                    foreach (var inOcc in ld.Value)
                    {
                        defUses[inOcc].Add(new IntraOccurence(block, new Occurrence(index, variable)));

                        if (globalDefUses != null)
                        {
                            globalDefUses[inOcc].Add(new IntraOccurence(block, new Occurrence(index, variable)));
                        }
                    }
                }
            }
        }

        private void fillDefUses(List<IThreeAddressCode> code)
        {
            fillLastDefFromIns();

            int index = 0;
            foreach (var line in code)
            {
                addUse(line.LeftOperand, index);
                addUse(line.RightOperand, index);

                if (line.Destination is IdentificatorValue)
                    setLastDef(line.Destination as IdentificatorValue, new IntraOccurence(block, new Occurrence(index, line.Destination as IdentificatorValue)));

                index++;
            }

            //return defUses;
        }

        private void fillUseDefs()
        {
            foreach (var defUse in defUses)
            {
                foreach (var use in defUse.Value)
                {
                    if (!useDefs.ContainsKey(use))
                    {
                        HashSet<Occurrence> defs = new HashSet<Occurrence>();
                        //defs.Add(defUse.Key);
                        //useDefs.Add(use, defs);
                    }
                    else {
                        useDefs[use].Add(defUse.Key);
                    }
                }
            }
        }

        public InblockDefUse(BaseBlock block)
        {
            this.block = block;

            fillDefUses(block.Commands);
            fillUseDefs();
        }

        public InblockDefUse(BaseBlock block
                , EqualsBitArray ins
                , Dictionary<Tuple<BaseBlock, Occurrence>, int>  occToBitNumber
                , Dictionary<IntraOccurence, HashSet<IntraOccurence>> globalDefUses)
        {
            this.block = block;

            this.ins = ins;
            this.occToBitNumber = occToBitNumber;
            this.globalDefUses = globalDefUses;

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
}
