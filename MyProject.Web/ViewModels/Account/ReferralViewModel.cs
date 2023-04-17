using System.ComponentModel.DataAnnotations;

namespace MyProject.Web.ViewModels.Account
{
	public class ReferralViewModel
	{

		[Display(Name = "Email address")]
		[Required(ErrorMessage = "The email address is required")]
		[EmailAddress(ErrorMessage = "Invalid Email Address")]
		public string Email { get; set; }
	}
}