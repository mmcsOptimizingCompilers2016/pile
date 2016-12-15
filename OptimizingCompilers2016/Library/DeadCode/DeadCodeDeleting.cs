using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimizingCompilers2016.Library.Analysis;
using Occurrence = System.Tuple<int, OptimizingCompilers2016.Library.ThreeAddressCode.Values.IdentificatorValue>;
using OptimizingCompilers2016.Library.ThreeAddressCode;

namespace OptimizingCompilers2016.Library.DeadCode
{
    public class DeadCodeDeleting
    {
        
        public static void optimizeDeadCode(BaseBlock block)
        {
            int count_commands = block.Commands.Count;

            iteration(block);
            while(block.Commands.Count != count_commands)
            {
                count_commands = block.Commands.Count;
                iteration(block);
            }
        }

        private static void iteration(BaseBlock block)
        {
            //throw new NotImplementedException("Not implemented deleting dead code");


            //Dictionary<Occurrence, HashSet<Occurrence>>

            HashSet<IThreeAddressCode> toDelete  = new HashSet<IThreeAddressCode>();
            HashSet<OptimizingCompilers2016.Library.ThreeAddressCode.Values.IdentificatorValue> viewed = new HashSet<OptimizingCompilers2016.Library.ThreeAddressCode.Values.IdentificatorValue>();
            InblockDefUse DU = new InblockDefUse(block);

            for (int i = DU.result.Count-1; i >=0; i--)
            {
                if(!viewed.Contains(DU.result.ElementAt(i).Key.Item2))
                {
                    viewed.Add(DU.result.ElementAt(i).Key.Item2);
                }
                else
                {
                    if (DU.result.ElementAt(i).Value.Count == 0)
                    {
                        toDelete.Add(block.Commands[DU.result.ElementAt(i).Key.Item1]);
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
