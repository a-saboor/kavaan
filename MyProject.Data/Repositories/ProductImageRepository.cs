using MyProject.Data.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{
	class ProductImageRepository : RepositoryBase<ProductImage>, IProductImageRepository
	{
		public ProductImageRepository(IDbFactory dbFactory)
			: base(dbFactory) { }


		public IEnumerable<ProductImage> GetProductImages(long productId)
		{
			var ProductImages = this.DbContext.ProductImages.Where(c => c.ProductID == productId).ToList();
			return ProductImages;
		}
	}

	public interface IProductImageRepository : IRepository<ProductImage>
	{
		IEnumerable<ProductImage> GetProductImages(long productId);
	}
}
