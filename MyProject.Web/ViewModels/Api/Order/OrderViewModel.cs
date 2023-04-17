using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyProject.Web.ViewModels.Api.Order
{
	public class OrderViewModel
	{
		public string BookingNo { get; set; }
		public long ServiceCategoryID { get; set; }
		public long ServiceID { get; set; }
		public long CustomerID { get; set; }
		public long VendorID { get; set; }
		public string CustomerName { get; set; }
		public string CustomerContact { get; set; }
		public string CustomerEmail { get; set; }
		public string CustomerAddress { get; set; }
		public DateTime DateOfVisit { get; set; }
		public TimeSpan  TimeOfVisit { get; set; }
		public string Description { get; set; }
		public Decimal DeliveryCharges { get; set; }
		public Decimal VisitCharges { get; set; }
		public Decimal SubTotal { get; set; }
		public Decimal TaxPercent { get; set; }
		public Decimal TaxAmount { get; set; }
		public Decimal CouponDiscount { get; set; }
		public decimal RedeemAmount { get; set; }
		public Decimal Total { get; set; }
		public string CouponCode { get; set; }
		public string Status { get; set; }
		public string PaymentMethod { get; set; }
		public string BookingStatus { get; set; }
		
	}

	public class EnsureOneElementAttribute : ValidationAttribute
	{
		public override bool IsValid(object value)
		{
			var list = value as IList;
			if (list != null)
			{
				return list.Count > 0;
			}
			return false;
		}
	}
}