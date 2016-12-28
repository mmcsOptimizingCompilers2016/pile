using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimizingCompilers2016.Library.ThreeAddressCode.Values;
using Occurrence = System.Tuple<int, OptimizingCompilers2016.Library.ThreeAddressCode.Values.IdentificatorValue>;
using OptimizingCompilers2016.Library.Analysis.DefUse;

namespace OptimizingCompilers2016.Library.Analysis
{
    public class ActiveVariables
    {
        public Dictionary<string, HashSet<IdentificatorValue>> result = new Dictionary<string, HashSet<IdentificatorValue>>();

        private Dictionary<string, HashSet<IdentificatorValue>> IN = new Dictionary<string, HashSet<IdentificatorValue>>();
        private Dictionary<string, HashSet<IdentificatorValue>> OUT = new Dictionary<string, HashSet<IdentificatorValue>>();
        private Dictionary<string, HashSet<IdentificatorValue>> Def = new Dictionary<string, HashSet<IdentificatorValue>>();
        private Dictionary<string, HashSet<IdentificatorValue>> Use = new Dictionary<string, HashSet<IdentificatorValue>>();
        private List<BaseBlock> blocks;

        public ActiveVariables(ControlFlowGraph blocks)
        {
            this.blocks = blocks.ToList();
            FillDefUse();
        }

        private void FillDefUse()
        {
            for (int i = 0; i < blocks.Count; i++)
            {
                var ldu = new InblockDefUse(blocks[i]);
                var Set = new HashSet<IdentificatorValue>();
                foreach (var key in ldu.defUses.Keys)
                    Set.Add(key.Item2.Item3);
                Def[blocks[i].Name] = Set;
            }

            for (int i = 0; i < blocks.Count; i++)
            {
                var ldu = new InblockDefUse(blocks[i]);
                var Set = new HashSet<IdentificatorValue>();
                foreach (var sets in ldu.defUses.Values)
                    foreach (var itemSet in sets)
                        Set.Add(itemSet.Item2.Item3);
                Use[blocks[i].Name] = Set;
            }

            for (int i = 0; i < blocks.Count; i++)
            {
                IN[blocks[i].Name] = new HashSet<IdentificatorValue>();
                OUT[blocks[i].Name] = new HashSet<IdentificatorValue>();
            }
        }

        public Dictionary<string, HashSet<IdentificatorValue>> runAnalys()
        {
            var oldIN = new Dictionary<string, HashSet<IdentificatorValue>>(IN);

            while (true)
            {
                bool isChanged = false;
                
                for (int i = 0; i < blocks.Count; i++)
                {
                    OUT[blocks[i].Name] = new HashSet<IdentificatorValue>();

                    var successors = new List<BaseBlock>();
                    successors.Add(blocks[i].Output);
                    successors.Add(blocks[i].JumpOutput);

                    foreach (var successor in successors)
                    {
                        if (successor != null)
                            OUT[blocks[i].Name].UnionWith(IN[successor.Name]);
                    }

                    OUT[blocks[i].Name].ExceptWith(Def[blocks[i].Name]);
                    OUT[blocks[i].Name].UnionWith(Use[blocks[i].Name]);
                    IN[blocks[i].Name] = new HashSet<IdentificatorValue>(OUT[blocks[i].Name]);
                }

                for (int i = 0; i < oldIN.Count(); i++)
                {
                    if (!oldIN[blocks[i].Name].SetEquals(IN[blocks[i].Name]))
                        isChanged = true;
                }

                if (!isChanged)
                    break;

                oldIN = new Dictionary<string, HashSet<IdentificatorValue>>(IN);
            }

            result = new Dictionary<string, HashSet<IdentificatorValue>>(IN);

            return result;
        }

        public override string ToString()
        {
            var res = "";

            foreach (var block in result)
            {
                res += block.Key + ": " + string.Join(", ", block.Value) + "\r\n";
            }

            return res;
        }

        
    }
}
