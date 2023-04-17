using MyProject.Data.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{
	class ProductAttributeSettingRepository : RepositoryBase<ProductAttributeSetting>, IProductAttributeSettingRepository
	{
		public ProductAttributeSettingRepository(IDbFactory dbFactory)
			: base(dbFactory) { }

		public ProductAttributeSetting GetProductAttributeSetting(long productId, long attributeId, long id = 0)
		{
			var ProductAttributeSetting = this.DbContext.ProductAttributeSettings.Where(c => c.ProductID == productId
			&& c.AttributeID == attributeId
			&& c.ID != id).FirstOrDefault();
			return ProductAttributeSetting;
		}

		public IEnumerable<ProductAttributeSetting> GetProductAttributesSetting(long productId)
		{
			var ProductAttributes = this.DbContext.ProductAttributeSettings.Where(c => c.ProductID == productId).ToList();
			return ProductAttributes;
		}
	}

	public interface IProductAttributeSettingRepository : IRepository<ProductAttributeSetting>
	{
		ProductAttributeSetting GetProductAttributeSetting(long productId, long attributeId, long id = 0);
		IEnumerable<ProductAttributeSetting> GetProductAttributesSetting(long productId);
	}
}
