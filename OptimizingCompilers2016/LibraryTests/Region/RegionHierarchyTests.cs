using Microsoft.VisualStudio.TestTools.UnitTesting;
using OptimizingCompilers2016.Library.Region;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickGraph.Graphviz;
using QuickGraph;

namespace OptimizingCompilers2016.Library.Region.Tests
{
	[TestClass()]
	public class RegionHierarchyTests
	{	
		private ControlFlowGraph cfg = SetupCFG();

		[TestMethod()]
		public void RegionHierarchyTest()
		{
			var i = 1;
		}

		private static ControlFlowGraph SetupCFG()
		{
			List<BaseBlock> blocks = new List<BaseBlock>();

			var block1 = new BaseBlock();
			block1.Name = "block 1";

			var block2 = new BaseBlock();
			block2.Name = "block 2";

			var block3 = new BaseBlock();
			block3.Name = "block 3";

			var block4 = new BaseBlock();
			block4.Name = "block 4";

			var block5 = new BaseBlock();
			block5.Name = "block 5";

			var block6 = new BaseBlock();
			block5.Name = "block 6";

			block6.Predecessors.Add(block5);
			block5.Predecessors.Add(block3);
			block5.JumpOutput = block2;
			block5.Output = block6;
			block4.Predecessors.Add(block2);
			block3.Predecessors.Add(block2);
			block3.Predecessors.Add(block3);
			block3.JumpOutput = block3;
			block3.Output = block5;
			block2.Predecessors.Add(block5);
			block2.Predecessors.Add(block1);
			block2.JumpOutput = block4;
			block2.Output = block3;
			block1.Output = block2;
			
			blocks.Add(block1);
			blocks.Add(block2);
			blocks.Add(block3);
			blocks.Add(block4);
			blocks.Add(block5);
			blocks.Add(block6);

			return new ControlFlowGraph(blocks);
		}

		[TestMethod()]
		public void InitializeGraphTest()
		{
			var graph = RegionHierarchy.InitializeGraph(ref cfg);

			var edges = cfg.GetEdges().ToList();
			var vertices = cfg.ToList();

			Assert.IsTrue(edges.ToList().Count == graph.Edges.Count());
			Assert.IsTrue(vertices.Count == graph.Vertices.Count());
			for ( var i = 0; i < vertices.Count; i++ )
			{
				Assert.IsTrue(vertices.ElementAt(i) == ((BaseBlockRegion)graph.Vertices.ElementAt(i)).Block);
			}

			for (var i = 0; i < edges.Count; i++ )
			{
				Assert.IsTrue(edges.ElementAt(i).Source == ((BaseBlockRegion)graph.Edges.ElementAt(i).Source).Block );
				Assert.IsTrue(edges.ElementAt(i).Target == ((BaseBlockRegion)graph.Edges.ElementAt(i).Target).Block);
			} 
		}
	}
}