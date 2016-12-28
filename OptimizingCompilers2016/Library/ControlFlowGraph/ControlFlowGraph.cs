﻿using QuickGraph;
using QuickGraph.Graphviz;
using System.Collections.Generic;
using System.Linq;

namespace OptimizingCompilers2016.Library
{
    public class ControlFlowGraph
    {
        public BidirectionalGraph<BaseBlock, Edge<BaseBlock>> CFG =
            new BidirectionalGraph<BaseBlock, Edge<BaseBlock>>();

        public Dictionary<Edge<BaseBlock>, EdgeType> EdgeTypes { get; }

        public HashSet<Edge<BaseBlock>> BackwardEdges { get; }
         
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
            BackwardEdges = new HashSet<Edge<BaseBlock>>();
        }

        public BaseBlock GetRoot()
        {
            return (NumberOfVertices() > 0) ? CFG.Vertices.ElementAt(0) : null;
        }

        public int NumberOfVertices()
        {
            return CFG.Vertices.Count();
        }

        public IEnumerable<BaseBlock> GetVertices()
        {

            return CFG.Vertices;
        }

        /// <summary>
        /// Сериализация графа в строку формата dot-файла для последующей визуализации с
        /// помощью graphviz
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var graphviz = new GraphvizAlgorithm<BaseBlock, Edge<BaseBlock>>(CFG);
            return graphviz.Generate();
        }

        public List<BaseBlock> ToList()
        {
            return CFG.Vertices.ToList();
        }

        public IEnumerator<BaseBlock> GetEnumerator()
        {
            return CFG.Vertices.ToList().GetEnumerator();
        }

        public static Edge<BaseBlock> MakeEdge(BaseBlock source, BaseBlock target) {
            
            return new Edge<BaseBlock>(source, target);
        }

        public bool CheckReducibility() {
            DepthSpanningTree DFST = new DepthSpanningTree(this);

            List<Edge<BaseBlock>> BackEdges = new List<Edge<BaseBlock>>();

            List<Edge<BaseBlock>> RetreatingEdges = new List<Edge<BaseBlock>>();

            var _BackEdges = new HashSet<Edge<BaseBlock>>(BackEdges);
            var _RetreatingEdges = new HashSet<Edge<BaseBlock>>(RetreatingEdges);

            return _BackEdges.SetEquals(_RetreatingEdges);
        }

        public void ClassificateEdges()
        {
            // TODO: Check when DepthSpanningTree will be fixed
            var depthTree = new DepthSpanningTree(this);
            foreach (var edge in CFG.Edges)
            {
                if (depthTree.SpanningTree.ContainsEdge(edge))
                {
                    EdgeTypes.Add(edge, EdgeType.Coming);
                }
                else if (depthTree.FindBackwardPath(edge.Source, edge.Target))
                {
                    EdgeTypes.Add(edge, EdgeType.Retreating);
                }
                else
                {
                    EdgeTypes.Add(edge, EdgeType.Cross);
                }
            }
        }

        // before calling, need to fill EdgeTypes collection
        public void FindBackwardEdges(Dictionary<BaseBlock, List<BaseBlock>> dominatorsTree)
        {
            foreach (var edgeType in EdgeTypes)
            {
                if (edgeType.Value != EdgeType.Retreating) continue;
                var edge = edgeType.Key;
                var pretendentOne = dominatorsTree.
                    FirstOrDefault(dominator => dominator.Key.Equals(edge.Source)).Value.
                    FirstOrDefault(source => source.Equals(edge.Target));
                if (pretendentOne != null)
                    BackwardEdges.Add(edge);
            }
        }
    }

    public class NaturalLoop{
        HashSet<BaseBlock> NatLoop = new HashSet<BaseBlock>();

        public HashSet<BaseBlock> Loop { get { return NatLoop; } }

        ReversedBidirectionalGraph<BaseBlock, Edge<BaseBlock>> reverseCFG;

        public NaturalLoop(ControlFlowGraph CFG, Edge<BaseBlock> BackEdge) {
            
            reverseCFG = new ReversedBidirectionalGraph<BaseBlock, Edge<BaseBlock>>(CFG.CFG);

            NatLoop.Add(BackEdge.Target);

            NatLoop.Add(BackEdge.Source);

            BuildNaturalLoop(BackEdge.Source);
            
        }

        void BuildNaturalLoop(BaseBlock Source) {

            foreach (var edge in reverseCFG.OutEdges(Source)) { 

                if (!NatLoop.Contains(edge.Target)) 
                {

                    NatLoop.Add(edge.Target);

                    BuildNaturalLoop(edge.Target);
                }

            }

        }

        public override string ToString()
        {
            string LoopStr = "";
            foreach (var block in NatLoop)
            {

                LoopStr += block.ToString() + "\n";
            }
            return LoopStr;
        }
    }
}
