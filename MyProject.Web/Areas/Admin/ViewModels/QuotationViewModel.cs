using Project.Data;
using System;
using System.Collections.Generic;

namespace Project.Web.Areas.Admin.ViewModels
{
	public class QuotationViewModel
	{
		public IEnumerable<QuotationDetail> quotationDetails = new List<QuotationDetail>();
		
		public IEnumerable<ServiceBooking> ServiceBookings = new List<ServiceBooking>();
		
		public string BookingNo { get; set; }
		public string Status { get; set; }
		public string ServiceName { get; set; }
		public string ServiceCategoryName { get; set; }
		public string CustomerName { get; set; }
		public string CustomerContact { get; set; }
		public string CustomerLogo { get; set; }
		public string MapLocation { get; set; }
		public string CancellationReason { get; set; }
		public bool IsPaid { get; set; }
		public decimal Amount { get; set; }
		public decimal Tax { get; set; }
		public decimal TotalAmount { get; set; }

		public Nullable<DateTime> DeliveryDate { get; set; }
		public Nullable<DateTime> CreatedOn { get; set; }

	}

}