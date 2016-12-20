using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimizingCompilers2016.Library.ThreeAddressCode.Values;
using OptimizingCompilers2016.Library.BaseBlock;


namespace OptimizingCompilers2016.Library.Analyses
{
    public class IDF
    {
        Dictionary<string, HashSet<BaseBlock.BaseBlock>> DF = new Dictionary<string, HashSet<BaseBlock.BaseBlock>>();
        private List<BaseBlock.BaseBlock> blocks;

        IDF()
        {
            for (int i = 0; i < blocks.Count; i++)
            {
                DF[blocks[i].Name] = new HashSet<BaseBlock.BaseBlock>();
            }

            FillDF();
        }

        private void FillDF()
        {
            var dom_relations = DOM.DOM_CREAT(blocks, blocks[0]);
            var directDominators = DOM.get_direct_dominators(dom_relations, blocks[0]);

            foreach (var block in blocks)
            {
                var directDominator = directDominators.Find(x => x.child.Name == block.Name).root;
                if (block.Predecessors.Count > 1)
                    foreach (var predecessor in block.Predecessors)
                    {
                        var r = predecessor;
                        while (r.Name != directDominator.Name)
                        {
                            DF[r.Name].Add(block);
                            r = directDominators.Find(x => x.child.Name == r.Name).root;
                        }
                    }
            }
        } 

    }
}
