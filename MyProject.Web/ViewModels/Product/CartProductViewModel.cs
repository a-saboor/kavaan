using System.Collections.Generic;

namespace MyProject.Web.ViewModels.Product
{
	public class CartProductViewModel
	{
		public List<long> ProductIds { get; set; }
		public List<long> ProductVariationIds { get; set; }
	}
}