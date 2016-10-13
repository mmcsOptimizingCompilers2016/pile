using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using OptimizingCompilers2016.Library.LinearCode;
using OptimizingCompilers2016.Library.ThreeAddressCode.Values;
using OptimizingCompilers2016.Library.ThreeAddressCode;

namespace OptimizingCompilers2016.Library.Transformations
{


    /// for right side of expression
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
            return Operation.ToString().GetHashCode() + LeftOperand.ToString().GetHashCode() +
                RightOperand.ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is BinaryExpression)
            {
                return (BinaryExpression)obj == this;
            }
            return false;
        }

        public static bool operator ==(BinaryExpression b1, BinaryExpression b2)
        {
            return b1.Operation.ToString() == b2.Operation.ToString() &&
                b1.LeftOperand.ToString() == b2.LeftOperand.ToString() &&
                b1.RightOperand.ToString() == b2.RightOperand.ToString();
        }
        public static bool operator !=(BinaryExpression b1, BinaryExpression b2)
        {
            return b1.Operation.ToString() != b2.Operation.ToString() ||
                b1.LeftOperand.ToString() != b2.LeftOperand.ToString() ||
                b1.RightOperand.ToString() != b2.RightOperand.ToString();
        }
    }


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
    /// First pass finds all common sequences, that could be not calculated
    /// Second pass inserts additional variables, used for common subexpressions
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

        private void createOrAdd(ref Dictionary<BinaryExpression, int> container, BinaryExpression expression,
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
            ref Dictionary<BinaryExpression, int> nodeIndex,
            ref Dictionary<IdentificatorValue, List<BinaryExpression>> resultDependency)
        {
            if (!BinaryExpression.isModifiableOperation(instruction.Operation))
            {
                return;
            }

            var currentExpression = new BinaryExpression(instruction);

            int value = nodeIndex.ContainsKey(currentExpression) ? nodeIndex[currentExpression] : -1;

            // add value to resultant set
            if (value == -1)
            {
                firstPassResult.Add(new RelevantCSEWatcher(number));
                createOrAdd(ref nodeIndex, currentExpression, firstPassResult.Count() - 1);
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
                    nodeIndex[item] = -1;

                }
            }

        }

        //private void removeOperation(int number, LinearRepresentation instruction,
        //    ref List<RelevantCSEWatcher> firstPassResult,
        //    ref Dictionary<BinaryExpression, int> nodeIndex,
        //    ref Dictionary<Identificator, List<BinaryExpression>> resultDependency)
        //{

        //    foreach (var item in collection)
        //    {

        //    }
        //}


        private List<LinearRepresentation> secondPass(List<LinearRepresentation> original,
            List<RelevantCSEWatcher> equalInstructions)
        {
            var insertedTmps = new HashSet<int>();
            var substitution = Enumerable.Repeat(-1, original.Count).ToList();

            for (int i = 0; i < equalInstructions.Count; ++i)
            {
                if (equalInstructions[i].expressions.Count <= 1)
                {
                    continue;
                }
                foreach (var inner in equalInstructions[i].expressions)
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

        //static private Dictionary<>
        public List<LinearRepresentation> optimize(List<LinearRepresentation> list)
        {
            var result = new List<LinearRepresentation>();

            var firstPassResult = new List<RelevantCSEWatcher>();
            // int is an index in firstPassResult
            var nodeIndex = new Dictionary<BinaryExpression, int>();
            var resultDependency = new Dictionary<IdentificatorValue, List<BinaryExpression>>();

            for (int i = 0; i < list.Count; ++i)
            {
                firstPassStep(i, list[i], ref firstPassResult, ref nodeIndex, ref resultDependency);
            }

            return secondPass(list, firstPassResult);
        }
    }
}

