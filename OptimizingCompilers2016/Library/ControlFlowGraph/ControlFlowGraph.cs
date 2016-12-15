﻿using QuickGraph;
using QuickGraph.Graphviz;
using System.Collections.Generic;
using System.Linq;

namespace OptimizingCompilers2016.Library.ControlFlowGraph
{
    using DepthSpanningTree = DepthSpanningTree.DepthSpanningTree;
    using ReverseControlFlowGraph = ReversedBidirectionalGraph<BaseBlock.BaseBlock, Edge<BaseBlock.BaseBlock>>;

    public class ControlFlowGraph
    {

        public BidirectionalGraph<BaseBlock.BaseBlock, Edge<BaseBlock.BaseBlock>> CFG =
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

        public static Edge<BaseBlock.BaseBlock> MakeEdge(BaseBlock.BaseBlock source, BaseBlock.BaseBlock target) {
            
            return new Edge<BaseBlock.BaseBlock>(source, target);
        }



        public bool CheckReducibility() {
            DepthSpanningTree DFST = new DepthSpanningTree(this);

            List<Edge<BaseBlock.BaseBlock>> BackEdges = new List<Edge<BaseBlock.BaseBlock>>();

            List<Edge<BaseBlock.BaseBlock>> RetreatingEdges = new List<Edge<BaseBlock.BaseBlock>>();

            var _BackEdges = new HashSet<Edge<BaseBlock.BaseBlock>>(BackEdges);
            var _RetreatingEdges = new HashSet<Edge<BaseBlock.BaseBlock>>(RetreatingEdges);

            return _BackEdges.SetEquals(_RetreatingEdges);

            
        }
    }

    

    public class NaturalLoop{
        HashSet<BaseBlock.BaseBlock> loop = new HashSet<BaseBlock.BaseBlock>();

        public HashSet<BaseBlock.BaseBlock> Loop { get { return loop; } }

        ReversedBidirectionalGraph<BaseBlock.BaseBlock, Edge<BaseBlock.BaseBlock>> reverseCFG;

        public NaturalLoop(ControlFlowGraph CFG, Edge<BaseBlock.BaseBlock> BackEdge) {
            
            reverseCFG = new ReversedBidirectionalGraph<BaseBlock.BaseBlock, Edge<BaseBlock.BaseBlock>>(CFG.CFG);

            loop.Add(BackEdge.Target);

            loop.Add(BackEdge.Source);

            BuildNaturalLoop(BackEdge.Source);
            
        }


        void BuildNaturalLoop(BaseBlock.BaseBlock Source) {

            foreach (var edge in reverseCFG.OutEdges(Source)) { 

                if (!loop.Contains(edge.Target)) 
                {

                    loop.Add(edge.Target);

                    BuildNaturalLoop(edge.Target);
                }

            }

        }



    }



}
