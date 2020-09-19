using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace TrussDeformation
{
	public class ConstraintNodeComponent : GH_Component
	{
		/// <summary>
		/// Initializes a new instance of the NodeComponent class.
		/// </summary>
		public ConstraintNodeComponent()
		  : base("ConstraintNode", "ConstraintNode",
			  "Creates a node object that adds constraint to a node",
			  "Truss", "Elements")
		{
		}

		/// <summary>
		/// Registers all the input parameters for this component.
		/// </summary>
		protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
		{
			pManager.AddPointParameter("Point", "point", "The spatial representation of the node", GH_ParamAccess.list);
			pManager.AddNumberParameter("Constraint in x", "constraint in x", "Value of constraint in x, set NaN if no constraint", GH_ParamAccess.list);
			pManager.AddNumberParameter("Constraint in y", "constraint in y", "Value of constraint in y, set NaN if no constraint", GH_ParamAccess.list);
			pManager.AddNumberParameter("Constraint in z", "constraint in z", "Value of constraint in z, set NaN if no constraint", GH_ParamAccess.list);

		}

		/// <summary>
		/// Registers all the output parameters for this component.
		/// </summary>
		protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
		{
			pManager.AddGenericParameter("constraintNode", "constraintNode", "Node", GH_ParamAccess.list);
		}

		/// <summary>
		/// This is the method that actually does the work.
		/// </summary>
		/// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
		protected override void SolveInstance(IGH_DataAccess DA)
		{
			List<Point3d> points = new List<Point3d>();
			List<double> constraintx = new List<double>();
			List<double> constrainty = new List<double>();
			List<double> constraintz = new List<double>();

			DA.GetDataList("Point", points);
			DA.GetDataList("Constraint in x", constraintx);
			DA.GetDataList("Constraint in y", constrainty);
			DA.GetDataList("Constraint in z", constraintz);

			// Create a restraint node object and store it in a list
			List<ContstraintNode> nodes = new List<ContstraintNode>();

			for (int i = 0; i < points.Count; i++)
			{

				double? constraintxI = null;
				double? constraintyI = null;
				double? constraintzI = null;

				if (!double.IsNaN(constraintx[i]))
				{
					constraintxI = constraintx[i];
				}
				if (!double.IsNaN(constrainty[i]))
				{
					constraintyI = constrainty[i];
				}
				if (!double.IsNaN(constraintz[i]))
				{
					constraintzI = constraintz[i];
				}


				nodes.Add(new ContstraintNode(points[i], constraintxI, constraintyI, constraintzI));

			}

			DA.SetDataList("constraintNode", nodes);

		}


		protected override System.Drawing.Bitmap Icon
		{
			get
			{
				//You can add image files to your project resources and access them like this:
				// return Resources.IconForThisComponent;
				return null;
			}
		}

		/// <summary>
		/// Gets the unique ID for this component. Do not change this ID after release.
		/// </summary>
		public override Guid ComponentGuid
		{
			get { return new Guid("0b8acc15-46c2-403a-829c-ea8ac6d0d087"); }
		}
	}
}