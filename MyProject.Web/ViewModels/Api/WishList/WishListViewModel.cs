using System.ComponentModel.DataAnnotations;

namespace MyProject.Web.ViewModels.Api.WishList
{
	public class WishListViewModel
	{
		[Required]
		public long CarID { get; set; }
	}
}