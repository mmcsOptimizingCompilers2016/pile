using System;
using System.Collections.Generic;
using QuickGraph;
using QuickGraph.Graphviz;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimizingCompilers2016.Library
{

    public class DepthSpanningTree
    {	
        HashSet<BaseBlock> Visited;
		public Dictionary<BaseBlock, int> Numbers;
		public BidirectionalGraph<BaseBlock, Edge<BaseBlock>> SpanningTree;

		/// <summary>
		/// Depth spanning tree constructor. 
		/// Bilds enumeration of blocks in reverse order.
		/// </summary>
		/// <param name="cfg">control flow graph</param>
        public DepthSpanningTree(ControlFlowGraph controlFlowGraph)
        {   
            int numberOfVertices = controlFlowGraph.NumberOfVertices() - 1;
            Visited = new HashSet<BaseBlock>();
			SpanningTree = new BidirectionalGraph<BaseBlock, Edge<BaseBlock>>();
            Numbers = new Dictionary<BaseBlock, int>();

			var rootBlock = controlFlowGraph.GetRoot();
			BuildTree(rootBlock, ref numberOfVertices);
        }

		/// <summary>
		/// Auxiliary recursive function for building depth spanning tree
		/// </summary>
		/// <param name="block">current block</param>
		/// <param name="currentNumber">counter for enumeration of blocks</param>
        private void BuildTree(BaseBlock block, ref int currentNumber)
        {
			if (block == null)
				return;

            Visited.Add(block);
			foreach (var predecessor in block.Predecessors)
			{
				if ( !Visited.Contains(predecessor) )
				{
                    if (!SpanningTree.Vertices.Contains(block))
                        SpanningTree.AddVertex(block);

                    if (!SpanningTree.Vertices.Contains(predecessor))
                        SpanningTree.AddVertex(predecessor);

					SpanningTree.AddEdge(new Edge<BaseBlock>(block, predecessor));
					BuildTree(predecessor, ref currentNumber);
				}

				Numbers[block] = currentNumber;
				currentNumber -= 1;
			}
        }
        
        public bool FindBackwardPath(BaseBlock source, BaseBlock target)
        {
            var result = false;

            var incomingEdges = SpanningTree.InEdges(source);
            while(incomingEdges.Count() > 0)
            {
                var edge = incomingEdges.First();
                if (edge.Source == target)
                {
                    result = true;
                    break;
                }
                else
                {
                    incomingEdges = SpanningTree.InEdges(edge.Source);
                }
            }

            return result;
        }

		public override string ToString()
		{
			var graphviz = new GraphvizAlgorithm<BaseBlock, Edge<BaseBlock>>(SpanningTree);
			return graphviz.Generate();
		}
	}
}
