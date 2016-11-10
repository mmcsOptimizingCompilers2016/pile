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

        //Queue<BaseBlock.BaseBlock> toProcess = new Queue<BaseBlock.BaseBlock>();
        //Dictionary<BaseBlock.BaseBlock, >

        Dictionary<Tuple<BaseBlock, Occurrence>, int> occToBitNumber = new Dictionary<Tuple<BaseBlock, Occurrence>, int>();
        Dictionary<BaseBlock, BitArray> generators = new Dictionary<BaseBlock, BitArray>();
        Dictionary<BaseBlock, BitArray> killers = new Dictionary<BaseBlock, BitArray>();
        Dictionary<BaseBlock, InblockDefUse> localDefUses = new Dictionary<BaseBlock, InblockDefUse>();

        private void fillOccToBitNumber(List<BaseBlock> blocks)
        {
            int counter = 0;
            foreach (var block in blocks)
            {
                for (int i=0; i < block.Commands.Count; ++i)
                {
                    var line = block.Commands[i];
                    if (line.Destination is IdentificatorValue) {
                        occToBitNumber.Add(new Tuple<BaseBlock, Tuple<int, IdentificatorValue>>(block, new Tuple<int, IdentificatorValue>(i, line.Destination as IdentificatorValue)), counter++);
                    }
                }
            }
        }

        private void fillGenerators(List<BaseBlock> blocks)
        {
            foreach(var block in blocks)
            {
                generators.Add(block, new BitArray(occToBitNumber.Count, false));

                var usedVariables = new HashSet<IdentificatorValue>();
                localDefUses.Add(block, new InblockDefUse(block));
                foreach (var ldur in localDefUses[block].result)
                {
                    //if ( usedVariables.Contains() )
                    generators[block].Set(occToBitNumber[new Tuple<BaseBlock, Occurrence>(block, ldur.Key)], true);
                }

                Console.WriteLine(block.Name + ":");
                foreach (var b in generators[block])
                {
                    //Console.Write(b.GetType());
                    var bb = b.ToString() == "True";
                    Console.Write(bb?"1":"0");
                }
                Console.WriteLine();
            }
        }


        private void iterationAlgorithm() {

        }

        public void runAnalys(List<BaseBlock> blocks) {
            fillOccToBitNumber(blocks);
            fillGenerators(blocks);
        }



    }
}
