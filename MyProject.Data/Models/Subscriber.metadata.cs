using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyProject.Data
{
	[MetadataType(typeof(SubscriberAnnotation))]
	public partial class Subscriber
	{
	}

	internal sealed class SubscriberAnnotation
	{
		[Required(ErrorMessage = "The email address is required")]

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
	}
}
