using MyProject.Data.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{

	class ProductVariationImageRepository : RepositoryBase<ProductVariationImage>, IProductVariationImageRepository
	{
		public ProductVariationImageRepository(IDbFactory dbFactory)
			: base(dbFactory) { }


		public IEnumerable<ProductVariationImage> GetProductVariationImages(long productVariationId)
		{
			var ProductVariationImages = this.DbContext.ProductVariationImages.Where(c => c.ProductVariationID == productVariationId).ToList();
			return ProductVariationImages;
		}
	}

	public interface IProductVariationImageRepository : IRepository<ProductVariationImage>
	{
		IEnumerable<ProductVariationImage> GetProductVariationImages(long productVariationId);
	}
}
