using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyProject.Data
{
	[MetadataType(typeof(CustomerAnnotation))]
	public partial class Customer
	{
	}

	internal sealed class CustomerAnnotation
	{
		[Required(ErrorMessage = "The First Name is required")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "The Phone Code is required")]
		public string PhoneCode { get; set; }

		[Required(ErrorMessage = "The Phone is required")]
		public string Contact { get; set; }

		[Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

	}
}
