
namespace MyProject.Web.Areas.CustomerPortal.ViewModels.Order
{
	public class OrderFilterViewModel
	{
		public string Status { get; set; }
		public string ShipmentStatus { get; set; }
		public int PageNumber { get; set; }
		public int SortBy { get; set; }
		public string Lang { get; set; }
	}
}