using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using OptimizingCompilers2016.Library.LinearCode;
using OptimizingCompilers2016.Library.ThreeAddressCode.Values;
using OptimizingCompilers2016.Library.ThreeAddressCode;

namespace OptimizingCompilers2016.Library.Transformations
{
 
    /// Maps equivalence class of right side expression to its index in 
    /// firstPassResult list. If this equivalence class is invalidated, index equals -1
    using EquivalenceClassIndex = Dictionary<BinaryExpression, int>;

    /// <summary>
    /// represent the right side of expression, i.e in 'a := b + 3' stands for 'b + 3'
    /// </summary>
    struct BinaryExpression
    {
        Operation Operation;
        IValue LeftOperand;
        IValue RightOperand;

        public BinaryExpression(LinearRepresentation instruction)
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
        private void createOrAdd(ref Dictionary<IdentificatorValue, List<BinaryExpression>> container,
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

        private void createOrAdd(ref EquivalenceClassIndex container, BinaryExpression expression,
            int index)
        {
            if (container.ContainsKey(expression))
            {
                container[expression] = index;
            }
            else
            {
                container.Add(expression, index);
            }
        }

        private void firstPassStep(int number, LinearRepresentation instruction,
            ref List<RelevantCSEWatcher> firstPassResult,
            ref EquivalenceClassIndex expressionToEqClass,
            ref Dictionary<IdentificatorValue, List<BinaryExpression>> resultDependency)
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
                createOrAdd(ref expressionToEqClass, currentExpression, firstPassResult.Count() - 1);
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

            // remove operaions, where have current result as an operand

            Debug.Assert(instruction.Destination != null);

            if (resultDependency.ContainsKey((IdentificatorValue)instruction.Destination))
            {
                foreach (var item in resultDependency[(IdentificatorValue)instruction.Destination])
                {
                    expressionToEqClass[item] = -1;

                }
            }

        }

        private List<LinearRepresentation> secondPass(List<LinearRepresentation> original,
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
            var result = new List<LinearRepresentation>();

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

        public List<LinearRepresentation> optimize(List<LinearRepresentation> list)
        {

            var firstPassResult = new List<RelevantCSEWatcher>();
            // int is an index in firstPassResult
            var nodeIndex = new EquivalenceClassIndex();
            var resultDependency = new Dictionary<IdentificatorValue, List<BinaryExpression>>();

            for (int i = 0; i < list.Count; ++i)
            {
                firstPassStep(i, list[i], ref firstPassResult, ref nodeIndex, ref resultDependency);
            }

            return secondPass(list, firstPassResult);
        }
    }
}

