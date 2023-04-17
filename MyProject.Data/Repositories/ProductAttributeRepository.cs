using MyProject.Data.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System;

namespace MyProject.Data.Repositories
{

	class ProductAttributeRepository : RepositoryBase<ProductAttribute>, IProductAttributeRepository
	{
		public ProductAttributeRepository(IDbFactory dbFactory)
			: base(dbFactory) { }

		public ProductAttribute GetProductAttribute(long productId, long attributeId, string value, long id = 0)
		{
			var ProductAttribute = this.DbContext.ProductAttributes.Where(c => c.ProductID == productId && c.AttributeID == attributeId && c.Value == value && c.ID != id && c.IsDeleted == false).FirstOrDefault();
			return ProductAttribute;
		}

		public ProductAttribute GetProductAttribute(long productId, string attribute, string value)
		{
			var ProductAttribute = this.DbContext.ProductAttributes.Where(c => c.ProductID == productId && c.Attribute.Name.ToString() == attribute && c.Value.ToUpper() == value && c.IsDeleted == false).FirstOrDefault();
			return ProductAttribute;
		}

		public IEnumerable<ProductAttribute> GetProductAttributes(long productId)
		{
			var ProductAttributes = this.DbContext.ProductAttributes.Where(c => c.ProductID == productId && c.IsDeleted == false).ToList();
			return ProductAttributes;
		}

		public bool DeleteMany(ProductAttribute productAttribute)
		{
			try
			{
				this.DbContext.SP_DeleteProductAttributeValues(productAttribute.ProductID, productAttribute.AttributeID);
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public IEnumerable<SP_GetProductAttributes_Result> GetProductAttributes(long productId, string lang)
		{
			var ProductAttributes = this.DbContext.SP_GetProductAttributes(productId, lang).ToList();
			return ProductAttributes;
		}

		public int InsertProductAttributes(long ProductID, string AttributeName, string AttributeValues, DateTime CreatedOn, bool VariationUsage)
		{
			try
			{
				return DbContext.PR_InsertProductAttributes(ProductID, AttributeName, AttributeValues, CreatedOn, VariationUsage);

			}
			catch (System.Exception ex)
			{
				//log.Error("Error", ex);
				//log.Error("Error", ex);
				return -1;
			}
		}

		public bool UpdateProductAttributesVariationUsage()
		{
			try
			{
				this.DbContext.SP_UpdateProductAttributesVariationUsage();
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}
	}

	public interface IProductAttributeRepository : IRepository<ProductAttribute>
	{
		int InsertProductAttributes(long ProductID, string AttributeName, string AttributeValues, DateTime CreatedOn, bool VariationUsage);
		ProductAttribute GetProductAttribute(long productId, long attributeId, string value, long id = 0);
		ProductAttribute GetProductAttribute(long productId, string attribute, string value);
		IEnumerable<ProductAttribute> GetProductAttributes(long productId);
		bool DeleteMany(ProductAttribute productAttribute);
		IEnumerable<SP_GetProductAttributes_Result> GetProductAttributes(long productId, string lang);
		bool UpdateProductAttributesVariationUsage();
	}
}
