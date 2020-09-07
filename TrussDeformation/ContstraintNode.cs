﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;

namespace TrussDeformation
{
	class ContstraintNode
	{
		// Properties	
		public Point3d Point { get; set; } = new Point3d();
		public double? ConstraintX { get; set; } = null;
		public double? ConstraintY { get; set; } = null;
		public double? ConstraintZ { get; set; } = null;

		// Constructor
		public ContstraintNode(Point3d point,
					double? constraintx,
					double? constrainty,
					double? constraintz)
		{
			Point = point;
			ConstraintX = constraintx;
			ConstraintY = constrainty;
			ConstraintZ = constraintz;
		}

		public static bool operator ==(ContstraintNode node1, Node node2)
		{
			// If they have the same point they count as the same node
			if (node1.Point.Equals(node2.Point))
			{
				return true;
			}
			return false;
		}

		public static bool operator !=(ContstraintNode node1, Node node2)
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
