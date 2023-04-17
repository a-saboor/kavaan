using System.ComponentModel.DataAnnotations;

namespace MyProject.Web.ViewModels.Api.Account
{
	public class LogoutViewModel
	{
		[Required]
		public string DeviceID { get; set; }
	}
}