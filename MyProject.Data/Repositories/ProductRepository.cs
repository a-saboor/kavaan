using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{

	class ProductRepository : RepositoryBase<Product>, IProductRepository
	{
		public ProductRepository(IDbFactory dbFactory)
			: base(dbFactory) { }

		public Product GetProductBySKU(string sku, long vendorId, long id = 0)
		{
			var user = this.DbContext.Products.Where(c => c.SKU == sku && c.VendorID == vendorId && c.ID != id && c.IsDeleted == false).FirstOrDefault();
			return user;
		}

		public Product GetProductSKU(string sku, long vendorId)
		{
			var user = this.DbContext.Products.Where(c => c.SKU == sku && c.VendorID == vendorId && c.IsDeleted == false).FirstOrDefault();
			return user;
		}

		public Product GetProductbySlug(string slug)
		{
			var product = this.DbContext.Products.Where(c => c.Slug == slug && c.IsDeleted == false).SingleOrDefault();
			return product;
		}

		public IEnumerable<SP_GetProducts_Result> GetProductsByVendor(int displayLength, int displayStart, int sortCol, string sortDir, string search, long? vendorId, bool isApproved = true)
		{
			var Products = this.DbContext.SP_GetProducts(displayLength, displayStart, sortCol, sortDir, search, vendorId, isApproved).ToList();
			return Products;
		}
		//public IEnumerable<SP_GetFilteredProducts_Result> GetFilteredProducts(string search, string categories, Nullable<long> vendorID, Nullable<long> brandID, Nullable<decimal> minPrice, Nullable<decimal> maxPrice, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string filters, Nullable<System.DateTime> currentDateTime, string lang, string imageServer, bool? isFeatured, bool? isRecommended, bool? isSale, bool? isTrending)
		//{
		//	var Products = this.DbContext.SP_GetFilteredProducts(search, categories, vendorID, brandID, minPrice, maxPrice, pageSize, pageNumber, sortBy, filters, currentDateTime, lang, imageServer, isFeatured, isRecommended, isSale, isTrending).ToList();
		//	return Products;
		//}

		//public IEnumerable<SP_GetDetailProducts_Result> SP_GetDetailProducts(string ImageServer, long? vendorId)
		//{
		//	var Products = this.DbContext.SP_GetDetailProducts(ImageServer, vendorId);
		//	return Products;
		//}

		//public IEnumerable<SP_GetDetailProductsVariations_Result> SP_GetDetailProductsVariations(string ImageServer, long? vendorId)
		//{
		//	var Products = this.DbContext.SP_GetDetailProductsVariations(ImageServer, vendorId);
		//	return Products;
		//}

		public IEnumerable<SP_GetProducts_Result> GetProductsByVendorUnApproved(int displayLength, int displayStart, int sortCol, string sortDir, string search, long vendorId, bool isApproved = false)
		{
			var Products = this.DbContext.SP_GetProducts(displayLength, displayStart, sortCol, sortDir, search, vendorId, isApproved).ToList();
			return Products;
		}

        public IEnumerable<SP_GetFilteredProducts_Result> GetFilteredProducts(string search, string categories, Nullable<long> vendorID, Nullable<long> brandID, Nullable<decimal> minPrice, Nullable<decimal> maxPrice, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string filters, Nullable<System.DateTime> currentDateTime, string lang, string imageServer, bool? isFeatured, bool? isRecommended, bool? isSale, bool? isTrending)
        {
            var Products = this.DbContext.SP_GetFilteredProducts(search, categories, vendorID, brandID, minPrice, maxPrice, pageSize, pageNumber, sortBy, filters, currentDateTime, lang, imageServer, isFeatured, isRecommended, isSale, isTrending).ToList();
            return Products;
        }

        //public IEnumerable<SP_GetPublishedProducts_Result> GetPublishedProducts(Nullable<System.DateTime> currentDateTime, string search, Nullable<long> categoryID, Nullable<long> vendorID, Nullable<long> brandID, Nullable<decimal> minPrice, Nullable<decimal> maxPrice, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string filters, string lang, string imageServer, bool? isFeatured, bool? IsRecommended, bool? IsSale)
        //{
        //	var Products = this.DbContext.SP_GetPublishedProducts(currentDateTime, search, categoryID, vendorID, brandID, minPrice, maxPrice, pageSize, pageNumber, sortBy, filters, lang, imageServer, isFeatured, IsRecommended, IsSale).ToList();
        //	return Products;
        //}

        public IEnumerable<SP_GetCategorywiseProducts_Result> GetCategorywsieProducts(Nullable<System.DateTime> currentDateTime, string search, Nullable<long> categoryID, string lang, string imageServer)
        {
            var Products = this.DbContext.SP_GetCategorywiseProducts(currentDateTime, categoryID, search, lang, imageServer).ToList();
            return Products;
        }

        public IEnumerable<SP_GetRelatedProducts_Result> GetRelatedProducts(Nullable<System.DateTime> currentDateTime, Nullable<long> productID, string lang, string imageServer)
		{
			var Products = this.DbContext.SP_GetRelatedProducts(currentDateTime, productID, lang, imageServer).ToList();
			return Products;
		}

		public SP_GetProductDetails_Result GetProductDetails(long productId, string lang, string imageServer)
		{
			var Product = this.DbContext.SP_GetProductDetails(productId, lang, imageServer).FirstOrDefault();
			return Product;
		}

		public bool InsertProduct(long VendorID
			, string Brand
			, string SKU
			, string Slug
			, string Name
			, string thumbnail
			, string ShortDescription
			, string LongDescription
			, string MobileDescription
			, decimal RegularPrice
			, decimal? SalePrice
			, DateTime? SalePriceFrom
			, DateTime? SalePriceTo
			, int Stock
			, int Threshold
			, string Type
			, float Weight
			, float Length
			, float Width
			, float Height
			, bool IsSoldIndividually
			, bool AllowMultipleOrder
			, string Status
			, int StockStatus
			, bool IsFeatured
			, bool IsRecommended
			, bool IsManageStock
			, DateTime CreatedOn
			, bool IsActive
			, bool isApproved
			, int ApprovalStatus
			, bool IsDeleted
			, string ProductCategories
			, string ProductTags)
		{
			try
			{
				if (DbContext.Database.CurrentTransaction != null)
				{
					DbContext.Database.CurrentTransaction.Dispose();
					DbContext.Database.BeginTransaction();
				}

				var result = DbContext.PR_InsertProduct(VendorID, Brand, SKU, Slug, Name, thumbnail, ShortDescription, LongDescription,MobileDescription, RegularPrice, SalePrice, SalePriceFrom, SalePriceTo, Stock, Threshold, Type, Weight, Length, Width, Height, IsSoldIndividually, AllowMultipleOrder, Status, StockStatus, IsFeatured,IsRecommended, IsManageStock, CreatedOn, IsActive, isApproved, ApprovalStatus, IsDeleted, ProductCategories, ProductTags);
				if (result >= 1)
				{
					return true;
				}
				else
					return false;
			}
			catch (System.Exception ex)
			{
				return false;
			}
		}


		public List<SP_CheckProductStockAvailability_Result> CheckProductStockAvailability(string productIds, string productVariationIds)
		{
			var ProductStock = this.DbContext.SP_CheckProductStockAvailability(productIds, productVariationIds).ToList();
			return ProductStock;
		}

		public int CheckProductStock(long productId, int quantity)
		{
			var ProductStock = this.DbContext.SP_CheckProductStock(productId, quantity).SingleOrDefault().Value;
			return ProductStock;
		}

		public List<Product> GetProductsUnApprovedList(long VendorID, bool isApproved)
		{
			var Product = this.DbContext.Products.Where(x => x.VendorID == VendorID && x.IsApproved == isApproved && x.IsDeleted == false).ToList();
			return Product;
		}

		public IEnumerable<Product> GetProductsByVendorID(long vendorId)
		{
			var product = this.DbContext.Products.Where(c => c.VendorID == vendorId && c.IsDeleted == false).ToList();
			return product;
		}

		public bool UpdateProductApprovalStatus(long productID)
		{
			try
			{
				this.DbContext.PR_UpdateProductApprovalStatus(productID);
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}
	}

	public interface IProductRepository : IRepository<Product>
	{
		Product GetProductSKU(string sku, long vendorId);
		Product GetProductbySlug(string slug);
		bool InsertProduct(long VendorID
				, string Brand
				, string SKU
				, string Slug
				, string Name
				, string thumbnail
				, string ShortDescription
				, string LongDescription
				, string MobileDescription
				, decimal RegularPrice
				, decimal? SalePrice
				, DateTime? SalePriceFrom
				, DateTime? SalePriceTo
				, int Stock
				, int Threshold
				, string Type
				, float Weight
				, float Length
				, float Width
				, float Height
				, bool IsSoldIndividually
				, bool AllowMultipleOrder
				, string Status
				, int StockStatus
				, bool IsFeatured
				, bool IsRecommended
				, bool IsManageStock
				, DateTime CreatedOn
				, bool IsActive
				, bool isApproved
				, int ApprovalStatus
				, bool IsDeleted
				, string ProductCategories
				, string ProductTags);
		Product GetProductBySKU(string sku, long vendorId, long id = 0);
		IEnumerable<SP_GetProducts_Result> GetProductsByVendor(int displayLength, int displayStart, int sortCol, string sortDir, string search, long? vendorId, bool isApproved = true);

        IEnumerable<SP_GetFilteredProducts_Result> GetFilteredProducts(string search, string categories, Nullable<long> vendorID, Nullable<long> brandID, Nullable<decimal> minPrice, Nullable<decimal> maxPrice, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string filters, Nullable<System.DateTime> currentDateTime, string lang, string imageServer, bool? isFeatured, bool? isRecommended, bool? isSale, bool? isTrending);

        //IEnumerable<SP_GetPublishedProducts_Result> GetPublishedProducts(Nullable<System.DateTime> currentDateTime, string search, Nullable<long> categoryID, Nullable<long> vendorID, Nullable<long> brandID, Nullable<decimal> minPrice, Nullable<decimal> maxPrice, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string filters, string lang, string imageServer, bool? isFeatured, bool? IsRecommended, bool? IsSale);

        IEnumerable<SP_GetCategorywiseProducts_Result> GetCategorywsieProducts(Nullable<System.DateTime> currentDateTime, string search, Nullable<long> categoryID, string lang, string imageServer);

		IEnumerable<SP_GetRelatedProducts_Result> GetRelatedProducts(Nullable<System.DateTime> currentDateTime, Nullable<long> productID, string lang, string imageServer);

		SP_GetProductDetails_Result GetProductDetails(long productId, string lang, string imageServer);

		IEnumerable<SP_GetProducts_Result> GetProductsByVendorUnApproved(int displayLength, int displayStart, int sortCol, string sortDir, string search, long vendorId, bool isApproved = true);

		List<SP_CheckProductStockAvailability_Result> CheckProductStockAvailability(string productIds, string productVariationIds);

		int CheckProductStock(long productId, int quantity);

		//IEnumerable<SP_GetDetailProducts_Result> SP_GetDetailProducts(string ImageServer, long? vendorId);
		//IEnumerable<SP_GetDetailProductsVariations_Result> SP_GetDetailProductsVariations(string ImageServer, long? vendorId);

		List<Product> GetProductsUnApprovedList(long VendorID, bool isApproved);

		IEnumerable<Product> GetProductsByVendorID(long vendorId);
		bool UpdateProductApprovalStatus(long productID);
	}
}
