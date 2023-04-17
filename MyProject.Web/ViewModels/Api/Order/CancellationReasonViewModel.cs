using System.ComponentModel.DataAnnotations;

namespace MyProject.Web.ViewModels.Api.Order
{
	public class CancellationReasonViewModel
	{
		[Required]
		public string Reason { get; set; }
	}
}