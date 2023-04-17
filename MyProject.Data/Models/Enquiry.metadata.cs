using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyProject.Data
{
	[MetadataType(typeof(EnquiryAnnotation))]
	public partial class Enquiry
	{
	}

	internal sealed class EnquiryAnnotation
	{
		[Required(ErrorMessage = "The Full Name address is required")]
		public string FullName { get; set; }

		[Required(ErrorMessage = "The Phone Code is required")]
		public string PhoneCode { get; set; }

		[Required(ErrorMessage = "The Contact address is required")]
		public string Contact { get; set; }

		[Required(ErrorMessage = "The Email Address is required")]
        //[RegularExpression(@"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$", ErrorMessage = "Invalid Email Address")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

		[Required(ErrorMessage = "The Message is required")]
		public string Message { get; set; }
	}
}
