using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MyProject.Web.ViewModels.Api.Vendor
{
    public class VendorBookingStaffViewModel
    {
        [Required]
        public long BookingID { get; set; }

        [Required]
        public long StaffID { get; set; }
    }
}