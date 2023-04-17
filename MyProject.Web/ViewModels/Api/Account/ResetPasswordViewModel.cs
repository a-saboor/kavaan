using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyProject.Web.ViewModels.Api.Account
{
	public class ResetPasswordViewModel
	{
		[Required]
		public string Contact { get; set; }
		[Required]
		public string PhoneCode { get; set; }
	}

	public class NewPasswordViewModel
	{
		[Required]
		public string Contact { get; set; }

		[Required]
		public string Password { get; set; }

		[Required]
		public int OTP { get; set; }
		[Required]
		public string PhoneCode { get; set; }
	}
}