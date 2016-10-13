using OptimizingCompilers2016.Library.LinearCode;
using OptimizingCompilers2016.Library.ThreeAddressCode;
using OptimizingCompilers2016.Library.ThreeAddressCode.Values;
using OptimizingCompilers2016.Library.ThreeAddressCode.Values.Base;
using System.Collections.Generic;

namespace OptimizingCompilers2016.Library.BaseBlock
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

    public static class BaseBlockDivider
    {
        private static int blockNum = 0;

        private static BaseBlock makeBlock()
        {
            var newblock = new BaseBlock();
            newblock.Name = "B" + blockNum.ToString();
            ++blockNum;
            return newblock;
        }

        private static void eraseEmptyBlocks(List<BaseBlock> blocks)
        {
            foreach (var block in blocks )
            {
                if ( block.Commands.Count == 0 && block.Predecessors.Count > 0)
                {
                    foreach ( var pred in block.Predecessors )
                    {
                        pred.Output = block.Output;
                        block.Output.Predecessors.Add(pred);
                    }
                    block.Output.Predecessors.Remove(block);
                }
            }

            blocks.RemoveAll(block => block.Commands.Count == 0);
        }

        public static List<BaseBlock> divide(List<LinearRepresentation> plainCode)
        {
            var blocks = new List<BaseBlock>();
            var labels = new Dictionary<LabelValue, int>();

            blocks.Add(makeBlock());

            for (int i=0; i < plainCode.Count; ++i)
            {
                if (plainCode[i].Label != null)
                {
                    var block = makeBlock();
                    blocks[blocks.Count - 1].Output = block;
                    block.Predecessors.Add(blocks[blocks.Count - 1]);

                    blocks.Add(block);
                    labels.Add(plainCode[i].Label, blocks.Count - 1);
                }
                blocks[blocks.Count-1].Commands.Add(plainCode[i]);
                if ( plainCode[i].Operation == Operation.CondGoto )
                {
                    var block = makeBlock();
                    blocks[blocks.Count - 1].Output = block;
                    block.Predecessors.Add(blocks[blocks.Count - 1]);

                    blocks.Add(block);
                }
            }

            foreach (var block in blocks)
            {
                foreach ( var line in block.Commands )
                {
                    if ( line.Operation == Operation.Goto 
                        || line.Operation == Operation.CondGoto)
                    {
                        var destBlock = blocks[labels[line.Destination as LabelValue]];
                        block.JumpOutput = destBlock;
                        destBlock.Predecessors.Add(block);
                    }
                }
            }

            eraseEmptyBlocks(blocks);

            return blocks;
        }
    }
}
