using System.Collections.Generic;
using System.Linq;
using OptimizingCompilers2016.Library.Analysis.DefUse;
using OptimizingCompilers2016.Library.Analysis;
using OptimizingCompilers2016.Library.ThreeAddressCode;
using OptimizingCompilers2016.Library.ThreeAddressCode.Values;
using Occurrence = System.Tuple<int, OptimizingCompilers2016.Library.ThreeAddressCode.Values.IdentificatorValue>;

namespace OptimizingCompilers2016.Library.DeadCode
{
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
            //throw new NotImplementedException("Not implemented deleting dead code");


            //Dictionary<Occurrence, HashSet<Occurrence>>

            HashSet<IThreeAddressCode> toDelete  = new HashSet<IThreeAddressCode>();
            HashSet<IdentificatorValue> viewed = new HashSet<IdentificatorValue>();
            InblockDefUse DU = new InblockDefUse(block);

            for (int i = DU.defUses.Count-1; i >=0; i--)
            {
                if (activeVars == null)
                {
                    //if (DU.defUses[ordering[i]].Count == 0)
                    //if (!viewed.Contains(DU.defUses.ElementAt(i).Key.Item2.Item3) && !activeVars.Contains(DU.defUses.ElementAt(i).Key.Item2.Item3))

                    if (!viewed.Contains(DU.defUses.ElementAt(i).Key.Item2.Item3))
                    {
                        viewed.Add(DU.defUses.ElementAt(i).Key.Item2.Item3);
                    }
                    else
                    {
                        if (DU.defUses.ElementAt(i).Value.Count == 0)
                        {
                            toDelete.Add(block.Commands[DU.defUses.ElementAt(i).Key.Item2.Item1]);
                        }

                    }
                }
                else
                {
                    var elem = DU.defUses.ElementAt(i).Key.Item2.Item3;
                    if (!viewed.Contains(elem) && activeVars.Contains(elem))
                    {
                        viewed.Add(DU.defUses.ElementAt(i).Key.Item2.Item3);
                    }
                    else
                    {
                        if (DU.defUses.ElementAt(i).Value.Count == 0)
                        {
                            toDelete.Add(block.Commands[DU.defUses.ElementAt(i).Key.Item2.Item1]);
                        }

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
