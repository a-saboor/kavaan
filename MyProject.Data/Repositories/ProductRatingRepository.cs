using MyProject.Data.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{
	class ProductRatingRepository : RepositoryBase<ProductRating>, IProductRatingRepository
	{
		public ProductRatingRepository(IDbFactory dbFactory)
			: base(dbFactory) { }

		public ProductRating GetProductRating(long orderDetailId, long customerId, long id = 0)
		{
			var user = this.DbContext.ProductRatings.Where(c => c.OrderDetailID == orderDetailId && c.ID != id && c.CustomerID == customerId).FirstOrDefault();
			return user;
		}

		public ProductRating GetOrderDetailRating(long orderDetailId)
		{
			var user = this.DbContext.ProductRatings.Where(c => c.OrderDetailID == orderDetailId).FirstOrDefault();
			return user;
		}

		public List<SP_GetProductRatings_Result> GetProductRatings(long productId, string ImageServer)
		{
			var productRatings = this.DbContext.SP_GetProductRatings(productId, ImageServer).ToList();
			return productRatings;
		}

		public List<SP_GetVendorRatings_Result> GetVendorRatings(long vendorId, long pageNumber, string ImageServer)
		{
			var vendorRatings = this.DbContext.SP_GetVendorRatings(vendorId, pageNumber, ImageServer).ToList();
			return vendorRatings;
		}
	}

	public interface IProductRatingRepository : IRepository<ProductRating>
	{
		ProductRating GetProductRating(long orderDetailId, long customerId, long id = 0);
		ProductRating GetOrderDetailRating(long orderDetailId);
		List<SP_GetProductRatings_Result> GetProductRatings(long productId, string ImageServer);
		List<SP_GetVendorRatings_Result> GetVendorRatings(long vendorId, long pageNumber, string ImageServer);
	}
}
