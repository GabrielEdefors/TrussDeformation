using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;

namespace TrussDeformation
{
	class Node
	{
		// Properties	
		public Point3d Point { get; set; } = new Point3d();
		public bool RestrainedX { get; set; } = false;
		public bool RestrainedY { get; set; } = false;
		public bool RestrainedZ { get; set; } = false;
		public double ForceX { get; set; } = 0.0;
		public double ForceY { get; set; } = 0.0;
		public double ForceZ { get; set; } = 0.0;
		public int? ID { get; set; } = null;
		public List<int> Dofs { get; set; } = new List<int>();

		// Constructor
		public Node(Point3d point,
					bool restrainedx,
					bool restrainedy,
					bool restrainedz,
					double forcex,
					double forcey,
					double forcez)
		{
			Point = point;
			RestrainedX = restrainedx;
			RestrainedY = restrainedy;
			RestrainedZ = restrainedz;
			ForceX = forcex;
			ForceY = forcey;
			ForceZ = forcez;
		}

		public static bool operator ==(Node node1, Node node2)
		{
			// If they have the same point they count as the same node
			if(node1.Equals(node2))
			{
				return true;
			}
			return false;
		}

		public static bool operator !=(Node node1, Node node2)
		{
			// If they have the same point they count as the same node
			if (node1.Equals(node2) == false)
			{
				return true;
			}
			return false;
		}


	}
}
