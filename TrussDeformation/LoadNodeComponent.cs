using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace TrussDeformation
{
	public class LoadNodeComponent : GH_Component
	{
		/// <summary>
		/// Initializes a new instance of the LoadNodeComponent class.
		/// </summary>
		public LoadNodeComponent()
		  : base("LoadNode", "LoadNode",
			  "Creates a node object that adds forces to a node",
			  "Truss", "Elements")
		{
		}

		/// <summary>
		/// Registers all the input parameters for this component.
		/// </summary>
		protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
		{
			pManager.AddPointParameter("Point", "point", "The spatial representation of the node", GH_ParamAccess.list);
			pManager.AddNumberParameter("Force in x", "Force in x", "Force in x direction", GH_ParamAccess.list);
			pManager.AddNumberParameter("Force in y", "Force in y", "Force in y direction", GH_ParamAccess.list);
			pManager.AddNumberParameter("Force in z", "Force in z", "Force in z direction", GH_ParamAccess.list);
		}

		/// <summary>
		/// Registers all the output parameters for this component.
		/// </summary>
		protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
		{
			pManager.AddGenericParameter("loadNode", "loadNode", "Node", GH_ParamAccess.list);
		}
		
		/// <summary>
		/// This is the method that actually does the work.
		/// </summary>
		/// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
		protected override void SolveInstance(IGH_DataAccess DA)
		{
			List<Point3d> points = new List<Point3d>();
			List<double> forceX = new List<double>();
			List<double> forceY = new List<double>();
			List<double> forceZ = new List<double>();

			DA.GetDataList("Point", points);
			DA.GetDataList("Force in x", forceX);
			DA.GetDataList("Force in y", forceY);
			DA.GetDataList("Force in z", forceZ);

			// Create a restraint node object and store it in a list
			List<LoadNode> nodes = new List<LoadNode>();

			for (int i = 0; i < points.Count; i++)
			{
				nodes.Add(new LoadNode(points[i], forceX[i], forceY[i], forceZ[i]));
			}

			DA.SetDataList("loadNode", nodes);

		}

		/// <summary>
		/// Provides an Icon for the component.
		/// </summary>
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
			get { return new Guid("99417394-1daa-46ec-ab00-288d2fa017f3"); }
		}
	}
}