using System.ComponentModel.DataAnnotations;

namespace MyProject.Web.ViewModels.Api.Customer
{
	public class ProfilePhotoViewModel
	{
		[Required(ErrorMessage = "The Image is required")]
		public string Image { get; set; }
	}
}