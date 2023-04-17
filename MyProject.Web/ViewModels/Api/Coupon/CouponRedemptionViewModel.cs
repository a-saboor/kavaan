using System.ComponentModel.DataAnnotations;

namespace MyProject.Web.ViewModels.Api.Coupon
{
	public class CouponRedemptionViewModel
	{
		[Required(ErrorMessage = "The Coupon Code is required")]
		public string CouponCode { get; set; }

        [Required(ErrorMessage = "The Booking Id is required")]
        public long ServiceBookingID { get; set; }
    }
}