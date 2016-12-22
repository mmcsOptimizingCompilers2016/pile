using QuickGraph;
using QuickGraph.Graphviz;
using System.Collections.Generic;
using System.Linq;

namespace OptimizingCompilers2016.Library.ControlFlowGraph
{
    using DepthSpanningTree;
    using BaseBlock;

    public class ControlFlowGraph
    {
        public UndirectedGraph<BaseBlock, Edge<BaseBlock>> CFG = 
            new UndirectedGraph<BaseBlock, Edge<BaseBlock>>();

        public Dictionary<Edge<BaseBlock>, EdgeType> EdgeTypes { get; set; }

        /// <summary>
        /// Конструктор класса ControlFlowGraph
        /// </summary>
        /// <param name="blocks">Коллекция базовых блоков трёхадресного кода</param>
        public ControlFlowGraph(IEnumerable<BaseBlock> blocks)
        {
            // Добавление вершин в граф
            CFG.AddVertexRange(blocks);
            // Теперь, когда граф содержит вершины, можно добавить и дуги
            foreach (var block in blocks)
            {
                if (block.Output != null)
                {
                    CFG.AddEdge(new Edge<BaseBlock>(block, block.Output));
                }
                if (block.JumpOutput != null)
                {
                    CFG.AddEdge(new Edge<BaseBlock>(block, block.JumpOutput));
                }
            }

            EdgeTypes = new Dictionary<Edge<BaseBlock>, EdgeType>();
        }

        public BaseBlock GetRoot()
        {
            return (NumberOfVertices() > 0) ? CFG.Vertices.ElementAt(0) : null;
        }

        public int NumberOfVertices()
        {
            return CFG.Vertices.Count();
        }
        /// <summary>
        /// Сериализация графа в строку формата dot-файла для последующей визуализации с
        /// помощью graphviz
        /// </summary>
        /// <returns></returns>
        public string GenerateGraphvizDotFile()
        {
            var graphviz = new GraphvizAlgorithm<BaseBlock, Edge<BaseBlock>>(CFG);
            return graphviz.Generate();
        }
        public void Classification()
        {
            // TODO: Check when DepthSpanningTree will be fixed
            var depthTree = new DepthSpanningTree(this);
            foreach(var edge in CFG.Edges)
            {
                if (depthTree.SpanningTree.ContainsEdge(edge))
                {
                    EdgeTypes.Add(edge, EdgeType.Coming);
                }
                else if(depthTree.FindBackwardPath(edge.Source, edge.Target))
                {
                    EdgeTypes.Add(edge, EdgeType.Retreating);
                }
                else
                {
                    EdgeTypes.Add(edge, EdgeType.Cross);
                }
            }
        }
        
    }
}
