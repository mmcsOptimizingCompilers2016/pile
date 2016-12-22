using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimizingCompilers2016.Library.ThreeAddressCode.Values;
using OptimizingCompilers2016.Library.BaseBlock;


namespace OptimizingCompilers2016.Library.Analyses
{
    public class DominanceFrontier
    {
        public Dictionary<string, HashSet<string>> DF = new Dictionary<string, HashSet<string>>();
        public Dictionary<string, HashSet<string>> IDF = new Dictionary<string, HashSet<string>>();

        private List<BaseBlock.BaseBlock> blocks;

        public DominanceFrontier(List<BaseBlock.BaseBlock> blocks)
        {
            this.blocks = blocks;
            for (int i = 0; i < blocks.Count; i++)
            {
                DF[blocks[i].Name] = new HashSet<string>();
            }

            FillDF();
        }

        private void FillDF()
        {
            var dom_relations = DOM.DOM_CREAT(blocks, blocks[0]);
            var directDominators = DOM.get_direct_dominators(dom_relations, blocks[0]);

            foreach (var block in blocks)
            {
                if (block.Name != "B0")
                {
                    var directDominator = directDominators.Find(x => x.child.Name == block.Name).root;
                    if (block.Predecessors.Count > 1)
                        foreach (var predecessor in block.Predecessors)
                        {
                            var r = predecessor;
                            while (r.Name != directDominator.Name)
                            {
                                DF[r.Name].Add(block.Name);
                                r = directDominators.Find(x => x.child.Name == r.Name).root;
                            }
                        }
                }
            }
        }

        public HashSet<string> ComputeIDF(List<BaseBlock.BaseBlock> blocks)
        {
            HashSet<string> IDF = new HashSet<string>();
            HashSet<string> oldIDF = new HashSet<string>();
            HashSet<string> DF_S = new HashSet<string>();
            HashSet<string> S = new HashSet<string>();

            //for (int i = 0; i < iterations; i++)
            //{
            //    IDF.Add(new HashSet<string>());
            //}

            foreach (var block in blocks)
            {
                S.Add(block.Name);
                DF_S.UnionWith(DF[block.Name]);
            }

            IDF = new HashSet<string>(DF_S);
            oldIDF = new HashSet<string>(IDF);

            bool flag = true;

            while (flag)
            {
                DF_S = new HashSet<string>();
                var tempS = new HashSet<string>(S);
                tempS.UnionWith(IDF);
                foreach (var nameBlock in tempS)
                    DF_S.UnionWith(DF[nameBlock]);

                IDF = new HashSet<string>(DF_S);

                if (IDF.SetEquals(oldIDF))
                    flag = false;

                oldIDF = new HashSet<string>(IDF);
            }

            return IDF;

        }

        public HashSet<string> ComputeIDF(BaseBlock.BaseBlock block)
        {
            HashSet<string> IDF = new HashSet<string>();
            HashSet<string> oldIDF = new HashSet<string>();
            HashSet<string> DF_S = new HashSet<string>();
            HashSet<string> S = new HashSet<string>();

            //for (int i = 0; i < iterations; i++)
            //{
            //    IDF.Add(new HashSet<string>());
            //}

            S.Add(block.Name);
            DF_S.UnionWith(DF[block.Name]);

            IDF = new HashSet<string>(DF_S);
            oldIDF = new HashSet<string>(IDF);

            bool flag = true;

            while (flag)
            {
                DF_S = new HashSet<string>();
                var tempS = new HashSet<string>(S);
                tempS.UnionWith(IDF);
                foreach (var nameBlock in tempS)
                    DF_S.UnionWith(DF[nameBlock]);

                IDF = new HashSet<string>(DF_S);

                if (IDF.SetEquals(oldIDF))
                    flag = false;

                oldIDF = new HashSet<string>(IDF);
            }

            return IDF;

        }

        public override string ToString()
        {
            var res = "";

            foreach (var block in DF)
            {

                res += "DF(" + block.Key + ")= {" + string.Join(", ", block.Value) + "}";

                res += "\n";
            }

            res += "\n";

            //res += "IDF({" + string.Join(", ", blocks.Select((BaseBlock.BaseBlock bl) => { return bl.Name; })) + "}) = {"+  string.Join(", ", IDF) + "}";

            return res;
        }

    }
}
