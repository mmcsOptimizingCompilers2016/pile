//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Diagnostics;
//using OptimizingCompilers2016.Library.LinearCode;

//namespace OptimizingCompilers2016.Library.Transformations
//{
    

//    /// for right side of expression
//    struct BinaryExpression
//    {
//        Operation operation;
//        Value leftOperand;
//        Value rightOperand;

//        public BinaryExpression(LinearRepresentation instruction)
//        {
//            Debug.Assert(isModifiableOperation(instruction.operation));
//            Debug.Assert(instruction.leftOperand != null);
//            Debug.Assert(instruction.rightOperand != null);
//            // what about possible unary operators (that are currently lack in our compiler)?
//            operation = instruction.operation;
//            leftOperand = instruction.leftOperand;
//            rightOperand = instruction.rightOperand;
//        }

//        public static bool isModifiableOperation(Operation op)
//        {
//            return 
//                op != Operation.NoOp && op != Operation.Assign &&
//                op != Operation.Goto && op != Operation.CondGoto;
//        }

//        public override int GetHashCode()
//        {
//            return operation.ToString().GetHashCode() + leftOperand.ToString().GetHashCode() +
//                rightOperand.ToString().GetHashCode();
//        }

//        public override bool Equals(object obj)
//        {
//            if (obj is BinaryExpression)
//            {
//                return (BinaryExpression)obj == this;
//            }
//            return false;
//        }

//        public static bool operator ==(BinaryExpression b1, BinaryExpression b2)
//        {
//            return b1.operation.ToString() == b2.operation.ToString() &&
//                b1.leftOperand.ToString() == b2.leftOperand.ToString() &&
//                b1.rightOperand.ToString() == b2.rightOperand.ToString();
//        }
//        public static bool operator !=(BinaryExpression b1, BinaryExpression b2)
//        {
//            return b1.operation.ToString() != b2.operation.ToString() ||
//                b1.leftOperand.ToString() != b2.leftOperand.ToString() ||
//                b1.rightOperand.ToString() != b2.rightOperand.ToString();
//        }
//    }


//    struct RelevantCSEWatcher
//    {
//        public bool isValid;
//        public List<int> expressions;

//        public RelevantCSEWatcher(int instructionNumber)
//        {
//            expressions = new List<int>();
//            expressions.Add(instructionNumber);

//            isValid = true;
//        }
//        public void invalidateMe()
//        {
//            isValid = false;
//        }
//    }

//    // TODO: Resolve temporary variable name conflicts
//    // possible solution: create global class with method, returning serial number for naming variables
//    /// <summary>
//    /// Transformation consists of two passes
//    /// First pass finds all common sequences, that could be not calculated
//    /// Second pass inserts additional variables, used for common subexpressions
//    /// </summary>
//    public class CommonExpressions
//    {
//        private void createOrAdd(ref Dictionary<Identificator, List<BinaryExpression>> container, Identificator id,
//            BinaryExpression expr)
//        {
//            if (container.ContainsKey(id))
//            {
//                container[id].Add(expr);
//            }
//            else
//            {
//                var lst = new List<BinaryExpression>();
//                lst.Add(expr);
//                container.Add(id, lst);
//            }
//        }

//        private void createOrAdd(ref Dictionary<BinaryExpression, int> container, BinaryExpression expression,
//            int index)
//        {
//            if (container.ContainsKey(expression))
//            {
//                container[expression] = index;
//            }
//            else
//            {
//                container.Add(expression, index);
//            }
//        }

//        private void firstPassStep(int number, LinearRepresentation instruction,
//            ref List<RelevantCSEWatcher> firstPassResult,
//            ref Dictionary<BinaryExpression, int> nodeIndex,
//            ref Dictionary<Identificator, List<BinaryExpression>> resultDependency)
//        {
//            if (!BinaryExpression.isModifiableOperation(instruction.operation))
//            {
//                return;
//            }

//            var currentExpression = new BinaryExpression(instruction);

//            int value = nodeIndex.ContainsKey(currentExpression) ? nodeIndex[currentExpression] : - 1;

//            // add value to resultant set
//            if (value == -1)
//            {
//                firstPassResult.Add(new RelevantCSEWatcher(number));
//                createOrAdd(ref nodeIndex, currentExpression, firstPassResult.Count() - 1);
//            }
//            else
//            {
//                firstPassResult[value].expressions.Add(number);
//            }

//            createOrAdd(ref resultDependency, (Identificator)instruction.leftOperand, currentExpression);
//            createOrAdd(ref resultDependency, (Identificator)instruction.rightOperand, currentExpression);

//            // remove operaions, where have current result as an operand

//            Debug.Assert(instruction.destination != null);

//            if (resultDependency.ContainsKey((Identificator)instruction.destination))
//            {
//                foreach (var item in resultDependency[(Identificator)instruction.destination])
//                {
//                    nodeIndex[item] = -1;
                    
//                }
//            }

//        }

//        //private void removeOperation(int number, LinearRepresentation instruction,
//        //    ref List<RelevantCSEWatcher> firstPassResult,
//        //    ref Dictionary<BinaryExpression, int> nodeIndex,
//        //    ref Dictionary<Identificator, List<BinaryExpression>> resultDependency)
//        //{
            
//        //    foreach (var item in collection)
//        //    {

//        //    }
//        //}
        

//        private List<LinearRepresentation> secondPass(List<LinearRepresentation> original,
//            List<RelevantCSEWatcher> equalInstructions)
//        {
//            var insertedTmps = new HashSet<int>();
//            var substitution = Enumerable.Repeat(-1, original.Count).ToList();

//            for (int i = 0; i < equalInstructions.Count; ++i)
//            {
//                if (equalInstructions[i].expressions.Count <= 1)
//                {
//                    continue;
//                }
//                foreach (var inner in equalInstructions[i].expressions)
//                {
//                    substitution[inner] = i;
//                }
//            }

//            // start implementing code generation
//            var result = new List<LinearRepresentation>();
            
//            for (int i = 0; i < original.Count; ++i)
//            {
//                var current = original[i];
//                // substitute with new variable
//                if (substitution[i] != -1)
//                {
//                    var id = new Identificator("%l_" + substitution[i]);
//                    if (!insertedTmps.Contains(substitution[i]))
//                    {
//                        result.Add(new LinearRepresentation(current.label, current.operation,
//                            id, current.leftOperand, current.rightOperand));
//                        insertedTmps.Add(substitution[i]);
//                    }
//                    result.Add(new LinearRepresentation(Operation.Assign, current.destination, id));
//                }
//                else
//                {
//                    result.Add(current);
//                }
//            }
            
//            return result;
//        }

//        //static private Dictionary<>
//        public List<LinearRepresentation> optimize(List<LinearRepresentation> list)
//        {
//            var result = new List<LinearRepresentation>();

//            var firstPassResult = new List<RelevantCSEWatcher>();
//            // int is an index in firstPassResult
//            var nodeIndex = new Dictionary<BinaryExpression, int>();
//            var resultDependency = new Dictionary<Identificator, List<BinaryExpression>>();

//            for (int i = 0; i < list.Count; ++i)
//            {
//                firstPassStep(i, list[i], ref firstPassResult, ref nodeIndex, ref resultDependency);
//            }

//            return secondPass(list, firstPassResult);
//        }
//    }
//}

