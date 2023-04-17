using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MyProject.Web.ViewModels.Api.ServiceRating
{
    public class ServiceRatingViewModel
    {
		[Required]
		public long ServiceID { get; set; }
		[Required]
		public long ServiceBookingID { get; set; }
		[Required]
		public decimal Rating { get; set; }
		[Required]
		public string Remarks { get; set; }
	}
}