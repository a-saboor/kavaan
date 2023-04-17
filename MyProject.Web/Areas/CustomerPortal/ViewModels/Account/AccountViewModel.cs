using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyProject.Web.Areas.CustomerPortal.ViewModels.Account
{
	public class CustomerSignupViewModel
	{
		[Required(ErrorMessage = "Please enter First Name,")]
		[MaxLength(100, ErrorMessage = "First Name cannot be more than 100 characters.")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Please enter Last Name,")]
		[MaxLength(100, ErrorMessage = "Last Name cannot be more than 100 characters.")]
		public string LastName { get; set; }

		[Required(ErrorMessage = "Please enter email address.")]
		[MaxLength(50, ErrorMessage = "Email address cannot be more than 50 characters.")]
		[EmailAddress(ErrorMessage = "Invalid email address")]
		public string Email { get; set; }

		public string PhoneCode { get; set; } = "";// if client wants, change it

		//[Required(ErrorMessage = "Please enter phone number.")]
		//[MaxLength(15, ErrorMessage = "Phone number cannot be more than 15 characters.")]
		public string Contact { get; set; } = "";// if client wants, change it

		[Required(ErrorMessage = "Please enter password.")]
		[MaxLength(25, ErrorMessage = "Password cannot be more than 25 characters.")]
		public string Password { get; set; }
	}
	public class CustomerLoginViewModel
	{
		//[Required(ErrorMessage = "Please enter phone number.")]
		//[MaxLength(15, ErrorMessage = "Phone number cannot be more than 15 characters.")]
		public string Contact { get; set; }
		public string PhoneCode { get; set; }

		[Required(ErrorMessage = "Please enter email address.")]
		[MaxLength(50, ErrorMessage = "Email address cannot be more than 50 characters.")]
		//[EmailAddress(ErrorMessage = "Invalid email address")]
		public string Email { get; set; }
		public string UserName { get; set; }

		[Required(ErrorMessage = "Please enter password.")]
		[MaxLength(25, ErrorMessage = "Password cannot be more than 25 characters.")]
		public string Password { get; set; }
		public string ReturnUrl { get; set; }
	}
	public class CustomerVerifyOTPViewModel
	{
		//[Required(ErrorMessage = "Phone number is required.")]
		//[MaxLength(15, ErrorMessage = "Phone number cannot be more than 15 characters.")]
		public string PhoneCode { get; set; }
		public string Contact { get; set; }

		[Required(ErrorMessage = "Please enter email address.")]
		[MaxLength(50, ErrorMessage = "Email address cannot be more than 50 characters.")]
		[EmailAddress(ErrorMessage = "Invalid email address")]
		public string Email { get; set; }

		public string UserName { get; set; }

		[Required(ErrorMessage = "OTP verification code is required.")]
		public int OTP { get; set; }

		public bool? AutoLogin { get; set; } = true;
		public bool? OTPExpired { get; set; } = false;

	}
	public class CustomerReferralViewModel
	{
		[Display(Name = "Email")]
		[Required(ErrorMessage = "The email address is required.")]
		[MaxLength(50, ErrorMessage = "Email address cannot be more than 50 characters.")]
		[EmailAddress(ErrorMessage = "Invalid email address.")]
		public string Email { get; set; }
	}
	public class CustomerForgotPasswordViewModel
	{
		[Display(Name = "Email")]
		[Required(ErrorMessage = "The email address is required.")]
		[MaxLength(50, ErrorMessage = "Email address cannot be more than 50 characters.")]
		[EmailAddress(ErrorMessage = "Invalid email address.")]
		public string Email { get; set; }
	}

	//public class CustomerResetPasswordViewModel
	//{
	//	[Required(ErrorMessage = "New password is required.")]
	//	[MaxLength(25, ErrorMessage = "Password cannot be more than 25 characters.")]
	//	public string NewPassword { get; set; }

	//	[Required(ErrorMessage = "Confirm Password is required.")]
	//	[Compare("NewPassword", ErrorMessage = "Confirm password doesn't match, Type again!")]
	//	public string ConfirmPassword { get; set; }
	//}
	public class CustomerResetPasswordViewModel
	{
		//[Required(ErrorMessage = "The email address is required.")]
		//[MaxLength(50, ErrorMessage = "Email address cannot be more than 50 characters.")]
		//[EmailAddress(ErrorMessage = "Invalid email address.")]
		//public string Email { get; set; }

		//[Required(ErrorMessage = "Current Password is required.")]
		//public string CurrentPassword { get; set; }

		[Required(ErrorMessage = "New Password is required.")]
		[MaxLength(25, ErrorMessage = "Password cannot be more than 25 characters.")]
		public string NewPassword { get; set; }

		//[Required(ErrorMessage = "Contact Number is required.")]
		//[MaxLength(20, ErrorMessage = "Contact Number cannot be more than 20 characters.")]
		//public string Contact { get; set; }
	}
	public class CustomerNewPasswordViewModel
	{
		[Required(ErrorMessage = "Contact Number is required.")]
		[MaxLength(20, ErrorMessage = "Contact Number cannot be more than 20 characters.")]
		public string Contact { get; set; }

		[Required(ErrorMessage = "New password is required.")]
		[MaxLength(25, ErrorMessage = "Password cannot be more than 25 characters.")]
		public string Password { get; set; }

		[Required(ErrorMessage = "OTP is required.")]
		public int OTP { get; set; }
	}
	public class CustomerChangePasswordViewModel
	{
		[Required(ErrorMessage = "Current Password is required.")]
		public string CurrentPassword { get; set; }

		[Required(ErrorMessage = "New Password is required.")]
		[MaxLength(25, ErrorMessage = "Password cannot be more than 25 characters.")]
		public string NewPassword { get; set; }

		[Required(ErrorMessage = "Confirm Password is required.")]
		[Compare("NewPassword", ErrorMessage = "Confirm password doesn't match, Type again!")]
		public string ConfirmPassword { get; set; }
	}
}