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
        private Dictionary<string, ExpressionSet> eGenB = new Dictionary<string, ExpressionSet>();
        private Dictionary<string, ExpressionSet> eKillB = new Dictionary<string, ExpressionSet>();
        private Dictionary<string, ExpressionSet> eInB = new Dictionary<string, ExpressionSet>();
        private Dictionary<string, ExpressionSet> eOutB = new Dictionary<string, ExpressionSet>();

        private Dictionary<string, Dictionary<BinaryExpression, IdentificatorValue>> eReplaceB =
            new Dictionary<string, Dictionary<BinaryExpression, IdentificatorValue>>();
        // for variable naming
        private int counter = 0;
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

            iterationalAlgorithm(blocks);

            initReplace(blocks);

            return replaceCSE(ref blocks);
        }

        private void initReplace(List<BaseBlock> blocks)
        {
            foreach (var block in blocks)
            {
                eReplaceB.Add(block.Name, new Dictionary<BinaryExpression, IdentificatorValue>());
            }
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
                    ExpressionSet ins = block.Name == blocks[0].Name ?
                        new ExpressionSet() :
                        updateInB(block, eOutB);
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
            return i1.LeftOperand.Value.Equals(i2.LeftOperand.Value) &&
                i1.RightOperand.Value.Equals(i2.RightOperand.Value);
        }

        private string newVarName()
        {
            const string namePrefix = "cseTmp";
            return namePrefix + counter++.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="block"></param>
        /// <param name="inst"></param>
        /// <param name="bInst"></param>
        /// <param name="newName">set by function</param>
        private void replaceAllOccurences(ref BaseBlock block, IThreeAddressCode inst, BinaryExpression bInst,
            ref IdentificatorValue newName, ref HashSet<string> watched)
        {
            for (int k = 0; k < block.Predecessors.Count; ++k)
            {
                var pred = block.Predecessors[k];
                if (watched.Contains(pred.Name))
                {
                    continue;
                }
                watched.Add(pred.Name);
                if (eGenB[pred.Name].Contains(bInst))
                {
                    // already has a variable name, just use it
                    if (eReplaceB[pred.Name].ContainsKey(bInst))
                    {
                        if (newName == null)
                        {
                            newName = eReplaceB[pred.Name][bInst];
                            continue;
                        }
                        if (newName.Equals(eReplaceB[pred.Name][bInst]))
                        {
                            continue;
                        }
                        // create new variable, aliasing the old one
                        var rename = new LinearRepresentation(Operation.Assign, newName,
                            eReplaceB[pred.Name][bInst]);
                        if (pred.Commands.Last().Operation == Operation.CondGoto ||
                            pred.Commands.Last().Operation == Operation.Goto)
                        {
                            pred.Commands.Insert(pred.Commands.Count - 1, rename);
                        }
                        else
                        {
                            pred.Commands.Add(rename);
                        }
                        continue;
                    }
                    int i = 0;
                    for (i = pred.Commands.Count-1; i >= 0; --i)
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
                            if (newName == null)
                            {
                                newName = new IdentificatorValue(newVarName());
                            }
                            LinearRepresentation commonExpression = new LinearRepresentation(inst.Operation,
                                newName, inst.LeftOperand, inst.RightOperand);
                            pred.Commands.Insert(i, commonExpression);
                            
                            LinearRepresentation replacer = new LinearRepresentation(Operation.Assign,
                                command.Destination, newName);
                            pred.Commands[i + 1] = replacer;

                            Debug.Assert(eGenB[pred.Name].Contains(bInst));
                            if (!eReplaceB[block.Name].ContainsKey(bInst))
                                eReplaceB[block.Name].Add(bInst, newName);
                            eReplaceB[pred.Name].Add(bInst, newName);

                            break;
                            
                            
                        }
                    }
                    Debug.Assert(i >= 0); // equal inst was found
                }
                else
                {
                    replaceAllOccurences(ref pred, inst, bInst, ref newName, ref watched);
                    continue;
                }
            }
            
            
        }

        private bool replaceCSE(ref List<BaseBlock> blocks)
        {
            bool result = false;
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
                    IdentificatorValue iValue = null;
                    HashSet<string> wathcedBBs = new HashSet<string>();
                    wathcedBBs.Add(block.Name);
                    replaceAllOccurences(ref block, inst, cur, ref iValue, ref wathcedBBs);
                    Debug.Assert(iValue != null);
                    block.Commands[j] = new LinearRepresentation(Operation.Assign, inst.Destination, iValue);
                    result = true;
                }

            }
            return result;
        }

    }
}