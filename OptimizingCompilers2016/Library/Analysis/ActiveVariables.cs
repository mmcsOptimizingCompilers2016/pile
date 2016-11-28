using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimizingCompilers2016.Library.ThreeAddressCode.Values;

namespace OptimizingCompilers2016.Library.Analysis
{
    public class ActiveVariables
    {

        public List<HashSet<IdentificatorValue>> result = new List<HashSet<IdentificatorValue>>();

        private List<HashSet<IdentificatorValue>> IN = new List<HashSet<IdentificatorValue>>();
        private List<HashSet<IdentificatorValue>> OUT = new List<HashSet<IdentificatorValue>>();
        private List<HashSet<IdentificatorValue>> Def = new List<HashSet<IdentificatorValue>>();
        private List<HashSet<IdentificatorValue>> Use = new List<HashSet<IdentificatorValue>>();
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
                var Set = new HashSet<IdentificatorValue>();
                foreach (var key in ldu.result.Keys)
                    Set.Add(new IdentificatorValue(key.Item2.Value));
                Def.Add(Set);
            }

            for (int i = 0; i < blocks.Count; i++)
            {
                var ldu = new InblockDefUse(blocks[i]);
                var Set = new HashSet<IdentificatorValue>();
                foreach (var sets in ldu.result.Values)
                    foreach (var itemSet in sets)
                        Set.Add(new IdentificatorValue(itemSet.Item2.Value));
                Use.Add(Set);
            }

            for (int i = 0; i < blocks.Count; i++)
            {
                IN.Add(new HashSet<IdentificatorValue>());
                OUT.Add(new HashSet<IdentificatorValue>());
            }
        }


        public List<HashSet<IdentificatorValue>> runAnalys()
        {
            var oldIN = new List<HashSet<IdentificatorValue>>(IN);
            
            while (true)
            {
                var count = 0;
                //Console.WriteLine("Ахахахха)000))");
                for (int i = 0; i < blocks.Count; i++)
                {
                    OUT[i] = new HashSet<IdentificatorValue>();
                    foreach (var predecessor in blocks[i].Predecessors)
                    {
                        int index = Convert.ToInt32(predecessor.Name.Remove(0, 1)) - 1;
                        if (index >=0)
                            OUT[i].UnionWith(IN[index]);
                    }
                    OUT[i].ExceptWith(Def[i]);
                    OUT[i].UnionWith(Use[i]);
                    IN[i] = new HashSet<IdentificatorValue>(OUT[i]);
                }

                for (int i = 0; i < oldIN.Count(); i++)
                {
                    if (!oldIN[i].SetEquals(IN[i]))
                        count++;
                }

                if (count == 0)
                    break;

                oldIN = new List<HashSet<IdentificatorValue>>(IN);
            }

            result = new List<HashSet<IdentificatorValue>>(IN);

            return result;   
        }


    }
}
