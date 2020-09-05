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
			pManager.AddGenericParameter("Line", "line", "Line of the bar", GH_ParamAccess.list);
			pManager.AddGenericParameter("Restraint Nodes", "restraint nodes", "Restraint nodes objects", GH_ParamAccess.list);
			pManager.AddGenericParameter("Load Nodes", "load nodes", "Load nodes objects", GH_ParamAccess.list);
			pManager.AddNumberParameter("Area", "A", "Cross sectional area of the bar", GH_ParamAccess.list);
			pManager.AddNumberParameter("Youngs Modulus", "E", "Stiffness of the bar", GH_ParamAccess.list);
		}

		protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
		{
		}

		protected override void SolveInstance(IGH_DataAccess DA)
		{

			// Retrive data from component
			List<double> A = new List<double>();
			List<double> E = new List<double>();
			List<Line> lines = new List<Line>();
			List<ContstraintNode> rNodes = new List<ContstraintNode>();
			List<LoadNode> loadNodes = new List<LoadNode>();

			DA.GetDataList("Line", lines);
			DA.GetDataList("Area", A);
			DA.GetDataList("Youngs Modulus", E);
			DA.GetDataList("Restraint Nodes", rNodes);
			DA.GetDataList("Load Nodes", loadNodes);

			// The length of A and E must be the same as lines
			if ((A.Count != E.Count) | (E.Count != lines.Count))
			{
				throw new ArgumentException("Length of A and E must equal length of Line");
			}

			// Create one list to store the nodes and one list to store the bars
			List<Node> trussNodes = new List<Node>();
			List<Bar> trussBars = new List<Bar>();

			// To keep track if the node is unique
			bool unique1 = true;
			bool unique2 = true;

			// Topology matrix to keep track of element dofs
			List<List<int>> eDof = new List<List<int>>();

			// Loop trough each line and create nodes at end points 
			for (int i = 0; i < lines.Count; i++)
			{

				Node node1 = new Node(lines[i].From);
				Node node2 = new Node(lines[i].To);


				// Check if node is unique, if so give it an ID and degress of freedom
				foreach (Node existingNode in trussNodes)
				{

					// If not unique use an already identified node
					if (node1 == existingNode)
					{
						node1 = existingNode;
						unique1 = false;
					}

					if (node2 == existingNode)
					{
						node2 = existingNode;
						unique2 = false;
					}
				}

				// If unique give it an ID
				if (unique1)
				{
					int id_node_1 = trussNodes.Count;
					node1.ID = id_node_1;
					node1.Dofs = System.Linq.Enumerable.Range(id_node_1, 3).ToList();

					// Check if any boundary node or load node exist at current node
					foreach (ContstraintNode rNode in rNodes)
					{
						if (rNode == node1)
						{
							// Add restraint data
							node1.ConstraintX = rNode.ConstraintX;
							node1.ConstraintY = rNode.ConstraintY;
							node1.ConstraintZ = rNode.ConstraintZ;
						}
					}

					foreach (LoadNode loadNode in loadNodes)
					{
						if (loadNode == node1)
						{
							// Add force data
							node1.ForceX = loadNode.ForceX;
							node1.ForceY = loadNode.ForceY;
							node1.ForceZ = loadNode.ForceZ;
						}
					}


					// Finally add the node
					trussNodes.Add(node1);

				}

				if (unique2)
				{
					int id_node_2 = trussNodes.Count;
					node2.ID = id_node_2;
					node2.Dofs = System.Linq.Enumerable.Range(id_node_2 * 3, 3).ToList();

					// Check if any boundary node or load node exist at current node
					foreach (ContstraintNode rNode in rNodes)
					{
						if (rNode == node2)
						{
							// Add restraint data
							node2.ConstraintX = rNode.ConstraintX;
							node2.ConstraintY = rNode.ConstraintY;
							node2.ConstraintZ = rNode.ConstraintZ;
						}
					}


					foreach (LoadNode loadNode in loadNodes)
					{
						if (loadNode == node2)
						{
							// Add force data
							node2.ForceX = loadNode.ForceX;
							node2.ForceY = loadNode.ForceY;
							node2.ForceZ = loadNode.ForceZ;
						}
					}

					// Finally add the node
					trussNodes.Add(node2);
				}


				// Create a bar object between the nodes
				Bar bar = new Bar(node1, node2, A[i], E[i]);
				trussBars.Add(bar);

				// Topology matrix
				List<int> dofs1 = bar.Nodes[0].Dofs;
				List<int> dofs2 = bar.Nodes[1].Dofs;

				List<int> eDofRow = new List<int>();
				eDofRow.AddRange(dofs1);
				eDofRow.AddRange(dofs2);
				eDof.Add(eDofRow);


			}

			int nDof = trussNodes.Count * 3;
			int nElem = eDof.Count;


			// Loop trough each node and construct a load vector and boundary vector
			LinearAlgebra.Vector<double> forceVector = LinearAlgebra.Vector<double>.Build.Dense(nDof);
			List<int> boundaryDofs = new List<int>();
			List<double?> boundaryConstraints = new List<double?>();

			for (int i = 0; i < trussNodes.Count; i++)
			{

				// Load vector
				forceVector[i*3] = trussNodes[i].ForceX;
				forceVector[i*3+1] = trussNodes[i].ForceY;
				forceVector[i*3+2] = trussNodes[i].ForceZ;

				// Boundary vector
				if(trussNodes[i].ConstraintX != null)
				{
					boundaryDofs.AddRange(trussNodes[i].Dofs);
					boundaryConstraints.Add(trussNodes[i].ConstraintX);
					boundaryConstraints.Add(trussNodes[i].ConstraintY);
					boundaryConstraints.Add(trussNodes[i].ConstraintZ);
				}

			}

			// Loop trough each element, compute local stiffness matrix and assemble into global stiffness matrix
			LinearAlgebra.Matrix<double> K = LinearAlgebra.Matrix<double>.Build.Dense(nDof, nDof);

			for (int i = 0; i < trussBars.Count; i++)
			{
				LinearAlgebra.Matrix<double> KElem = trussBars[i].ComputeStiffnessMatrix();

				// Assemble
				for (int rowIndex = 0; rowIndex < nElem; rowIndex++)
				{
					for (int colIndex = 0; colIndex < 6; colIndex++)
					{
						K[eDof[rowIndex][0], eDof[rowIndex][colIndex]] = KElem[rowIndex, colIndex];  
					}	
				}			
			}

			// Calculate the displacements
			Solver solver = new Solver();
			LinearAlgebra.Vector<double> displacements = solver.solveEquations(K, forceVector, boundaryDofs, boundaryConstraints.Cast<double>().ToList());
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
