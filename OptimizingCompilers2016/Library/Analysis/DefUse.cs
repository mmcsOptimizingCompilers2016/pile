using OptimizingCompilers2016.Library.LinearCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Occurrence = System.Tuple<int, OptimizingCompilers2016.Library.ThreeAddressCode.Values.IdentificatorValue>;
using DefsMap = System.Collections.Generic.Dictionary<OptimizingCompilers2016.Library.ThreeAddressCode.Values.IdentificatorValue, System.Tuple<int, OptimizingCompilers2016.Library.ThreeAddressCode.Values.IdentificatorValue>>;
using OptimizingCompilers2016.Library.ThreeAddressCode.Values;
using OptimizingCompilers2016.Library;
using System.Collections;
using OptimizingCompilers2016.Library.ThreeAddressCode;

namespace OptimizingCompilers2016.Library.Analysis
{

    public class InblockDefUse
    {
        public Dictionary<Occurrence, HashSet<Occurrence>> result { get; set; } = new Dictionary<Occurrence, HashSet<Occurrence>>();
        DefsMap lastDef = new DefsMap();

        private void setLastDef(IdentificatorValue variable, Occurrence occurrence)
        {
            if ( !lastDef.ContainsKey(variable) )
            {
                lastDef.Add(variable, occurrence);
            }
            else
            {
                lastDef[variable] = occurrence;
            }
            result.Add(lastDef[variable], new HashSet<Occurrence>());
        }

        private bool checkLastDefs(IdentificatorValue occ)
        {
            if (!lastDef.ContainsKey(occ))
            {
                Console.WriteLine("Warning: There isn't such variable: " + occ.ToString());
                return false;
            }
            else
                return true;
        }

        private void addUse(IValue term, int index)
        {
            if (!(term is IdentificatorValue))
                return;

            var variable = term as IdentificatorValue;

            if ( checkLastDefs(variable))
            {
                result[lastDef[variable]].Add(new Occurrence(index, variable));
            }
        }


        private Dictionary<Occurrence, HashSet<Occurrence>> runAnalys(List<IThreeAddressCode> code)
        {
            
            int index = 0;
            foreach ( var line in code )
            {
                addUse(line.LeftOperand, index);
                addUse(line.RightOperand, index);

                if (line.Destination is IdentificatorValue)
                    setLastDef(line.Destination as IdentificatorValue, new Occurrence(index, line.Destination as IdentificatorValue));

                index++;
            }

            return result;
        }

        public InblockDefUse(BaseBlock block)
        {
            runAnalys(block.Commands);
        }

        public override string ToString()
        {
            var defUseString = result.Select(item => item.Key + " => {" + String.Join(", ", item.Value) + "}");
            return String.Join("\n", defUseString);
        }

    }

    public class GlobalDefUse
    {

        //Queue<BaseBlock> toProcess = new Queue<BaseBlock>();
        //Dictionary<BaseBlock, >

        Dictionary<Tuple<BaseBlock, Occurrence>, int> occToBitNumber = new Dictionary<Tuple<BaseBlock, Occurrence>, int>();
        Dictionary<BaseBlock, BitArray> generators = new Dictionary<BaseBlock, BitArray>();
        Dictionary<BaseBlock, BitArray> killers = new Dictionary<BaseBlock, BitArray>();
        Dictionary<BaseBlock, InblockDefUse> localDefUses = new Dictionary<BaseBlock, InblockDefUse>();

        private void fillSupportingStructures(List<BaseBlock> blocks)
        {
            int counter = 0;
            foreach (var block in blocks)
            {
                Console.WriteLine(block.Name + " : " + new InblockDefUse(block).ToString());
                localDefUses.Add(block, new InblockDefUse(block));
                for (int i=0; i < block.Commands.Count; ++i)
                {
                    var line = block.Commands[i];
                    if (line.Destination is IdentificatorValue) {
                        occToBitNumber.Add(new Tuple<BaseBlock, Tuple<int, IdentificatorValue>>(block, new Tuple<int, IdentificatorValue>(i, line.Destination as IdentificatorValue)), counter++);
                    }
                }
            }
        }

        private void fillGeneratorsAndKillers(List<BaseBlock> blocks)
        {
            foreach(var block in blocks)
            {
                generators.Add(block, new BitArray(occToBitNumber.Count, false));
                killers.Add(block, new BitArray(occToBitNumber.Count, false));
                foreach (var ldur in localDefUses[block].result)
                {
                    generators[block].Set(occToBitNumber[new Tuple<BaseBlock, Occurrence>(block, ldur.Key)], true);

                    var variable = ldur.Key.Item2;
                    //check if variable is used somewhere in another block
                    foreach (var block2 in blocks)
                    {
                        if (block != block2)
                        {
                            foreach (var ldur2 in localDefUses[block2].result)
                            {
                                if (ldur2.Key.Item2.Equals(variable))
                                {
                                    killers[block].Set(occToBitNumber[new Tuple<BaseBlock, Occurrence>(block2, ldur2.Key)], true);
                                }
                            }
                        }
                    }
                }
                Console.WriteLine(block.Name);
                Console.WriteLine("Gen: " + bitArrayToString(generators[block]));
                Console.WriteLine("Kill: " + bitArrayToString(killers[block]));
            }
        }

        private String bitArrayToString(BitArray arr) {
            String fullString = "";
            foreach (var bit in arr)
            {
                fullString += bit.ToString() == "True" ? "1" : "0";
            }
            return fullString;
        }

        private void iterationAlgorithm(List<BaseBlock> blocks) {
            Dictionary<BaseBlock, BitArray> outs = new Dictionary<BaseBlock, BitArray>();
            Dictionary<BaseBlock, BitArray> ins = new Dictionary<BaseBlock, BitArray>();
            Dictionary<BaseBlock, BitArray> prevOuts = new Dictionary<BaseBlock, BitArray>();
            foreach (var block in blocks)
            {
                outs.Add(block, new BitArray(occToBitNumber.Count, false));
                prevOuts.Add(block, new BitArray(occToBitNumber.Count, true));
                ins.Add(block, new BitArray(occToBitNumber.Count, false));
            }

            var prevBlock = blocks[0];
            while (areDifferent(outs, prevOuts))
            {
                foreach (var block in blocks) {
                    ins[block] = ins[block].Or(outs[prevBlock]);
                    outs[block] = generators[block].Or(ins[block]);
                    prevBlock = block;
                }
           }
        }

        private bool areDifferent(Dictionary<BaseBlock, BitArray>  currentOuts, Dictionary<BaseBlock, BitArray> prevOuts) {
            return true;
        }


        public void runAnalys(List<BaseBlock> blocks) {
            fillSupportingStructures(blocks);
            fillGeneratorsAndKillers(blocks);
        }



    }
}
