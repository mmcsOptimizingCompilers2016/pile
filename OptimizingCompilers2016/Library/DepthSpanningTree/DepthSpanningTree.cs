using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimizingCompilers2016.Library.ControlFlowGraph;
using OptimizingCompilers2016.Library.BaseBlock;

namespace OptimizingCompilers2016.Library.DepthSpanningTree
{
    using BaseBlock = BaseBlock.BaseBlock;
    using ControlFlowGraph = ControlFlowGraph.ControlFlowGraph;

    class DepthSpanningTree
    {
        HashSet<BaseBlock> Visited;

        public DepthSpanningTree(ControlFlowGraph cfg)
        {   
            int numberOfVertices = cfg.NumberOfVertices();
            Visited = new HashSet<BaseBlock>();


        }

        private void BuildTree(BaseBlock block)
        {
            Visited.Add(block);
        }
    }
}
