using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MyProject.Web.ViewModels.Api.Vendor.VendorAccount
{
    public class VendorResetPasswordViewModel
    {
        [Required]
        public string Contact { get; set; }
        public string PhoneCode { get; set; }
    }
	public class NewVendorPasswordViewModel
	{
		[Required]
		public string Contact { get; set; }

		[Required]
		public string Password { get; set; }

		[Required]
		public int OTP { get; set; }
		
		public string PhoneCode { get; set; }
	}
}