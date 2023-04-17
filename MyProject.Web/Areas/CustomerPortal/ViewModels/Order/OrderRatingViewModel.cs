using System;
using System.ComponentModel.DataAnnotations;

namespace MyProject.Web.Areas.CustomerPortal.ViewModels.Order
{
	public class OrderRatingViewModel
	{
		[Required(ErrorMessage = "The Rating is required")]
		public double Rating { get; set; }
		public string Remarks { get; set; }
	}
}