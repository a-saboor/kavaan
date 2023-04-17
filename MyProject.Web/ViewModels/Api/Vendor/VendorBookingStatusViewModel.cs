using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyProject.Web.ViewModels.Api.Vendor
{
    public class VendorBookingStatusViewModel
    {
        [Required]
        public long BookingID { get; set; }

        [Required]
        public string Status { get; set; }
    }
}