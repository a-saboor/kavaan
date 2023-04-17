using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyProject.Web.Areas.CustomerPortal.ViewModels.Account
{
	public class ProfileViewModel
	{
		[Required(ErrorMessage = "The Name is required")]
		public string Name { get; set; }
		[Required(ErrorMessage = "The Email is required")]
		public string Email { get; set; }
		[Required(ErrorMessage = "The Contact is required")]
		public string Contact { get; set; }
		public string ImagePath { get; set; }
	}
}