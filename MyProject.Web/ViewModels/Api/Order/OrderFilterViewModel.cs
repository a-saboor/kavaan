using System;

namespace MyProject.Web.ViewModels.Api.Order
{
	public class OrderFilterViewModel
	{
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		
		public string Search { get; set; }
		public string Status { get; set; }
		public int? PageNumber { get; set; }
		public int? PageSize { get; set; }
		public int? SortBy { get; set; }
	}
}