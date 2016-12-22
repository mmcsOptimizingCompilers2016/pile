using OptimizingCompilers2016.Library.LinearCode;
using OptimizingCompilers2016.Library.ThreeAddressCode;
using OptimizingCompilers2016.Library.ThreeAddressCode.Values;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace OptimizingCompilers2016.Library.Optimizators
{
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
    
    /// Maps equivalence class of right side expression to firstPassResult values
    using BinExpToEqClass = Dictionary<BinaryExpression, RelevantCSEWatcher>;

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

        public static bool isCommutative(Operation op)
        {
            return op == Operation.Plus || op == Operation.Mult;
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
                if (!expression.Operation.Equals(this.Operation))
                {
                    return false;
                }
                if (isCommutative(Operation) &&
                    expression.LeftOperand.Equals(RightOperand) &&
                    expression.RightOperand.Equals(LeftOperand))
                {
                    return true;
                }


                return expression.LeftOperand.Equals(this.LeftOperand) &&
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
        /// supposed to be used in interblocked CSE
        public bool isValid;
        /// indecies of BaseBlock's IThreeAddressCode
        public List<int> expressions;

        public RelevantCSEWatcher(int instructionNumber)
        {
            expressions = new List<int>();
            expressions.Add(instructionNumber);

            isValid = true;
        }
        public void Invalidate()
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
    public class CommonExpressions : IOptimizator
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
            ref BinExpToEqClass expressionToEqClass,
            ref VariableOccurrence resultDependency)
        {
            if (!BinaryExpression.isModifiableOperation(instruction.Operation))
            {
                return;
            }

            var currentExpression = new BinaryExpression(instruction);

            var value = expressionToEqClass.ContainsKey(currentExpression) ?
                (RelevantCSEWatcher?)expressionToEqClass[currentExpression] : null;

            // add value to resultant set
            if (value == null)
            {
                firstPassResult.Add(new RelevantCSEWatcher(number));
                expressionToEqClass[currentExpression] = firstPassResult.Last();
            }
            else
            {
                value.Value.expressions.Add(number);
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
                    if (expressionToEqClass.ContainsKey(item))
                    {
                        expressionToEqClass[item].Invalidate();
                        expressionToEqClass.Remove(item);
                    }
                }
            }
        }

        private bool secondPass(BaseBlock block,
            List<RelevantCSEWatcher> commonExpressionsAll)
        {
            List<RelevantCSEWatcher> commonExpressions = commonExpressionsAll.
                Where(x => x.expressions.Count > 1).ToList();

            if (commonExpressions.Count == 0)
            {
                // nothing to change
                return false;
            }

            var modifiableCode = block.Commands;
            var substitution = Enumerable.Repeat<RelevantCSEWatcher?>(null, modifiableCode.Count).ToList();

            foreach (var item in commonExpressions)
            {
                foreach (var expressionId in item.expressions)
                {
                    substitution[expressionId] = item;
                }
            }
            
            // start implementing code generation
            var resultantCode = new List<IThreeAddressCode>();

            for (int i = 0; i < modifiableCode.Count; ++i)
            {
                var current = modifiableCode[i];
                if (substitution[i] != null)
                {
                    // substitute with new variable
                    var id = new IdentificatorValue("%v_" + commonExpressions.IndexOf(substitution[i].Value));
                    // create new tmp, if it wasn't created yet
                    if (substitution[i].Value.expressions.ElementAt(0) == i)
                    {
                        resultantCode.Add(new LinearRepresentation(current.Label, current.Operation,
                            id, current.LeftOperand, current.RightOperand));
                    }
                    resultantCode.Add(new LinearRepresentation(Operation.Assign, current.Destination, id));
                }
                else
                {
                    resultantCode.Add(current);
                }
            }
            block.Commands = resultantCode;
            return true;
        }

        public bool Optimize(BaseBlock block)
        {
            var firstPassResult = new List<RelevantCSEWatcher>();
            var nodeIndex = new BinExpToEqClass();
            var resultDependency = new VariableOccurrence();

            for (int i = 0; i < block.Commands.Count; ++i)
            {
                firstPassStep(i, block.Commands[i], ref firstPassResult, ref nodeIndex, ref resultDependency);
            }
            return secondPass(block, firstPassResult);
        }
    } // CommonExpressions
}