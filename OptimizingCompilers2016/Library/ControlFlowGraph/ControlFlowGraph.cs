﻿using QuickGraph;
using QuickGraph.Graphviz;
using System.Collections.Generic;
using System.Linq;

namespace OptimizingCompilers2016.Library.ControlFlowGraph
{
    public class ControlFlowGraph
    {
        UndirectedGraph<BaseBlock, Edge<BaseBlock>> CFG = 
            new UndirectedGraph<BaseBlock, Edge<BaseBlock>>();

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

    }
}
