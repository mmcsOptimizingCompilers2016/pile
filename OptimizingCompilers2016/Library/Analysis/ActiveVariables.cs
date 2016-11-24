using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimizingCompilers2016.Library.ThreeAddressCode.Values;

namespace OptimizingCompilers2016.Library.Analysis
{
    class ActiveVariables
    {
        public List<HashSet<IdentificatorValue>> IN = new List<HashSet<IdentificatorValue>>();
        public List<HashSet<IdentificatorValue>> OUT = new List<HashSet<IdentificatorValue>>();
        public List<HashSet<IdentificatorValue>> Def = new List<HashSet<IdentificatorValue>>();
        public List<HashSet<IdentificatorValue>> Use = new List<HashSet<IdentificatorValue>>();

        private List<BaseBlock> blocks;

        public ActiveVariables(List<BaseBlock> blocks)
        {
            this.blocks = blocks;
            FillDefUse();
        }

        private void FillDefUse()
        {
            for (int i = 0; i < blocks.Count; i++)
            {
                var ldu = new InblockDefUse(blocks[i]);
                var Set = new HashSet<Library.ThreeAddressCode.Values.IdentificatorValue>();
                foreach (var key in ldu.result.Keys)
                    Set.Add(new OptimizingCompilers2016.Library.ThreeAddressCode.Values.IdentificatorValue(key.Item2.Value));
                Def.Add(Set);
            }

            for (int i = 0; i < blocks.Count; i++)
            {
                var ldu = new InblockDefUse(blocks[i]);
                var Set = new HashSet<Library.ThreeAddressCode.Values.IdentificatorValue>();
                foreach (var sets in ldu.result.Values)
                    foreach (var itemSet in sets)
                        Set.Add(new OptimizingCompilers2016.Library.ThreeAddressCode.Values.IdentificatorValue(itemSet.Item2.Value));
                Use.Add(Set);
            }
        }


    }
}
