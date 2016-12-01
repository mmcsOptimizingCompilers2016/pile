using QuickGraph;
using QuickGraph.Graphviz;
using System.Collections.Generic;
using System.Linq;
using OptimizingCompilers2016.Library.DepthSpanningTree;

namespace OptimizingCompilers2016.Library.ControlFlowGraph
{
    using DepthSpanningTree = DepthSpanningTree.DepthSpanningTree;

    public class ControlFlowGraph
    {


        BidirectionalGraph<BaseBlock.BaseBlock, Edge<BaseBlock.BaseBlock>> CFG = 
            new BidirectionalGraph<BaseBlock.BaseBlock, Edge<BaseBlock.BaseBlock>>();

        /// <summary>
        /// Конструктор класса ControlFlowGraph
        /// </summary>
        /// <param name="blocks">Коллекция базовых блоков трёхадресного кода</param>
        public ControlFlowGraph(IEnumerable<BaseBlock.BaseBlock> blocks)
        {
            // Добавление вершин в граф
            CFG.AddVertexRange(blocks);
            // Теперь, когда граф содержит вершины, можно добавить и дуги
            foreach (var block in blocks)
            {
                if (block.Output != null)
                {
                    CFG.AddEdge(new Edge<BaseBlock.BaseBlock>(block, block.Output));
                }
                if (block.JumpOutput != null)
                {
                    CFG.AddEdge(new Edge<BaseBlock.BaseBlock>(block, block.JumpOutput));
                }
            }
        }

        public BaseBlock.BaseBlock GetRoot()
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
            var graphviz = new GraphvizAlgorithm<BaseBlock.BaseBlock, Edge<BaseBlock.BaseBlock>>(CFG);
            return graphviz.Generate();
        }

        
        List<Edge<BaseBlock.BaseBlock>> BackEdges = new List<Edge<BaseBlock.BaseBlock>>();

        List<Edge<BaseBlock.BaseBlock>> RetreatingEdges = new List<Edge<BaseBlock.BaseBlock>>();


        public bool CheckReducibility() {
            DepthSpanningTree DFST = new DepthSpanningTree(this);
           
            var BackEdges = new HashSet<Edge<BaseBlock.BaseBlock>>(this.BackEdges);
            var RetreatingEdges = new HashSet<Edge<BaseBlock.BaseBlock>>(this.RetreatingEdges);

            return BackEdges.SetEquals(RetreatingEdges);


        } 




    }



}
