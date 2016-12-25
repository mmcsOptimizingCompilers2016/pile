using System.Collections.Generic;
using QuickGraph;
using System.Linq;
using System;

namespace OptimizingCompilers2016.Library.Region
{
	using BidirectionalGraph = QuickGraph.BidirectionalGraph<Region, Edge<Region>>;
	//using ControlFlowGraph = ControlFlowGraph.ControlFlowGraph;
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

	public class UnreducibleCFGException : Exception
	{
		public UnreducibleCFGException() {}
		public UnreducibleCFGException(string message) : base(message) {}
		public UnreducibleCFGException(string message, Exception inner): base(message, inner) {}
	}

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
		// region that dominates all the other regions in the cycle
		public Region CycleBodyStart { get; }
		public CycleBodyRegion(Region cycleBodyStart) : base()
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

		public void AddReverseEdge(Region reverseEdgeSource)
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
			if ( !cfg.CheckReducibility() )
			{
				throw new UnreducibleCFGException("Control flow graph is not reducible");
			}
				
			// initialize the first level in the hierarchy
			BidirectionalGraph graph = InitializeGraph(ref cfg);
			Hierarchy = graph.Vertices.ToList();

			//FIXME: get all reverse edges - they correspond to native cycles
			// .............. waiting for implementation by DreamTeam
			var reverseEdges = GetReverseEdges(graph);

			// FIX ME: iterate through reverse edges
			while (reverseEdges.Count > 0)
			{
				// FIXME: take reverse edge - IN WHICH ORDER??
				var reverseEdge = GetReverseEdgeForInnerCycle(ref reverseEdges);

				CreateCycleRegion(ref graph, reverseEdge);

				// TODO: get next reverseEdge
			}
		}

		public void CreateCycleRegion(ref BidirectionalGraph graph, Edge<Region> reverseEdge)
		{
			var cycleStart = reverseEdge.Target;
			var cycle = FindCycle(ref graph, reverseEdge);

			// FIXME: Build new cycle body region 
			var cycleBodyRegion = new CycleBodyRegion(cycleStart);
			var cycleBodyRegionGraph = new BidirectionalGraph();

			var visitedVertices = new HashSet<Region>();
			visitedVertices.Add(cycleStart);
			foreach (var edge in cycle)
			{
				if (!visitedVertices.Contains(edge.Source))
				{
					cycleBodyRegionGraph.AddVertex(edge.Source);
					visitedVertices.Add(edge.Source);
					edge.Source.ParentRegion = cycleBodyRegion;
				}

				if (!visitedVertices.Contains(edge.Target))
				{
					cycleBodyRegionGraph.AddVertex(edge.Target);
					visitedVertices.Add(edge.Target);
					edge.Source.ParentRegion = cycleBodyRegion;
				}
				
				cycleBodyRegionGraph.AddEdge(edge);
			}

			cycleBodyRegion.HierarchyLevel = cycleBodyRegionGraph;
			Hierarchy.Add(cycleBodyRegion);

			// TODO: update graph: 
			//			remove edges in cycle
			//			update edges

			// TODO: add CycleRegion
			// TODO: update graph: 
			//			remove edges in cycle
			//			update edges
		}

		//FIXME: get all reverse edges - they correspond to native cycles
		// .............. waiting for implementation by DreamTeam
		public static List<Edge<Region>> GetReverseEdges( BidirectionalGraph graph )
		{	
			return new List<Edge<Region>>();
		}

		// FIXME: make sure that the reverse edge is associated with an inner (i.e. non-outer) cycle
		// if it corresponds to an outer cycle, take next
		public static Edge<Region> GetReverseEdgeForInnerCycle(ref List<Edge<Region>> edges)
		{
			return edges.Last();
		}

		/// <summary>
		/// Find cycle within graph which corresponds to the given reverseEdge.
		/// This cycle should include no inner cycles.
		/// Returns all edges besides the provided reverse one.
		/// </summary>
		/// <param name="graph">input graph</param>
		/// <param name="reverseEdge">reverse edge corresponding to a cycle</param>
		/// <returns></returns>
		public static IEnumerable<Edge<Region>> FindCycle(ref BidirectionalGraph graph, Edge<Region> reverseEdge)
		{
			var allEdges = graph.Edges.ToList();
			var cycle =	new List<Edge<Region>>();
		
			var cycleHeader = reverseEdge.Target;
			var incidentEdges = allEdges.FindAll(x => x.Source == cycleHeader);
			var queue = new List<Edge<Region>>(incidentEdges);

			while (queue.Count > 0)
			{
				var nextEdge = queue.Last();
				queue.RemoveAt(queue.Count - 1);

				if ( cycle.Count > 0 && cycle.Last().Source == nextEdge.Source )
					cycle.RemoveAt(cycle.Count - 1);

				cycle.Add(nextEdge);

				if (nextEdge == reverseEdge)
				{
					cycle.RemoveAt(cycle.Count - 1);
					break; 
				}

				incidentEdges = allEdges.FindAll(x => x.Source == nextEdge.Target);

				if (incidentEdges.Count == 0) {
					cycle.RemoveAt(cycle.Count - 1);
					continue;
				}

				queue.AddRange(incidentEdges);
			}

			return cycle;
		}

		/// build graph representing the first level in regions hierarchy
		public static BidirectionalGraph InitializeGraph(ref ControlFlowGraph cfg)
		{
			BidirectionalGraph graph = new BidirectionalGraph();

			// collection of regions to keep order of primitive regions
			IEnumerable<Region> vertices = cfg.ToList().Select(block => new BaseBlockRegion(block));
			graph.AddVertexRange(vertices.ToList());

			vertices = graph.Vertices;
			var edges = cfg.GetEdges().Select(edge => new Edge<Region>(
			vertices.FirstOrDefault(x => ((BaseBlockRegion)x).Block == edge.Source),
			vertices.FirstOrDefault(x => ((BaseBlockRegion)x).Block == edge.Target))).ToList();

			graph.AddEdgeRange(edges.ToList());
			return graph;
		}
	}
}
