using OptimizingCompilers2016.Library.ThreeAddressCode;
using System.Collections.Generic;
using System.Linq;

namespace OptimizingCompilers2016.Library
{
    public class BaseBlock
    {
        public List<IThreeAddressCode> Commands { get; set; }

        public List<BaseBlock> Predecessors { get; }

        public BaseBlock JumpOutput { get; set; }

        public BaseBlock Output { get; set; }

        public string Name { get; set; }

        public BaseBlock()
        {
            Commands = new List<IThreeAddressCode>();
            Predecessors = new List<BaseBlock>();
        }

        public override bool Equals(object obj)
        {
            var second = obj as BaseBlock;
            return second != null && Name.Equals(second.Name);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            var result = "Block " + Name + "\n";

            result += "Ins: [";
            result = Predecessors.Aggregate(result, (current, ins) => current + (ins.Name + ", "));
            result += "]\n";

            result = Commands.Aggregate(result, (current, line) => current + (line.ToString() + "\n"));

            result += "JumpOut: " + JumpOutput?.Name + "\n";
            result += "Out: " + Output?.Name + "\n";

            return result;
        }
    }
}
