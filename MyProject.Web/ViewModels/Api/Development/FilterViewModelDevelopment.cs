using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyProject.Web.ViewModels.Api.Development
{
    public class FilterViewModelDevelopment
    {
		public string search { get; set; }
		public Nullable<int> pageSize { get; set; }
		public Nullable<int> pageNumber { get; set; }
		public Nullable<int> sortBy { get; set; }

		public Nullable<int> displayLength { get; set; }
		public Nullable<int> displayStart { get; set; }
		public Nullable<int> sortCol { get; set; }
		public string sortDir { get; set; }
		public Nullable<long> anyID { get; set; }


		public Nullable<DateTime> startDate { get; set; }
		public Nullable<DateTime> endDate { get; set; }
	}
}