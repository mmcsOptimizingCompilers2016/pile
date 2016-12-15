using System.Collections.Generic;
using QuickGraph;

namespace OptimizingCompilers2016.Library.Region
{
	using BidirectionalGraph = QuickGraph.BidirectionalGraph<Region, Edge<Region>>;
	using ControlFlowGraph = ControlFlowGraph.ControlFlowGraph;
	/// <summary>
	/// Type of the region:
	///		BaseBlock - region consisting of one base block (i.e. header)
	///		CycleBody - region, containing cycle body ( without cycle )
	///		Cycle	  - region, containing cycle body and edges to the header
	///		General   - all the other regions
	/// </summary>
	public enum RegionType {
		BaseBlock, 
		CycleBody,
		Cycle,
		General
	};

	/// <summary>
	/// Representation of a region.
	/// The following conditions should be satisfied:
	///		1) header should dominate all the other nodes of a region
	///		2) node is in the region if there is another node which dominates it
	///		3) region should contain all the arcs between its nodes 
	///			(may be except for the reverse arcs to the header)
	/// </summary>
	public class Region
	{
		public Region ParentRegion { get; set; } /// direct parent (next level in hierarchy)								 
		public BidirectionalGraph HierarchyLevel { get; set; } /// relations between edges - only parent region contains them 
		public IEnumerable<Edge<Region>> ChildEdges
		{
			get { return HierarchyLevel.Edges; }
		}

		public Region() { HierarchyLevel = new BidirectionalGraph(); }
	}

	public class BaseBlockRegion : Region
	{
		public BaseBlock Block { get; }

		public BaseBlockRegion(ref BaseBlock block): base()
		{
			Block = block;
		}
	}

	/// <summary>
	/// Region, corresponding to cycle body (contains no cycles)
	/// </summary>
	public class CycleBodyRegion : Region
	{
		// region that dominates all the other regions in the cycle
		public Region CycleBodyStart { get; }
		public CycleBodyRegion(ref Region cycleBodyStart) : base()
		{
			CycleBodyStart = cycleBodyStart;
		}
	}

	/// <summary>
	/// Region, corresponding to cycle ( cycle body + header )
	/// </summary>
	public class CycleRegion : Region
	{
		/// cycle entry 
		public Region CycleStart { get; }
		/// Regions, which are the start of reverse edges
		public List<Region> ReverseEdgeSources { get; }
		public List<Edge<Region>> ReverseEdges { get; }
		public CycleRegion(ref Region cycleStart) : base()
		{
			CycleStart = cycleStart;
			ReverseEdgeSources = new List<Region>();
			ReverseEdges = new List<Edge<Region>>();
		}

		public void AddReverseEdge(ref Region reverseEdgeSource)
		{
			ReverseEdgeSources.Add(reverseEdgeSource);
			ReverseEdges.Add( new Edge<Region>(reverseEdgeSource, CycleStart) );
		}
	}

	public class RegionHierarchy
	{	
		/// ascending sequence of regions
		public List<Region> Hierarchy { get; }
		public RegionHierarchy(ControlFlowGraph cfg)
		{
			Hierarchy = new List<Region>();
			BidirectionalGraph graph = new BidirectionalGraph();
			BaseBlock root = cfg.GetRoot();
			/*TODO: 
			 * foreach ( var baseBlock in cfg)
			 *     create new BlockRegion
			 *     add it to the hierarchy
			 *     add edges to graph
			 *     
			 * BuildRegions
			 *    
			 */
		}

		/// TODO: take graph as param
		private void BuildRegions()
		{
			/*
			 * while has cycles
			 *    take cycle
			 *    create cycleBodyRegion
			 *    add it to hierarchy
			 *    update Parent of regions in cycle
			 *    update graph
			 *    
			 *    create cycleRegion
			 *    add it to the hierarchy
			 *    update graph
			 *
			 */
		}
	}
}
