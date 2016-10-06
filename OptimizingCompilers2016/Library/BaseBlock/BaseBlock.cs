using OptimizingCompilers2016.Library.ThreeAddressCode;
using System.Collections.Generic;

namespace OptimizingCompilers2016.Library.BaseBlock
{
    public class BaseBlock
    {
        public IEnumerable<IThreeAddressCode> Commands { get; }

        public IEnumerable<BaseBlock> Predecessors { get; }

        public BaseBlock JumpOutput { get; set; }

        public BaseBlock Output { get; set; }

        public BaseBlock()
        {
            Commands = new List<IThreeAddressCode>();
            Predecessors = new List<BaseBlock>();
        }
    }
}
