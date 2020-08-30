using System;
using System.Collections.Generic;
using LinearAlgebra = MathNet.Numerics.LinearAlgebra;

using Grasshopper.Kernel;
using Rhino.Geometry;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace TrussDeformation
{
	public class TrussDeformationComponent : GH_Component
	{
		/// <summary>
		/// Each implementation of GH_Component must provide a public 
		/// constructor without any arguments.
		/// Category represents the Tab in which the component will appear, 
		/// Subcategory the panel. If you use non-existing tab or panel names, 
		/// new tabs/panels will automatically be created.
		/// </summary>
		public TrussDeformationComponent()
		  : base("TrussDeformation", "TrussDeformation",
			  "Solves the Equalibrium of the Truss Structure",
			  "Truss", "Solver")
		{
		}

		/// <summary>
		/// Registers all the input parameters for this component.
		/// </summary>
		protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
		{
			pManager.AddGenericParameter("Bar", "bar", "Bar Object", GH_ParamAccess.list);
		}

		protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
		{
		}

		protected override void SolveInstance(IGH_DataAccess DA)
		{
			List<Bar> barObjects = new List<Bar>();
			DA.GetDataList("Bar", barObjects);

			// Loop trough each bar and construct a topology matrix eDof
			List<List<int>> eDof = new List<List<int>>();

			for (int i = 0; i < barObjects.Count; i++)
			{
				List<int> dofs1 = barObjects[i].Nodes[0].Dofs;
				List<int> dofs2 = barObjects[i].Nodes[1].Dofs;


				List<int> eDofRow = new List<int>();
				eDofRow.AddRange(dofs1);
				eDofRow.AddRange(dofs2);
				eDof.Add(eDofRow);

					
			}

			int a = 1;
			
		}

		/// <summary>
		/// Provides an Icon for every component that will be visible in the User Interface.
		/// Icons need to be 24x24 pixels.
		/// </summary>
		protected override System.Drawing.Bitmap Icon
		{
			get
			{
				// You can add image files to your project resources and access them like this:
				//return Resources.IconForThisComponent;
				return null;
			}
		}

		/// <summary>
		/// Each component must have a unique Guid to identify it. 
		/// It is vital this Guid doesn't change otherwise old ghx files 
		/// that use the old ID will partially fail during loading.
		/// </summary>
		public override Guid ComponentGuid
		{
			get { return new Guid("5791e210-aadb-40c6-b83c-da866162ee63"); }
		}
	}
}
