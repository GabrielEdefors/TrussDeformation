using System;
using System.Collections.Generic;
using System.Data;
using System.Deployment.Internal;
using System.IO;
using System.Linq;
using Grasshopper;
using Grasshopper.GUI;
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
			pManager.AddGenericParameter("Node 1", "node 1", "First node of the bar", GH_ParamAccess.list);
			pManager.AddGenericParameter("Node 2", "node 2", "Seconds node of the bar", GH_ParamAccess.list);
			pManager.AddNumberParameter("Area", "A", "Cross sectional area of the bar", GH_ParamAccess.list);
			pManager.AddNumberParameter("Youngs Modulus", "E", "Stiffness of the bar", GH_ParamAccess.list);
		}

		/// <summary>
		/// Registers all the output parameters for this component.
		/// </summary>
		protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
		{
			pManager.AddGenericParameter("Bars", "bars", "Bar object", GH_ParamAccess.list);
			pManager.AddGenericParameter("eDof", "edof", "Topology Matrix", GH_ParamAccess.list);
			pManager.AddGenericParameter("Node", "nodes", "Node List", GH_ParamAccess.list);
		}

		protected override void SolveInstance(IGH_DataAccess DA)
		{

			// Retrive data from component
			List<double> A = new List<double>();
			List<double> E = new List<double>();
			List<Node> nodes1 = new List<Node>();
			List<Node> nodes2 = new List<Node>();

			DA.GetDataList("Node 1", nodes1);
			DA.GetDataList("Node 2", nodes2);
			DA.GetDataList("Area", A);
			DA.GetDataList("Youngs Modulus", E);

			// The length of A and E must be the same as the Node 1 and Node 2
			if ((A.Count != E.Count) | (E.Count != nodes1.Count) | (E.Count != nodes2.Count))
			{
				throw new ArgumentException("Length of A and E must equal length of Node 1 and Node 2");
			}

			// Create one list to store the nodes and one list to store the bars
			List<Node> trussNodes = new List<Node>();
			List<Bar> trussBars = new List<Bar>();

			// To keep track if the node is unique
			bool unique1 = true;
			bool unique2 = true;

			// Topology matrix to keep track of element dofs
			List<List<int>> eDof = new List<List<int>>();


			// Loop trough each Node pair
			for (int i = 0; i < nodes1.Count; i++)
			{

				Node node1 = nodes1[i];
				Node node2 = nodes2[i];


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
					trussNodes.Add(node1);
				}

				if (unique2)
				{
					int id_node_2 = trussNodes.Count;
					node2.ID = id_node_2;
					node2.Dofs = System.Linq.Enumerable.Range(id_node_2*3, 3).ToList();
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

			DA.SetDataList("Bars", trussBars);
			DA.SetDataList("eDof", eDof);
			DA.SetDataList("Nodes", trussNodes);

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