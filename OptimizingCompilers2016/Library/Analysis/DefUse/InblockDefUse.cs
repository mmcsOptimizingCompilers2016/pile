using OptimizingCompilers2016.Library.ThreeAddressCode;
using OptimizingCompilers2016.Library.ThreeAddressCode.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using DefsMap = System.Collections.Generic.Dictionary<OptimizingCompilers2016.Library.ThreeAddressCode.Values.IdentificatorValue, System.Tuple<int, OptimizingCompilers2016.Library.ThreeAddressCode.Values.IdentificatorValue>>;
using Occurrence = System.Tuple<int, OptimizingCompilers2016.Library.ThreeAddressCode.Values.IdentificatorValue>;

namespace OptimizingCompilers2016.Library.Analysis.DefUse
{
    public class InblockDefUse
    {
        public Dictionary<Occurrence, HashSet<Occurrence>> defUses { get; set; } = new Dictionary<Occurrence, HashSet<Occurrence>>();
        public Dictionary<Occurrence, HashSet<Occurrence>> useDefs { get; set; } = new Dictionary<Occurrence, HashSet<Occurrence>>();
        DefsMap lastDef = new DefsMap();

        private void setLastDef(IdentificatorValue variable, Occurrence occurrence)
        {
            if (!lastDef.ContainsKey(variable))
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

            if (checkLastDefs(variable))
            {
                defUses[lastDef[variable]].Add(new Occurrence(index, variable));
            }
        }

        private void fillDefUses(List<IThreeAddressCode> code)
        {
            //defUses.Clear();
            int index = 0;
            foreach (var line in code)
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
            foreach (var defUse in defUses)
            {
                foreach (var use in defUse.Value)
                {
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
}
