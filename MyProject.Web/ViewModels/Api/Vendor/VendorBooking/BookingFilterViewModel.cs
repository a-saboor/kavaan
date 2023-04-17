using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MyProject.Web.ViewModels.Api.Vendor.VendorBooking
{
    public class BookingFilterViewModel
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