using System;
using System.Collections.Generic;

namespace MyProject.Web.ViewModels.Blog
{
	public class FilterViewModel
	{
		public string search { get; set; }
		public Nullable<int> pageSize { get; set; }
		public Nullable<int> pageNumber { get; set; }
		public Nullable<int> sortBy { get; set; }
		public Nullable<bool> vrTour { get; set; }

		public Nullable<int> displayLength { get; set; }
		public Nullable<int> displayStart { get; set; }
		public Nullable<int> sortCol { get; set; }
		public string sortDir { get; set; }
		public Nullable<long> parentID { get; set; }

		public List<string> features { get; set; } 
		public List<string> amenities { get; set; } 
		public Nullable<int> bedrooms { get; set; } 
		public Nullable<int> bathrooms { get; set; } 
		public Nullable<decimal> minprice { get; set; }
		public Nullable<decimal> maxprice { get; set; }
		public Nullable<long> typeId { get; set; }
		public Nullable<bool> isFeatured { get; set; }

		
		public Nullable<DateTime> startDate { get; set; }
		public Nullable<DateTime> endDate { get; set; }


	}
}