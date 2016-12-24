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
        Dictionary<string, ExpressionSet> eInB = new Dictionary<string, ExpressionSet>();
        Dictionary<string, ExpressionSet> eOutB = new Dictionary<string, ExpressionSet>();

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
        private void initOutB(List<BaseBlock> blocks)
        {
            ExpressionSet allExpressions = extractAllExpressions(ref blocks);

            foreach (var block in blocks)
            {
                eOutB[block.Name] = new ExpressionSet(allExpressions);
            }
        }

        private void initInB(List<BaseBlock> blocks)
        {
            foreach (var block in blocks)
            {
                eInB[block.Name] = new ExpressionSet();
            }
        }

        /// <summary>
        /// Optimize program using the results of available expressions analysis
        /// </summary>
        public bool Optimize(ControlFlowGraph graph)
        {
            List<BaseBlock> blocks = graph.GetVertices().ToList();
            initInB(blocks);
            initOutB(blocks);
            if (eOutB.Count == 0 || eOutB.First().Value.Count == 0)
                return false;

            // TODO: set inB in first bb into {} manually
            iterationalAlgorithm(blocks);

            return replaceCSE(ref blocks);
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
        private void iterationalAlgorithm(List<BaseBlock> blocks)
        {

            foreach (var block in blocks)
            {
                var genB = new ExpressionSet();
                var killB = new IdentificatorSet();
                getGenKill(block, ref genB, ref killB);
                eGenB.Add(block.Name, genB);
                eKillB.Add(block.Name, transformToExprSet(eOutB[block.Name], killB));
            }

            bool changed;
            do
            {
                changed = false;
                foreach (var block in blocks)
                {
                    ExpressionSet ins = updateInB(block, eOutB);
                    if (!ins.SetEquals(eInB[block.Name]))
                    {
                        changed = true;
                        eInB[block.Name] = ins;
                    }

                    ExpressionSet outs = updateOutB(block, eInB[block.Name]);
                    if (!outs.SetEquals(eOutB[block.Name]))
                    {
                        changed = true;
                        eOutB[block.Name] = outs;
                    }
                }
            }
            while (changed);
            
        }

        public static bool isEqualBinRHS(IThreeAddressCode i1, IThreeAddressCode i2)
        {
            if (i1.Operation != i2.Operation)
                return false;
            
            if (BinaryExpression.isCommutative(i1.Operation))
            {
                if (i1.LeftOperand.Value.Equals(i2.RightOperand.Value) &&
                    i1.RightOperand.Value.Equals(i2.LeftOperand.Value))
                    return true;
            }
            return i1.LeftOperand == i2.LeftOperand && i1.RightOperand == i2.RightOperand;
        }

        private void replaceAllOccurences(ref BaseBlock block, IThreeAddressCode inst, BinaryExpression bInst,
            IdentificatorValue newName)
        {
            foreach(var pred in block.Predecessors)
            {
                if (eGenB[pred.Name].Contains(bInst))
                {
                    for (int i = pred.Commands.Count-1; i >= 0; --i)
                    {
                        var command = pred.Commands[i];
                        if (command.Destination != null)
                        {
                            // expression must have been killed
                            Debug.Assert(command.Destination.Value != bInst.LeftOperand.Value as string);
                            Debug.Assert(command.Destination.Value != bInst.RightOperand.Value as string);
                        }
                        if (BinaryExpression.isModifiableOperation(command.Operation) && isEqualBinRHS(inst, command))
                        {
                            LinearRepresentation commonExpression = new LinearRepresentation(inst.Operation,
                                newName, inst.LeftOperand, inst.RightOperand);
                            pred.Commands.Insert(i, commonExpression);
                            
                            LinearRepresentation replacer = new LinearRepresentation(Operation.Assign,
                                command.Destination, newName);
                            pred.Commands[i + 1] = replacer;

                            // TODO: modify gen

                            return;
                            
                            
                        }
                    }
                    //Debug.Assert(false); // unreachable
                }
                else
                {
                    //TODO:
                    // run algorithm (replaceAllOccurences) for this block predecessors.
                    // all predecessors (?) should be filled with value 'newName'
                    continue;
                }
            }
            
            
        }

        private bool replaceCSE(ref List<BaseBlock> blocks)
        {
            const string namePrefix = "cseTmp";
            int nameId = 0;

            for (int i = 0; i < blocks.Count; ++i)
            {
                var block = blocks[i];
                Debug.Assert(eInB.ContainsKey(block.Name));
                ExpressionSet blockIn = eInB[block.Name];
                ExpressionSet blockGen = eGenB[block.Name];
                if (blockIn.Count == 0)
                    continue;

                for (int j = 0; j < block.Commands.Count; ++j)
                {
                    var inst = block.Commands[j];
                    if (!BinaryExpression.isModifiableOperation(inst.Operation))
                        continue;
                    var cur = new BinaryExpression(inst);
                    if (!blockIn.Contains(cur))
                        continue;
                    var iValue = new IdentificatorValue(namePrefix + nameId.ToString());
                    replaceAllOccurences(ref block, inst, cur, iValue);
                    block.Commands[j] = new LinearRepresentation(Operation.Assign, inst.Destination, iValue);
                }

            }
            return false;
        }

    }
}