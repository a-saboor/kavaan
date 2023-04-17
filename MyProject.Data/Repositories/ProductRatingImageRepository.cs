using MyProject.Data.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{
	class ProductRatingImageRepository : RepositoryBase<ProductRatingImage>, IProductRatingImageRepository
	{
		public ProductRatingImageRepository(IDbFactory dbFactory)
			: base(dbFactory) { }


		public IEnumerable<ProductRatingImage> GetProductRatingImages(long productRatingId)
		{
			var ProductRatingImages = this.DbContext.ProductRatingImages.Where(c => c.ProductRatingID == productRatingId).ToList();
			return ProductRatingImages;
		}
	}

	public interface IProductRatingImageRepository : IRepository<ProductRatingImage>
	{
		IEnumerable<ProductRatingImage> GetProductRatingImages(long productRatingId);
	}
}
