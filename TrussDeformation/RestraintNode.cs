using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;

namespace TrussDeformation
{
	class RestraintNode
	{
		// Properties	
		public Point3d Point { get; set; } = new Point3d();
		public bool RestrainedX { get; set; } = false;
		public bool RestrainedY { get; set; } = false;
		public bool RestrainedZ { get; set; } = false;

		// Constructor
		public RestraintNode(Point3d point,
					bool restrainedx,
					bool restrainedy,
					bool restrainedz)
		{
			Point = point;
			RestrainedX = restrainedx;
			RestrainedY = restrainedy;
			RestrainedZ = restrainedz;
		}

		public static bool operator ==(RestraintNode node1, Node node2)
		{
			// If they have the same point they count as the same node
			if (node1.Point.Equals(node2.Point))
			{
				return true;
			}
			return false;
		}

		public static bool operator !=(RestraintNode node1, Node node2)
		{
			// If they have the same point they count as the same node
			if (node1.Point.Equals(node2.Point) == false)
			{
				return true;
			}
			return false;
		}


	}
}
