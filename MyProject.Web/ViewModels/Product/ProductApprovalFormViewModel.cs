using System.ComponentModel.DataAnnotations;
using static MyProject.Web.Helpers.Enumerations.Enumeration;

namespace MyProject.Web.ViewModels.Product
{
	public class ProductApprovalFormViewModel
	{
		[Required]
		public long ID { get; set; }
		[Required]
		public bool IsApproved { get; set; }
		[Required]
		public string Remarks { get; set; }
	}
}