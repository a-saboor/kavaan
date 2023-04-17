using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyProject.Web.ViewModels.Api.Units
{
	public class UnitEnquiresViewModel
	{
		public long UnitID { get; set; }

		public long CustomerID { get; set; }

		public string Contact { get; set; }

		public string Email { get; set; }

		public string FullName { get; set; }

	}
}