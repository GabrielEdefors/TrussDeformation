using System;
using System.Collections.Generic;
using LinearAlgebra = MathNet.Numerics.LinearAlgebra;
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
		public double ElemLength { get; set; } = 0.0;

		// Constructor
		public Bar(Node node1,
				   Node node2,
				   double area,
				   double stiffnessModulus)
		{
			Nodes.Add(node1);
			Nodes.Add(node2);
			StiffnessModulus = stiffnessModulus;
			Area = area;
		}

		public LinearAlgebra.Matrix<double> ComputeStiffnessMatrix()
		{
			LinearAlgebra.Matrix<double> K = LinearAlgebra.Matrix<double>.Build.Dense(2,2);

			double x1 = Nodes[0].Point.X;
			double y1 = Nodes[0].Point.Y;
			double z1 = Nodes[0].Point.Z;

			double x2 = Nodes[1].Point.X;
			double y2 = Nodes[1].Point.Y;
			double z2 = Nodes[1].Point.Z;

			ElemLength = Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1) + (z2 - z1) * (z2 - z1));

			K[0, 0] = Area * StiffnessModulus / ElemLength;
			K[0, 1] = - Area * StiffnessModulus / ElemLength;
			K[1, 0] = - Area * StiffnessModulus / ElemLength;
			K[1, 1] = Area * StiffnessModulus / ElemLength;

			return K;
		}
	}
}
