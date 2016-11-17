using OptimizingCompilers2016.Library.LinearCode;
using OptimizingCompilers2016.Library.ThreeAddressCode;
using OptimizingCompilers2016.Library.ThreeAddressCode.Values;
using OptimizingCompilers2016.Library.ThreeAddressCode.Values.Base;
using System.Collections.Generic;

namespace OptimizingCompilers2016.Library
{
    public class BaseBlock
    {
        public List<IThreeAddressCode> Commands { get; }

        public List<BaseBlock> Predecessors { get; }

        public BaseBlock JumpOutput { get; set; }

        public BaseBlock Output { get; set; }

        public string Name { get; set; }

        public BaseBlock()
        {
            Commands = new List<IThreeAddressCode>();
            Predecessors = new List<BaseBlock>();
        }

        public override string ToString()
        {
            string result = "Block " + Name + "\n";

            result += "Ins: [";
            foreach (var ins in Predecessors)
                result += ins.Name + ", ";
            result += "]\n";

            foreach (var line in Commands)
                result += line.ToString() + "\n";

            result += "JumpOut: " + JumpOutput?.Name + "\n";
            result += "Out: " + Output?.Name + "\n";

            return result;
        }
    }
}
