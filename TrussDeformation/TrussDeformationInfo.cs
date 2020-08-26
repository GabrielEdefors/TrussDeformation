using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace TrussDeformation
{
	public class TrussDeformationInfo : GH_AssemblyInfo
	{
		public override string Name
		{
			get
			{
				return "TrussDeformation";
			}
		}
		public override Bitmap Icon
		{
			get
			{
				//Return a 24x24 pixel bitmap to represent this GHA library.
				return null;
			}
		}
		public override string Description
		{
			get
			{
				//Return a short string describing the purpose of this GHA library.
				return "";
			}
		}
		public override Guid Id
		{
			get
			{
				return new Guid("fbb869f2-4ce3-4d67-b932-ea012dacc3df");
			}
		}

		public override string AuthorName
		{
			get
			{
				//Return a string identifying you or your company.
				return "";
			}
		}
		public override string AuthorContact
		{
			get
			{
				//Return a string representing your preferred contact details.
				return "";
			}
		}
	}
}
