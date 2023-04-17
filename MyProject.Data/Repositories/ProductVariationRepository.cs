using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{

	class ProductVariationRepository : RepositoryBase<ProductVariation>, IProductVariationRepository
	{
		public ProductVariationRepository(IDbFactory dbFactory)
			: base(dbFactory) { }

		public ProductVariation GetProductVariation(long productId, string sku, long id = 0)
		{
			var ProductVariation = this.DbContext.ProductVariations.Where(c => c.ProductID == productId && c.SKU == sku && c.ID != id && c.IsDeleted == false).FirstOrDefault();
			return ProductVariation;
		}

		public IEnumerable<ProductVariation> GetProductVariations(long productId)
		{
			var ProductVariations = this.DbContext.ProductVariations.Where(c => c.ProductID == productId && c.IsDeleted == false).ToList();
			return ProductVariations;
		}

		public void DeleteMany(long productId)
		{
			var ProductVariations = this.DbContext.ProductVariations.Where(i => i.ProductID == productId).ToList();
			this.DbContext.ProductVariations.RemoveRange(ProductVariations);
		}

		public IEnumerable<SP_GetProductVaraitions_Result> GetProductVaraitions(long productId, string lang, string imageServer)
		{
			var ProductVariations = this.DbContext.SP_GetProductVaraitions(productId, lang, imageServer).ToList();
			return ProductVariations;
		}
		public bool InsertVariationProduct(bool IsActive, DateTime CreatedOn, long ProductID, decimal RegularPrice, decimal? SalePrice, DateTime? SalePriceFrom, DateTime? SalePriceTo, int Stock, int Threshold, int StockStatus, string SKU, string thumbnail, float Weight, float Length, float Width, float Height, string Description, bool IsSoldIndividually, bool IsManageStock)
		{
			try
			{
				DbContext.PR_InsertProductVariations(ProductID, SKU, thumbnail, RegularPrice, SalePrice, SalePriceFrom, SalePriceTo, Stock, Threshold, StockStatus, Weight, Length, Width, Height, Description, IsManageStock, IsSoldIndividually, CreatedOn, IsActive);
				return true;
			}
			catch (System.Exception ex)
			{
				//log.Error("Error", ex);
				//log.Error("Error", ex);
				return false;
			}
		}
		public ProductVariation GetProductSKU(string sku)
		{
			var user = this.DbContext.ProductVariations.Where(c => c.SKU == sku && c.IsDeleted == false).FirstOrDefault();
			return user;
		}


		public int CheckProductVariationStock(long productVariationId, int quantity)
		{
			var ProductStock = this.DbContext.SP_CheckProductVariationStock(productVariationId, quantity).SingleOrDefault().Value;
			return ProductStock;
		}
	}

	public interface IProductVariationRepository : IRepository<ProductVariation>
	{
		bool InsertVariationProduct(bool IsActive, DateTime CreatedOn, long ProductID, decimal RegularPrice, decimal? SalePrice, DateTime? SalePriceFrom, DateTime? SalePriceTo, int Stock, int Threshold, int StockStatus, string SKU, string thumbnail, float Weight, float Length, float Width, float Height, string Description, bool IsSoldIndividually, bool IsManageStock);
		ProductVariation GetProductSKU(string sku);
		ProductVariation GetProductVariation(long productId, string sku, long id = 0);
		IEnumerable<ProductVariation> GetProductVariations(long productId);
		void DeleteMany(long productId);
		IEnumerable<SP_GetProductVaraitions_Result> GetProductVaraitions(long productId, string lang, string imageServer);
		int CheckProductVariationStock(long productVariationId, int quantity);
	}
}
