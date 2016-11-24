using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickGraph.Collections;

namespace OptimizingCompilers2016.Library.Region
{
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
	class Region
	{	
		/// <summary>
		/// Type of the region
		/// </summary>
		private RegionType Type { get; }

		/// <summary>
		/// Node which dominates all the other nodes in this region
		/// </summary>
		private BaseBlock HeaderNode { get; }

		//private QuickGraph.BidirectionalGraph<Region, QuickGraph.Edge<Region> > Graph { get; }

		public Region(BaseBlock header, RegionType type)
		{	
			// TODO: check whether all the three conditions are satisfied
			this.HeaderNode = header;
			this.Type = type;
		}
	}
}
