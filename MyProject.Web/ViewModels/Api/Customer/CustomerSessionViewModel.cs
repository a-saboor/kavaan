using System.ComponentModel.DataAnnotations;

namespace MyProject.Web.ViewModels.Api.Customer
{
	public class CustomerSessionViewModel
	{
		[Required]
		public string FirebaseToken { get; set; }
		[Required]
		public string DeviceID { get; set; }
		
		public string AccessToken { get; set; }
	}
}