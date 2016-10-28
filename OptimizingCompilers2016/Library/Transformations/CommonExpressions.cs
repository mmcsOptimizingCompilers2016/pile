using OptimizingCompilers2016.Library.BaseBlock;
using OptimizingCompilers2016.Library.LinearCode;
using OptimizingCompilers2016.Library.ThreeAddressCode;
using OptimizingCompilers2016.Library.ThreeAddressCode.Values;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace OptimizingCompilers2016.Library.Transformations
{
    using BaseBlock = BaseBlock.BaseBlock;
    /** DEF: equivalence class of right side expression 
         right side expressions form equivalence class when:
        1) operation and operands are the same
        2) their operands aren't reassigned between these occurrences

        Example:
        d1: c := a + b
        d2: d := a + b
        d3: a := 1
        d4: e := a + b

        Classes of equivalence: {d1, d2}, {d3}, {d4}
    **/
    
    /// Set of right side expressions 
    /// alias to make iteration process readable
    using ExpressionSet = HashSet<BinaryExpression>;
    
    /// Maps equivalence class of right side expression to its index in 
    /// firstPassResult list. If this equivalence class is invalidated, index equals -1
    using EquivalenceClassIndex = Dictionary<BinaryExpression, int>;

    /// Maps variable name to its entries in class equivalence of expression
    using VariableOccurrence = Dictionary<IdentificatorValue, List<BinaryExpression>>;

    /// <summary>
    /// represent the right side of expression, i.e in 'a := b + 3' stands for 'b + 3'
    /// </summary>
    struct BinaryExpression
    {
        Operation Operation;
        IValue LeftOperand;
        IValue RightOperand;

        public BinaryExpression(IThreeAddressCode instruction)
        {
            Debug.Assert(isModifiableOperation(instruction.Operation));
            Debug.Assert(instruction.LeftOperand != null);
            Debug.Assert(instruction.RightOperand != null);
            // what about possible unary operators (that are currently lack in our compiler)?
            Operation = instruction.Operation;
            LeftOperand = instruction.LeftOperand;
            RightOperand = instruction.RightOperand;
        }

        public static bool isModifiableOperation(Operation op)
        {
            return
                op != Operation.NoOp && op != Operation.Assign &&
                op != Operation.Goto && op != Operation.CondGoto;
        }

        public override int GetHashCode()
        {
            return Operation.GetHashCode() + LeftOperand.GetHashCode() +
                RightOperand.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is BinaryExpression)
            {
                BinaryExpression expression = (BinaryExpression)obj;
                return expression.LeftOperand.Equals(this.LeftOperand) &&
                        expression.Operation.Equals(this.Operation) &&
                        expression.RightOperand.Equals(this.RightOperand);
            }
            return false;
        }
    }

    /// <summary>
    /// Structure, that keeps number of instructions, that are using the same evaluated expression
    /// isValid is not used in block expression
    /// It is supposed to help in interblock CSE, answering whether expression is still alive at the end of the block
    /// </summary>
    struct RelevantCSEWatcher
    {
        public bool isValid;
        public List<int> expressions;

        public RelevantCSEWatcher(int instructionNumber)
        {
            expressions = new List<int>();
            expressions.Add(instructionNumber);

            isValid = true;
        }
        public void invalidateMe()
        {
            isValid = false;
        }
    }

    // TODO: Resolve temporary variable name conflicts
    // possible solution: create global class with method, returning serial number for naming variables
    /// <summary>
    /// Transformation consists of two passes
    /// First pass finds all common sequences and split it into the sets of subexpressions,
    /// that could be calculated once
    /// Second pass inserts additional temporary variables, used for common subexpressions
    /// </summary>
    public class CommonExpressions
    {
        private void createOrAdd(ref VariableOccurrence container,
                                 IdentificatorValue id,
                                 BinaryExpression expr)
        {
            if (container.ContainsKey(id))
            {
                container[id].Add(expr);
            }
            else
            {
                var lst = new List<BinaryExpression>();
                lst.Add(expr);
                container.Add(id, lst);
            }
        }

		
        private void firstPassStep(int number, IThreeAddressCode instruction,
            ref List<RelevantCSEWatcher> firstPassResult,
            ref EquivalenceClassIndex expressionToEqClass,
            ref VariableOccurrence resultDependency)
        {
            if (!BinaryExpression.isModifiableOperation(instruction.Operation))
            {
                return;
            }

            var currentExpression = new BinaryExpression(instruction);

            int value = expressionToEqClass.ContainsKey(currentExpression) ? expressionToEqClass[currentExpression] : -1;

            // add value to resultant set
            if (value == -1)
            {
                firstPassResult.Add(new RelevantCSEWatcher(number));
                expressionToEqClass[currentExpression] = firstPassResult.Count() - 1;
            }
            else
            {
                firstPassResult[value].expressions.Add(number);
            }

            if (instruction.LeftOperand is IdentificatorValue)
            {
                createOrAdd(ref resultDependency, (IdentificatorValue)instruction.LeftOperand, currentExpression);
            }
            if (instruction.RightOperand is IdentificatorValue)
            {
                createOrAdd(ref resultDependency, (IdentificatorValue)instruction.RightOperand, currentExpression);
            }

            // remove operations, which have current result as an operand

            Debug.Assert(instruction.Destination != null);

            if (resultDependency.ContainsKey((IdentificatorValue)instruction.Destination))
            {
                foreach (var item in resultDependency[(IdentificatorValue)instruction.Destination])
                {
                    expressionToEqClass[item] = -1;

                }
            }

        }

        private List<IThreeAddressCode> secondPass(List<IThreeAddressCode> original,
            List<RelevantCSEWatcher> commonExpressions)
        {
            var insertedTmps = new HashSet<int>();
            var substitution = Enumerable.Repeat(-1, original.Count).ToList();

            for (int i = 0; i < commonExpressions.Count; ++i)
            {
                if (commonExpressions[i].expressions.Count <= 1)
                {
                    continue;
                }
                foreach (var inner in commonExpressions[i].expressions)
                {
                    substitution[inner] = i;
                }
            }

            // start implementing code generation
            var result = new List<IThreeAddressCode>();

            for (int i = 0; i < original.Count; ++i)
            {
                var current = original[i];
                // substitute with new variable
                if (substitution[i] != -1)
                {
                    var id = new IdentificatorValue("%v_" + substitution[i]);
                    if (!insertedTmps.Contains(substitution[i]))
                    {
                        result.Add(new LinearRepresentation(current.Label, current.Operation,
                            id, current.LeftOperand, current.RightOperand));
                        insertedTmps.Add(substitution[i]);
                    }
                    result.Add(new LinearRepresentation(Operation.Assign, current.Destination, id));
                }
                else
                {
                    result.Add(current);
                }
            }

            return result;
        }

        public BaseBlock optimize(BaseBlock block)
        {
            List<IThreeAddressCode> list = block.Commands;
            var firstPassResult = new List<RelevantCSEWatcher>();
            // int is an index in firstPassResult
            var nodeIndex = new EquivalenceClassIndex();
            var resultDependency = new VariableOccurrence();

            for (int i = 0; i < list.Count; ++i)
            {
                firstPassStep(i, list[i], ref firstPassResult, ref nodeIndex, ref resultDependency);
            }
            BaseBlock result = new BaseBlock();
            result.Commands.AddRange(secondPass(list, firstPassResult));
            return result;
        }
		
/// Available Expressions Analysis:

        /// <summary>
        /// Optimize program using the results of available expressions analysis
        /// @param blocks - list of all blocks in the program + fictional source block
        /// @return block representation of the optimized program
        /// </summary>
        public List<BaseBlock> optimize(ref List<BaseBlock> blocks)
        {   
            List<BaseBlock> result = new List<BaseBlock>();

            // TODO: apply iterational algorithm to find the list of available expressions
            iterationalAlgorithm(ref blocks);
            
            // TODO: do something
            return result;
        }   
        
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
            
            foreach ( IThreeAddressCode instruction in instructionsInBlock )
            {
                if (!BinaryExpression.isModifiableOperation(instruction.Operation)) { continue; }

                var currentExpression = new BinaryExpression(instruction);
                result.Add(currentExpression);
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
            
            foreach ( BaseBlock block in blocks )
            {   
                // TODO: add comparator?
                result.Union( extractAllExpressions(block) );
            }
            
            return result;
        }
        
        private void initOutB(ref ExpressionSet[] outB, ref List<BaseBlock> blocks)
        {   
            ExpressionSet allExpressions = extractAllExpressions(ref blocks);
            
            for ( var i = 0; i < outB.Count(); i++ )
            {
                outB[i] = new ExpressionSet(allExpressions);
            }
        }
        
        /// <summary>
        /// Update input expressions set for a current block
        ///     IN[B] = П OUT[P] for all B.predecessors
        ///
        /// @param inB - input expressions set
        /// @param currentBlock
        /// @param changed - status of the expression set (whether it was altered by this func)
        /// </summary>
        private void updateInB( ref ExpressionSet inB, BaseBlock currentBlock, ref bool changed)
        {   
            // TODO: implement me
        }
        
        /// <summary>
        /// Update output expressions set for a current block
        ///     OUT[B] EgenB OR (IN[B] \ EkillB)
        ///
        /// @param outB - output expressions set
        /// @param currentBlock
        /// @param changed - status of the expression set (whether it was altered by this func)
        /// </summary>
        private void updateOutB( ref ExpressionSet outB, BaseBlock currentBlock, ref bool changed)
        {   
            // TODO: implement me
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
        private void iterationalAlgorithm(ref List<BaseBlock> blocks) 
        {   
            // TODO: consider using Dictionary< BaseBlock, ExpressionSet > instead of ExpressionSet[]
            ExpressionSet[] inB = new ExpressionSet[blocks.Count];
            ExpressionSet[] outB = new ExpressionSet[blocks.Count];

            for (var i = 0; i < blocks.Count; i++) 
            {   
                // init OUT[B] with all the expressions
                // TODO: find out - should it be done outside the loop?
                initOutB(ref outB, ref blocks);

                // while OUT[B] changes do
                bool changed = true;
                while ( changed )
                {
                    for (var j = 0; j < blocks.Count; j++)
                    {   
                    // IN[B] = П OUT[P] for all B.predecessors
                        updateInB(ref inB[j], blocks[i], ref changed);
                    // OUT[B] EgenB OR (IN[B] \ EkillB)
                        updateOutB(ref outB[j], blocks[i], ref changed);
                    }
                }
            }
        }       
    }
}