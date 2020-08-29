using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel.Geometry;
using Rhino.Geometry;

namespace TrussDeformation
{
	class Bar
	{
		// Properties	
		public List<Node> Nodes = new List<Node>();
		public double Area { get; set; } = 0.0;
		public double StiffnessModulus { get; set; } = 0.0;

		// Constructor
		public Bar(Node node1,
				   Node node2,
				   double area,
				   double stiffnessModulus)
		{
			Nodes[0] = node1;
			Nodes[1] = node2;
			StiffnessModulus = stiffnessModulus;
			Area = area;
		}
	}
}
