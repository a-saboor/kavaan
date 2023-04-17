using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using static MyProject.Web.Helpers.Enumerations.Enumeration;

namespace MyProject.Web.ViewModels.Vendor
{
    public class VendorApprovalFormViewModel
	{
		[Required]
		public long ID { get; set; }
		[Required]
		public bool IsApproved { get; set; }
		[Required]
		public string Remarks { get; set; }
	}
}