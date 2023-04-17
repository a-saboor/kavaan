using MyProject.Data.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{
	class ProductCategoryRepository : RepositoryBase<ProductCategory>, IProductCategoryRepository
	{
		public ProductCategoryRepository(IDbFactory dbFactory)
			: base(dbFactory) { }

		public ProductCategory GetProductCategory(long productId, long categoryId, long id = 0)
		{
			var user = this.DbContext.ProductCategories.Where(c => c.ProductID == productId && c.ProductCategoryID == categoryId && c.ID != id).FirstOrDefault();
			return user;
		}

		public IEnumerable<ProductCategory> GetProductCategories(long productId)
		{
			var productCategories = this.DbContext.ProductCategories.Where(c => c.ProductID == productId).ToList();
			return productCategories;
		}

		public bool InsertProductCategories(long ProductID, string ProductCategories)
		{
			try
			{
				DbContext.PR_InsertProductCategories(ProductID, ProductCategories);
				return true;
			}
			catch (System.Exception ex)
			{
				//log.Error("Error", ex);
				//log.Error("Error", ex);
				return false;
			}
		}
	}

	public interface IProductCategoryRepository : IRepository<ProductCategory>
	{
		ProductCategory GetProductCategory(long productId, long categoryId, long id = 0);
		IEnumerable<ProductCategory> GetProductCategories(long productId);
		bool InsertProductCategories(long ProductID, string ProductCategories);
	}
}
