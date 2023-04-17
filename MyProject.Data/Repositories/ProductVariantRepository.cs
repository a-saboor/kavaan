using MyProject.Data.Infrastructure;
using System.Linq;

namespace MyProject.Data.Repositories
{

	class ProductVariantRepository : RepositoryBase<ProductVariant>, IProductVariantRepository
	{
		public ProductVariantRepository(IDbFactory dbFactory)
			: base(dbFactory) { }

		public ProductVariant GetProductVariantBySKU(long productId, string sku, long id = 0)
		{
			var user = this.DbContext.ProductVariants.Where(c => c.ProductID == productId && c.SKU == sku && c.ID != id).FirstOrDefault();
			return user;
		}
	}

	public interface IProductVariantRepository : IRepository<ProductVariant>
	{
		ProductVariant GetProductVariantBySKU(long productId, string sku, long id = 0);
	}
}
