using MyProject.Data.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{
	class ProductVariationAttributeRepository : RepositoryBase<ProductVariationAttribute>, IProductVariationAttributeRepository
	{
		public ProductVariationAttributeRepository(IDbFactory dbFactory)
			: base(dbFactory) { }

		public ProductVariationAttribute GetProductVariationAttribute(long productVariationId, long productAttributeId, long id = 0)
		{
			var productVariationAttributes = this.DbContext.ProductVariationAttributes.Where(c => c.ProductVariationID == productVariationId && c.ProductAttributeID == productAttributeId && c.ID != id).FirstOrDefault();
			return productVariationAttributes;
		}

		public IEnumerable<ProductVariationAttribute> GetVariationAttributesByProduct(long productId)
		{
			var productVariationAttributes = this.DbContext.ProductVariationAttributes.Where(c => c.ProductID == productId).ToList();
			return productVariationAttributes;
		}

		public IEnumerable<ProductVariationAttribute> GetProductVariationAttributes(long productVariationId)
		{
			var productVariationAttributes = this.DbContext.ProductVariationAttributes.Where(c => c.ProductVariationID == productVariationId).ToList();
			return productVariationAttributes;
		}

		public void DeleteMany(long productVariationId)
		{
			var productVariationAttributes = this.DbContext.ProductVariationAttributes.Where(i => i.ProductVariationID == productVariationId).ToList();
			this.DbContext.ProductVariationAttributes.RemoveRange(productVariationAttributes);
		}

		public bool InsertProductVariantAttributes(long ProductID, string AttributeName, string AttributeValue, long ProductVariatioID)
		{
			try
			{
				DbContext.PR_InsertProductVariationAttributes(ProductID, AttributeName, AttributeValue, ProductVariatioID);
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

	public interface IProductVariationAttributeRepository : IRepository<ProductVariationAttribute>
	{
		bool InsertProductVariantAttributes(long ProductID, string AttributeName, string AttributeValue, long ProductVariatioID);
		ProductVariationAttribute GetProductVariationAttribute(long productVariationId, long productAttributeId, long id = 0);
		IEnumerable<ProductVariationAttribute> GetVariationAttributesByProduct(long productId);
		IEnumerable<ProductVariationAttribute> GetProductVariationAttributes(long productVariationId);
		void DeleteMany(long productVariationId);
	}
}
