using System;
using System.Collections.Generic;

namespace MyProject.Web.ViewModels.Api.Technician.Product
{
    public class FilterViewModel
    {
		public string search { get; set; }
		public string categories { get; set; }
		public Nullable<long> vendorID { get; set; }
		public Nullable<long> brandID { get; set; }
		public Nullable<decimal> minPrice { get; set; }
		public Nullable<decimal> maxPrice { get; set; }
		public List<FilterAttributesViewModel> attributes { get; set; }
		public Nullable<int> pageSize { get; set; }
		public Nullable<int> pageNumber { get; set; }
		public Nullable<int> sortBy { get; set; }
		public Nullable<bool> isFeatured { get; set; }
		public Nullable<bool> isSale { get; set; }
		public Nullable<bool> isRecommended { get; set; }
	}
	public class FilterAttributesViewModel
	{
		public Nullable<long> attributeID { get; set; }
		public List<string> values { get; set; }
	}
}