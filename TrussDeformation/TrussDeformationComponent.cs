using System;
using System.Collections.Generic;
using LinearAlgebra = MathNet.Numerics.LinearAlgebra;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace TrussDeformation
{
	public class TrussDeformationComponent : GH_Component
	{
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
			pManager.AddGenericParameter("eDof", "edof", "Topology Matrix", GH_ParamAccess.list);
			pManager.AddGenericParameter("Nodes", "nodes", "Truss Nodes", GH_ParamAccess.list);
		}

		protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
		{
		}

		protected override void SolveInstance(IGH_DataAccess DA)
		{
			List<Bar> barObjects = new List<Bar>();
			List<Node> nodeObjects = new List<Node>();
			List<List<int>> eDof = new List<List<int>>();
			DA.GetDataList("Bar", barObjects);
			DA.GetDataList("Nodes", nodeObjects);
			DA.GetDataList("eDof", eDof);

			int nDof = eDof.Max().Max();
			int nElem = eDof.Count;


			// Loop trough each bar and construct a load vector
			LinearAlgebra.Vector<double> forceVector = LinearAlgebra.Vector<double>.Build.Dense(nDof);


			for (int i = 0; i < nodeObjects.Count; i++)
			{

				// Load vector
				forceVector[i] = nodeObjects[i].ForceX;
				forceVector[i+1] = nodeObjects[i].ForceY;
				forceVector[i+2] = nodeObjects[i].ForceZ;

			}

			// Loop trough each element, compute local stiffness matrix and assemble into global stiffness matrix
			LinearAlgebra.Matrix<double> K = LinearAlgebra.Matrix<double>.Build.Dense(nDof, nDof);

			for (int i = 0; i < barObjects.Count; i++)
			{
				LinearAlgebra.Matrix<double> KElem = barObjects[i].ComputeStiffnessMatrix();

				// Assemble
				for (int rowIndex = 0; rowIndex < nElem; rowIndex++)
				{
					for (int colIndex = 0; colIndex < 6; colIndex++)
					{
						K[eDof[rowIndex][0], eDof[rowIndex][colIndex]] = KElem[rowIndex, eDof[rowIndex][colIndex]];  
					}
					
				}
				
			}

			int t = 1;

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

		public override Guid ComponentGuid
		{
			get { return new Guid("5791e210-aadb-40c6-b83c-da866162ee63"); }
		}
	}
}
