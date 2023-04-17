using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MyProject.Web.ViewModels.Api.Vendor.VendorBooking
{
    public class CancelBookingViewModel
    {
        [Required(ErrorMessage = "BookingID is required")]
        public long BookingID{ get; set; }
        [Required(ErrorMessage = "Reason is required")]
        public string Reason { get; set; }
        public string Status { get; set; }
    }
}