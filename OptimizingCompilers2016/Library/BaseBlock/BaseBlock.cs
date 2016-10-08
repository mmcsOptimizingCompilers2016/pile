using OptimizingCompilers2016.Library.LinearCode;
using OptimizingCompilers2016.Library.ThreeAddressCode;
using System.Collections.Generic;

namespace OptimizingCompilers2016.Library.BaseBlock
{
    public class BaseBlock
    {
        public List<LinearRepresentation> Commands { get; }

        public List<BaseBlock> Predecessors { get; }

        public BaseBlock JumpOutput { get; set; }

        public BaseBlock Output { get; set; }

        public string Name { get; set; }

        public BaseBlock()
        {
            Commands = new List<LinearRepresentation>();
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
                if ( block.Commands.Count == 0)
                {
                    block.Predecessors[0].Output = block.Output;
                    block.Output.Predecessors.Remove(block);
                    block.Output.Predecessors.Add(block.Predecessors[0]);
                }
            }

            blocks.RemoveAll(block => block.Commands.Count == 0);
        }

        public static List<BaseBlock> divide(List<LinearRepresentation> plainCode)
        {
            var blocks = new List<BaseBlock>();
            var labels = new Dictionary<Label, int>();

            
            blocks.Add(makeBlock());

            for (int i=0; i < plainCode.Count; ++i)
            {
                if (plainCode[i].operation == LinearCode.Operation.LabelOp)
                {
                    var block = makeBlock();
                    blocks[blocks.Count - 1].Output = block;
                    block.Predecessors.Add(blocks[blocks.Count - 1]);

                    blocks.Add(block);
                    labels.Add(plainCode[i].destination as Label, blocks.Count - 1);
                }
                blocks[blocks.Count-1].Commands.Add(plainCode[i]);
                if (plainCode[i].operation == LinearCode.Operation.Goto || plainCode[i].operation == LinearCode.Operation.CondGoto)
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
                    if ( line.operation == LinearCode.Operation.Goto || line.operation == LinearCode.Operation.CondGoto)
                    {
                        block.JumpOutput = blocks[labels[line.destination as Label]];
                        blocks[labels[line.destination as Label]].Predecessors.Add(block);
                    }
                }
            }

            eraseEmptyBlocks(blocks);

            return blocks;
        }
    }
}
