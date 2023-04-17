using MyProject.Data.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{
	class ProductTagRepository : RepositoryBase<ProductTag>, IProductTagRepository
	{
		public ProductTagRepository(IDbFactory dbFactory)
			: base(dbFactory) { }

		public ProductTag GetProductTag(long productId, long tagId, long id = 0)
		{
			var ProductTag = this.DbContext.ProductTags.Where(c => c.ProductID == productId && c.TagID == tagId && c.ID != id).FirstOrDefault();
			return ProductTag;
		}

		public IEnumerable<ProductTag> GetProductTags(long productId)
		{
			var ProductTags = this.DbContext.ProductTags.Where(c => c.ProductID == productId).ToList();
			return ProductTags;
		}
        public bool InsertProductTags(long ProductID, string ProductTags)
        {
            try
            {
                DbContext.PR_InsertProductTags(ProductID, ProductTags);
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

	public interface IProductTagRepository : IRepository<ProductTag>
	{
        bool InsertProductTags(long ProductID, string ProductTags);

        ProductTag GetProductTag(long productId, long categoryId, long id = 0);
		IEnumerable<ProductTag> GetProductTags(long productId);
	}
}
