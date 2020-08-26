using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace TrussDeformation
{
	public class BarComponent : GH_Component
	{
		/// <summary>
		/// Initializes a new instance of the MyComponent1 class.
		/// </summary>
		public BarComponent()
		  : base("Bar", "Bar",
			  "Bar Object",
			  "Truss", "Elements")
		{
		}

		/// <summary>
		/// Registers all the input parameters for this component.
		/// </summary>
		protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
		{
			pManager.AddPointParameter("Point 1", "p1", "First point of the bar", GH_ParamAccess.list);
			pManager.AddPointParameter("Point 2", "p2", "Second point of the bar", GH_ParamAccess.list);
			pManager.AddNumberParameter("Area", "A", "Cross sectional area of the bar", GH_ParamAccess.list);
			pManager.AddNumberParameter("Youngs Modulus", "E", "Stiffness of the bar", GH_ParamAccess.list);
		}

		/// <summary>
		/// Registers all the output parameters for this component.
		/// </summary>
		protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
		{
			pManager.AddLineParameter("Line", "Line", "Line representing the geometry of the bar", GH_ParamAccess.list);
		}

		/// <summary>
		/// This is the method that actually does the work.
		/// </summary>
		/// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
		protected override void SolveInstance(IGH_DataAccess DA)
		{

			// Retrive data from component
			List<Point3d> point1 = new List<Point3d>();
			List<Point3d> point2 = new List<Point3d>();
			List<double> A = new List<double>();
			List<double> E = new List<double>();

			DA.GetDataList("Point 1", point1);
			DA.GetDataList("Point 2", point2);
			DA.GetDataList("Area", A);
			DA.GetDataList("Youngs Modulus", E);

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
			get { return new Guid("289c727c-39fc-438e-81a3-358ae046e63b"); }
		}
	}
}