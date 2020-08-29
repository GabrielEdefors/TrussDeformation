using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace TrussDeformation
{
	public class NodeComponent : GH_Component
	{
		/// <summary>
		/// Initializes a new instance of the NodeComponent class.
		/// </summary>
		public NodeComponent()
		  : base("NodeComponent", "Nickname",
			  "Description",
			  "Category", "Subcategory")
		{
		}

		/// <summary>
		/// Registers all the input parameters for this component.
		/// </summary>
		protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
		{
			pManager.AddPointParameter("Point", "point", "The spatial representation of the node", GH_ParamAccess.list);
			pManager.AddBooleanParameter("Restrained in x", "restrained in x", "True if restrained in x direction", GH_ParamAccess.list);
			pManager.AddBooleanParameter("Restrained in y", "restrained in y", "True if restrained in y direction", GH_ParamAccess.list);
			pManager.AddBooleanParameter("Restrained in z", "restrained in z", "True if restrained in z direction", GH_ParamAccess.list);
		}

		/// <summary>
		/// Registers all the output parameters for this component.
		/// </summary>
		protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
		{
			pManager.AddGenericParameter("Node", "node", "Node", GH_ParamAccess.list);
		}

		/// <summary>
		/// This is the method that actually does the work.
		/// </summary>
		/// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
		protected override void SolveInstance(IGH_DataAccess DA)
		{
			List<Point3d> points = new List<Point3d>();
			List<bool> restrainedx = new List<bool>();
			List<bool> restrainedy = new List<bool>();
			List<bool> restrainedz = new List<bool>();

			DA.GetDataList("Point", points);
			DA.GetDataList("Restrained in x", restrainedx);
			DA.GetDataList("Restrained in y", restrainedy);
			DA.GetDataList("Restrained in z", restrainedz);

			// Create a node object and store it in a list
			List<Node> nodes = new List<Node>();

			for (int i = 0; i < points.Count; i++)
			{
				nodes.Add(new Node(points[i], restrainedx[i], restrainedy[i], restrainedz[i]));
			}

			

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