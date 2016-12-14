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
		// TODO: decide whether to use list or graph
		public List<Region> SourceRegions { get; }
		public List<Region> TargetRegions { get; }

		public Region()
		{
			SourceRegions = new List<Region>();
			TargetRegions = new List<Region>();
		}

		public void AddTargetRegion(Region target)
		{
			TargetRegions.Add(target);
		}

		public void AddSourceRegion(Region source)
		{
			SourceRegions.Add(source);
		}

	}

	public class BaseBlockRegion : Region
	{
		public BaseBlock Block { get; }

		public BaseBlockRegion(BaseBlock block): base()
		{
			Block = block;
		}
	}

	/// <summary>
	/// Region, corresponding to cycle body (contains no cycles)
	/// </summary>
	public class CycleBodyRegion : Region
	{
		public Region CycleBodyStart { get; }
		public BidirectionalGraph OutEdges { get; }
		public CycleBodyRegion(Region cycleBodyStart) : base()
		{
			CycleBodyStart = cycleBodyStart;
			OutEdges = new BidirectionalGraph();
		}

		public void AddOutEdge(Region source, Region target)
		{
			var edge = new QuickGraph.Edge<Region>(source, target);
			OutEdges.AddEdge(edge);
		}
	}

	/// <summary>
	/// Region, corresponding to cycle ( cycle body + header )
	/// </summary>
	public class CycleRegion : Region
	{
		public Region CycleStart { get; }
		/// <summary>
		/// Regions, which are the start of reverse edges
		/// </summary>
		public List<Region> ReverseEdgeSource { get; }
		public CycleRegion(Region cycleStart) : base()
		{
			CycleStart = cycleStart;
			ReverseEdgeSource = new List<Region>();
		}

		public void AddReverseEdgeFrom(Region source)
		{
			ReverseEdgeSource.Add(source);
		}
	}

	public class RegionHierarchy
	{
		public BidirectionalGraph Hierarchy { get; }
		public RegionHierarchy(ControlFlowGraph cfg)
		{
			Hierarchy = new BidirectionalGraph();
			// TODO: implement me
		}
	}
}
