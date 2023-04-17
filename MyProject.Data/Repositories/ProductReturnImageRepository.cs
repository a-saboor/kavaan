using MyProject.Data.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{

	class ProductReturnImageRepository : RepositoryBase<ProductReturnImage>, IProductReturnImageRepository
	{
		public ProductReturnImageRepository(IDbFactory dbFactory)
			: base(dbFactory) { }

		public IEnumerable<ProductReturnImage> GetProductReturnImages(long productReturnId)
		{
			var ProductReturnImages = this.DbContext.ProductReturnImages.Where(c => c.ProductReturnID == productReturnId).ToList();
			return ProductReturnImages;
		}
	}

	public interface IProductReturnImageRepository : IRepository<ProductReturnImage>
	{
		IEnumerable<ProductReturnImage> GetProductReturnImages(long productReturnId);
	}
}
