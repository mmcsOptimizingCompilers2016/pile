using System.Collections.Generic;
using System.Linq;
using OptimizingCompilers2016.Library.Analysis.DefUse;
using OptimizingCompilers2016.Library.Analysis;
using OptimizingCompilers2016.Library.ThreeAddressCode;
using OptimizingCompilers2016.Library.ThreeAddressCode.Values;
using Occurrence = System.Tuple<int, OptimizingCompilers2016.Library.ThreeAddressCode.Values.IdentificatorValue>;

namespace OptimizingCompilers2016.Library.DeadCode
{

using IntraOccurence = System.Tuple<OptimizingCompilers2016.Library.BaseBlock, System.Tuple<int, int, OptimizingCompilers2016.Library.ThreeAddressCode.Values.IdentificatorValue>>;

    public class DeadCodeDeleting
    {
        
        public static void optimizeDeadCode(BaseBlock block, HashSet<IdentificatorValue> activeVars = null)
        {
            int count_commands = block.Commands.Count;

            iteration(block, activeVars);
            while(block.Commands.Count != count_commands)
            {
                count_commands = block.Commands.Count;
                iteration(block, activeVars);
            }
        }

        private static void iteration(BaseBlock block, HashSet<IdentificatorValue> activeVars = null)
        {
            HashSet<IThreeAddressCode> toDelete  = new HashSet<IThreeAddressCode>();
            
            InblockDefUse DU = new InblockDefUse(block);
            List<IntraOccurence> ordering = new List<IntraOccurence>(DU.defUses.Keys);

            HashSet<IdentificatorValue> viewed = new HashSet<IdentificatorValue>();
            if (activeVars == null)
            {
                foreach (var item in DU.defUses.Keys)
                {
                    viewed.Add(item.Item2.Item3);
                }
            }
            else
            {
                foreach (var item in DU.defUses.Keys)
                {
                    if (!activeVars.Contains(item.Item2.Item3))
                        viewed.Add(item.Item2.Item3);
                }
            }

            ordering = new List<IntraOccurence>(ordering.OrderBy((IntraOccurence val) => { return val.Item1; }));
            for (int i = ordering.Count-1; i >=0; i--)
            {
                if (!viewed.Contains(ordering[i].Item2.Item3))
                {
                    viewed.Add(ordering[i].Item2.Item3);
                }
                else
                {
                    if (DU.defUses[ordering[i]].Count == 0)
                    {
                        toDelete.Add(block.Commands[ordering[i].Item2.Item1]);
                    }

                }
            }

            foreach (var item in toDelete)
            {
                block.Commands.Remove(item);
            }
        }

    }
}
