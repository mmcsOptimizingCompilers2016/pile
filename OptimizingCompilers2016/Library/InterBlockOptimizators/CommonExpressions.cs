using OptimizingCompilers2016.Library.LinearCode;
using OptimizingCompilers2016.Library.ThreeAddressCode;
using OptimizingCompilers2016.Library.ThreeAddressCode.Values;
using System.Collections.Generic;
using System.Diagnostics;
using OptimizingCompilers2016.Library.Optimizators;
using System.Linq;


namespace OptimizingCompilers2016.Library.InterBlockOptimizators
{
    using ExpressionSet = HashSet<BinaryExpression>;

    using IdentificatorSet = HashSet<IdentificatorValue>;

    public class CommonExpressions : IInterBlockOptimizator
    {

        // block name -> expression set
        Dictionary<string, ExpressionSet> eGenB = new Dictionary<string, ExpressionSet>();
        Dictionary<string, ExpressionSet> eKillB = new Dictionary<string, ExpressionSet>();


        /// <summary>
        /// Retrieve all expressions from a block
        /// Example:
        ///      d1: c := a + b
        ///      d2: d := a + b
        /// d1 and d2 are considered to be the same expression i.e. ( a + b )

        /// @param block - base block
        /// @return set of all the expressions of the block
        /// </summary>
        private ExpressionSet extractAllExpressions(BaseBlock block)
        {
            ExpressionSet result = new ExpressionSet();

            List<IThreeAddressCode> instructionsInBlock = block.Commands;

            foreach (IThreeAddressCode instruction in instructionsInBlock)
            {
                if (!BinaryExpression.isModifiableOperation(instruction.Operation))
                    continue;

                result.Add(new BinaryExpression(instruction));
            }

            return result;
        }

        /// <summary>
        /// Retrieve all expressions from a program
        /// Example:
        ///      d1: c := a + b
        ///      d2: d := a + b
        /// d1 and d2 are considered to be the same expression i.e. ( a + b )

        /// @param blocks - list of all blocks in the program 
        /// @return set of all the expressions of the program
        /// </summary>
        private ExpressionSet extractAllExpressions(ref List<BaseBlock> blocks)
        {
            ExpressionSet result = new ExpressionSet();

            foreach (BaseBlock block in blocks)
            {
                // TODO: add comparator?
                result.UnionWith(extractAllExpressions(block));
            }

            return result;
        }

        /// <summary>
        /// init OUT Dictionary with all the expressions in program
        /// OUT dictionary maps block ( by its name ) to all expressions available after exiting the block
        ///
        /// @param blocks
        /// </summary>
        private Dictionary<string, ExpressionSet> initOutB(List<BaseBlock> blocks)
        {
            var result = new Dictionary<string, ExpressionSet>();
            ExpressionSet allExpressions = extractAllExpressions(ref blocks);

            foreach (var block in blocks)
            {
                result[block.Name] = new ExpressionSet(allExpressions);
            }
            return result;
        }

        private Dictionary<string, ExpressionSet> initInB(List<BaseBlock> blocks)
        {
            Dictionary<string, ExpressionSet> result = new Dictionary<string, HashSet<BinaryExpression>>();
            foreach (var block in blocks)
            {
                result[block.Name] = new ExpressionSet();
            }
            return result;
        }

        /// <summary>
        /// Optimize program using the results of available expressions analysis
        /// </summary>
        public bool Optimize(ControlFlowGraph graph)
        {
            List<BaseBlock> blocks = graph.GetBaseBlocks();
            var inB = initInB(blocks);
            var outB = initOutB(blocks);
            if (outB.Count == 0 || outB.First().Value.Count == 0)
                return false;


            // TODO: apply iterational algorithm to find the list of available expressions
            iterationalAlgorithm(blocks, ref inB, ref outB);

            return false;
        }

        /// <summary>
        /// Retrieves eGetB and eKillB for single basic block
        /// </summary>
        /// <param name="block"></param>
        /// <param name="gen"></param>
        /// <param name="kill"></param>
        void getGenKill(BaseBlock block, ref ExpressionSet gen, ref IdentificatorSet kill)
        {
            foreach (var instruction in block.Commands)
            {
                if (BinaryExpression.isModifiableOperation(instruction.Operation))
                {
                    gen.Add(new BinaryExpression(instruction));
                }

                if (instruction.Destination != null && instruction.Operation != Operation.Goto &&
                    instruction.Operation != Operation.CondGoto)
                {
                    kill.Add(instruction.Destination as IdentificatorValue);
                    gen.RemoveWhere(x => x.LeftOperand.Value as string == instruction.Destination.Value ||
                        x.RightOperand.Value as string == instruction.Destination.Value);
                }
            }
        }

        ExpressionSet transformToExprSet(ExpressionSet U, IdentificatorSet kills)
        {
            var result = new ExpressionSet();
            foreach (var kill in kills)
            {
                result.UnionWith(U.Where(x => x.LeftOperand.Value as string == kill.Value ||
                    x.RightOperand.Value as string == kill.Value));
            }
            return result;
        }
        /// <summary>
        /// Update input expressions set for a current block
        ///     IN[B] = П OUT[P] for all B.predecessors
        ///
        /// @param outB - output available expressions set
        /// </summary>
        private ExpressionSet updateInB(BaseBlock block,
            Dictionary<string, ExpressionSet> outB)
        {
            if (block.Predecessors.Count == 0)
                return new ExpressionSet();
            ExpressionSet newInB = new ExpressionSet(outB[block.Predecessors.First().Name]);
            foreach (var predecessor in block.Predecessors)
            {
                newInB.IntersectWith(outB[predecessor.Name]);
            }
            return newInB;
        }

        /// <summary>
        /// Update output expressions set for a current block
        ///     OUT[B] = EgenB OR (IN[B] \ EkillB)
        ///
        /// @param inB - input available expressions set
        /// </summary>
        private ExpressionSet updateOutB(BaseBlock block,
            ExpressionSet inB)
        {
            return new ExpressionSet(eGenB[block.Name].Union(inB.Except(eKillB[block.Name])));
        }

        /// <summary>
        /// Iterational process to find the list of available expressions
        ///     IN - set of expressions available at the start of a block
        ///     OUT - set of expressions available at the end of a block
        ///     EgenB - set of all the expressions in the block
        ///     EkillB - set of all the expressions which contain variables that are assigned to in this block
        ///
        /// @param blocks - list of all blocks in the program + fictional source block
        /// TODO: decide what this function should return
        /// </summary>
        private void iterationalAlgorithm(List<BaseBlock> blocks,
            ref Dictionary<string, ExpressionSet> inB, ref Dictionary<string, ExpressionSet> outB)
        {

            foreach (var block in blocks)
            {
                var genB = new ExpressionSet();
                var killB = new IdentificatorSet();
                getGenKill(block, ref genB, ref killB);
                eGenB.Add(block.Name, genB);
                eKillB.Add(block.Name, transformToExprSet(outB[block.Name], killB));
            }

            bool changed;
            do
            {
                changed = false;
                foreach (var block in blocks)
                {
                    ExpressionSet ins = updateInB(block, outB);
                    if (!ins.SetEquals(inB[block.Name]))
                    {
                        changed = true;
                        inB[block.Name] = ins;
                    }

                    ExpressionSet outs = updateOutB(block, inB[block.Name]);
                    if (!outs.SetEquals(outB[block.Name]))
                    {
                        changed = true;
                        outB[block.Name] = outs;
                    }
                }
            }
            while (changed);
            
        }
    }
}