using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimizingCompilers2016.Library.ThreeAddressCode.Values;
using Occurrence = System.Tuple<int, OptimizingCompilers2016.Library.ThreeAddressCode.Values.IdentificatorValue>;


namespace OptimizingCompilers2016.Library.Analysis
{
    public class ActiveVariables
    {
        public new Dictionary<string, HashSet<IdentificatorValue>> result = new Dictionary<string, HashSet<IdentificatorValue>>();

        private Dictionary<string, HashSet<IdentificatorValue>> IN = new Dictionary<string, HashSet<IdentificatorValue>>();
        private Dictionary<string, HashSet<IdentificatorValue>> OUT = new Dictionary<string, HashSet<IdentificatorValue>>();
        private Dictionary<string, HashSet<IdentificatorValue>> Def = new Dictionary<string, HashSet<IdentificatorValue>>();
        private Dictionary<string, HashSet<IdentificatorValue>> Use = new Dictionary<string, HashSet<IdentificatorValue>>();
        private List<BaseBlock> blocks;

        public ActiveVariables(List<BaseBlock> blocks)
        {
            this.blocks = blocks;
            FillDefUse();
        }

        private void FillDefUse()
        {
            for (int i = 0; i < blocks.Count; i++)
            {
                var ldu = new InblockDefUse(blocks[i]);
                var Set = new HashSet<IdentificatorValue>();
                foreach (var key in ldu.result.Keys)
                    Set.Add(new IdentificatorValue(key.Item2.Value));
                Def[blocks[i].Name] = Set;
            }

            for (int i = 0; i < blocks.Count; i++)
            {
                var ldu = new InblockDefUse(blocks[i]);
                var Set = new HashSet<IdentificatorValue>();
                foreach (var sets in ldu.result.Values)
                    foreach (var itemSet in sets)
                        Set.Add(new IdentificatorValue(itemSet.Item2.Value));
                Use[blocks[i].Name] = Set;
            }

            for (int i = 0; i < blocks.Count; i++)
            {
                IN[blocks[i].Name] = new HashSet<IdentificatorValue>();
                OUT[blocks[i].Name] = new HashSet<IdentificatorValue>();
            }
        }

        public Dictionary<string, HashSet<IdentificatorValue>> runAnalys()
        {
            var oldIN = new Dictionary<string, HashSet<IdentificatorValue>>(IN);

            while (true)
            {
                var count = 0;
                //Console.WriteLine("Ахахахха)000))");
                for (int i = 0; i < blocks.Count; i++)
                {
                    OUT[blocks[i].Name] = new HashSet<IdentificatorValue>();

                    var successors = new List<BaseBlock>();
                    successors.Add(blocks[i].Output);
                    successors.Add(blocks[i].JumpOutput);

                    foreach (var successor in successors)
                    {
                        if (successor != null)
                            OUT[blocks[i].Name].UnionWith(IN[successor.Name]);
                    }

                    OUT[blocks[i].Name].ExceptWith(Def[blocks[i].Name]);
                    OUT[blocks[i].Name].UnionWith(Use[blocks[i].Name]);
                    IN[blocks[i].Name] = new HashSet<IdentificatorValue>(OUT[blocks[i].Name]);
                }

                for (int i = 0; i < oldIN.Count(); i++)
                {
                    if (!oldIN[blocks[i].Name].SetEquals(IN[blocks[i].Name]))
                        count++;
                }

                if (count == 0)
                    break;

                oldIN = new Dictionary<string, HashSet<IdentificatorValue>>(IN);
            }

            result = new Dictionary<string, HashSet<IdentificatorValue>>(IN);

            return result;
        }

        public override string ToString()
        {
            var res = "";
            foreach (var block in result)
            {
                res += block.Key;
                res += ":";
                foreach (var ident in block.Value)
                {
                    res += ident.Value;
                    res += " ";
                }
                res += "\n";

            }

            return res;
        }

        //private void FillDefUse()
        //{
        //    for (int i = 0; i < blocks.Count; i++)
        //    {
        //        var ldu = new InblockDefUse(blocks[i]);
        //        var Set = new HashSet<Occurrence>();
        //        foreach (var key in ldu.result.Keys)
        //            Set.Add(key);
        //        Def[blocks[i].Name] = Set;
        //    }

        //    for (int i = 0; i < blocks.Count; i++)
        //    {
        //        var ldu = new InblockDefUse(blocks[i]);
        //        var Set = new HashSet<Occurrence>();
        //        foreach (var sets in ldu.result.Values)
        //            foreach (var itemSet in sets)
        //                Set.Add(itemSet);
        //        Use[blocks[i].Name] = Set;
        //    }

        //    for (int i = 0; i < blocks.Count; i++)
        //    {
        //        IN[blocks[i].Name] = new HashSet<Occurrence>();
        //        OUT[blocks[i].Name] = new HashSet<Occurrence>();
        //    }
        //}

        //public Dictionary<string, HashSet<Occurrence>> runAnalys()
        //{
        //    var oldIN = new Dictionary<string, HashSet<Occurrence>>(IN);

        //    while (true)
        //    {
        //        var isChanged = false;
        //        //Console.WriteLine("Ахахахха)000))");
        //        for (int i = 0; i < blocks.Count; i++)
        //        {
        //            OUT[blocks[i].Name] = new HashSet<Occurrence>();

        //            var successors = new List<BaseBlock>();
        //            successors.Add(blocks[i].Output);
        //            successors.Add(blocks[i].JumpOutput);


        //            //foreach (var p in blocks[i].Predecessors)
        //            //{
        //            //    if (p.Name!= "B0")
        //            //    OUT[blocks[i].Name].UnionWith(IN[p.Name]);
        //            //}


        //            foreach (var successor in successors)
        //            {
        //                if (successor != null)
        //                    OUT[blocks[i].Name].UnionWith(IN[successor.Name]);
        //            }

        //            OUT[blocks[i].Name].ExceptWith(Def[blocks[i].Name]);
        //            OUT[blocks[i].Name].UnionWith(Use[blocks[i].Name]);
        //            IN[blocks[i].Name] = new HashSet<Occurrence>(OUT[blocks[i].Name]);
        //        }

        //        for (int i = 0; i < oldIN.Count(); i++)
        //        {
        //            if (!oldIN[blocks[i].Name].SetEquals(IN[blocks[i].Name]))
        //                isChanged = true;
        //        }

        //        if (!isChanged)
        //            break;

        //        oldIN = new Dictionary<string, HashSet<Occurrence>>(IN);
        //    }

        //    result = IN;

        //    return result;
        //}


    }
}
