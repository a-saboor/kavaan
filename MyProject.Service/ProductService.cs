using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
	public class ProductService : IProductService
	{
		private readonly IProductRepository _productRepository;
		private readonly IUnitOfWork _unitOfWork;

		public ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork)
		{
			this._productRepository = productRepository;
			this._unitOfWork = unitOfWork;
		}

		#region IProductService Members

		public IEnumerable<Product> GetProducts()
		{
			var products = _productRepository.GetAll().Where(x => x.ApprovalStatus == 2);
			return products;
		}

		public Product GetProductbySKU(string SKU, long vendorId)
		{
			var products = _productRepository.GetProductSKU(SKU, vendorId);
			return products;
		}

		public Product GetProductbySlug(string slug)
		{
			var product = _productRepository.GetProductbySlug(slug);
			return product;
		}

		public IEnumerable<SP_GetProducts_Result> GetVendorProducts(int displayLength, int displayStart, int sortCol, string sortDir, string search, long? vendorId, bool IsApproved = true)
		{
			var products = _productRepository.GetProductsByVendor(displayLength, displayStart, sortCol, sortDir, search, vendorId, IsApproved);
			return products;
		}

		public IEnumerable<SP_GetProducts_Result> GetVendorProductsUnapproved(int displayLength, int displayStart, int sortCol, string sortDir, string search, long vendorId, bool IsApproved = false)
		{
			var products = _productRepository.GetProductsByVendorUnApproved(displayLength, displayStart, sortCol, sortDir, search, vendorId, IsApproved);
			return products;
		}

		public IEnumerable<SP_GetFilteredProducts_Result> GetFilteredProducts(string search, string categories, Nullable<long> vendorID, Nullable<long> brandID, Nullable<decimal> minPrice, Nullable<decimal> maxPrice, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string filters, string lang, string imageServer, bool? isFeatured, bool? isRecommended, bool? isSale, bool? isTrending)
		{
			var currentDateTime = Helpers.TimeZone.GetLocalDateTime();
			var Products = _productRepository.GetFilteredProducts(search, categories, vendorID, brandID, minPrice, maxPrice, pageSize, pageNumber, sortBy, filters, currentDateTime, lang, imageServer, isFeatured, isRecommended, isSale, isTrending).ToList();
			return Products;
		}

		//public IEnumerable<SP_GetPublishedProducts_Result> GetPublishedProducts(string search, Nullable<long> categoryID, Nullable<long> vendorID, Nullable<long> brandID, Nullable<decimal> minPrice, Nullable<decimal> maxPrice, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string filters, string lang, string imageServer, bool? isFeatured, bool? IsRecommended, bool? IsSale)
		//{
		//	var currentDateTime = Helpers.TimeZone.GetLocalDateTime();
		//	var Products = _productRepository.GetPublishedProducts(currentDateTime, search, categoryID, vendorID, brandID, minPrice, maxPrice, pageSize, pageNumber, sortBy, filters, lang, imageServer, isFeatured, IsRecommended, IsSale).ToList();
		//	return Products;
		//}

		public IEnumerable<SP_GetCategorywiseProducts_Result> GetCategorywsieProducts(string search, Nullable<long> categoryID, string lang, string imageServer)
		{
			var currentDateTime = Helpers.TimeZone.GetLocalDateTime();
			var Products = _productRepository.GetCategorywsieProducts(currentDateTime, search, categoryID, lang, imageServer).ToList();
			return Products;
		}

		public IEnumerable<SP_GetRelatedProducts_Result> GetRelatedProducts(Nullable<long> productId, string lang, string imageServer)
		{
			var currentDateTime = Helpers.TimeZone.GetLocalDateTime();
			var Products = _productRepository.GetRelatedProducts(currentDateTime, productId, lang, imageServer).ToList();
			return Products;
		}

		public SP_GetProductDetails_Result GetProductDetails(long productId, string lang, string imageServer)
		{
			var currentDateTime = Helpers.TimeZone.GetLocalDateTime();
			var Product = _productRepository.GetProductDetails(productId, lang, imageServer);
			return Product;
		}

		public IEnumerable<object> GetProductsForDropDown()
		{
			var Products = _productRepository.GetAll();
			var dropdownList = from products in Products
							   select new { value = products.ID, text = products.Name };
			return dropdownList;
		}

		public Product GetProduct(long id)
		{
			var product = _productRepository.GetById(id);
			return product;
		}

		public bool CreateProduct(Product product, ref string message)
		{
			try
			{
				if (_productRepository.GetProductBySKU(product.SKU, (long)product.VendorID) == null)
				{
					product.Status = "Draft";
					product.IsPublished = product.IsPublished.HasValue ? product.IsPublished.Value : false;
					product.IsFeatured = product.IsFeatured.HasValue ? product.IsFeatured.Value : false;
					product.IsApproved = product.IsApproved.HasValue ? product.IsApproved.Value : false;
					product.IsActive = product.IsActive.HasValue ? product.IsActive.Value : true;
					product.IsDeleted = product.IsDeleted.HasValue ? product.IsDeleted.Value : false;
					product.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
					_productRepository.Add(product);
					if (SaveProduct())
					{
						message = "Product added successfully ...";
						return true;

					}
					else
					{
						message = "Oops! Something went wrong. Please try later.";
						return false;
					}
				}
				else
				{
					message = "Product already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool UpdateProduct(ref Product product, ref string message, bool updater = true)
		{
			try
			{
				if (_productRepository.GetProductBySKU(product.SKU, (long)product.VendorID, product.ID) == null)
				{
					Product CurrentProduct = new Product();
					if (updater)
					{
						CurrentProduct = _productRepository.GetById(product.ID);

						CurrentProduct.SKU = product.SKU;
						CurrentProduct.Name = product.Name;
						CurrentProduct.NameAr = product.NameAr;
						CurrentProduct.Slug = product.Slug;
						CurrentProduct.MobileDescription = product.MobileDescription;
						CurrentProduct.MobileDescriptionAr = product.MobileDescriptionAr;
						CurrentProduct.ShortDescription = product.ShortDescription;
						CurrentProduct.ShortDescriptionAr = product.ShortDescriptionAr;
						CurrentProduct.LongDescription = product.LongDescription;
						CurrentProduct.LongDescriptionAr = product.LongDescriptionAr;
						CurrentProduct.RegularPrice = product.RegularPrice;
						CurrentProduct.SalePrice = product.SalePrice;
						CurrentProduct.SalePriceFrom = product.SalePriceFrom;
						CurrentProduct.SalePriceTo = product.SalePriceTo;
						CurrentProduct.Stock = product.Stock;
						CurrentProduct.Threshold = product.Threshold;
						CurrentProduct.StockStatus = product.StockStatus;
						CurrentProduct.Weight = product.Weight;
						CurrentProduct.Length = product.Length;
						CurrentProduct.Width = product.Width;
						CurrentProduct.Height = product.Height;
						CurrentProduct.Type = product.Type;
						CurrentProduct.BrandID = product.BrandID;

						CurrentProduct.PurchaseNote = product.PurchaseNote;
						CurrentProduct.EnableReviews = product.EnableReviews;
						CurrentProduct.IsSoldIndividually = product.IsSoldIndividually;

						CurrentProduct.IsManageStock = product.IsManageStock;
						CurrentProduct.IsPublished = product.IsPublished;
						CurrentProduct.IsRecommended = product.IsRecommended;
						CurrentProduct.IsFeatured = product.IsFeatured;
						CurrentProduct.Status = product.Status;

						CurrentProduct.PublishStartDate = product.PublishStartDate;
						CurrentProduct.PublishEndDate = product.PublishEndDate;

						product = null;

						_productRepository.Update(CurrentProduct);
					}
					else
					{
						_productRepository.Update(product);
					}
					if (SaveProduct())
					{
						product = product == null ? CurrentProduct : product;
						message = "Product updated successfully ...";
						return true;
					}
					else
					{
						message = "Oops! Something went wrong. Please try later.";
						return false;
					}
				}
				else
				{
					message = "Product not found  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool DeleteProduct(long id, ref string message, bool softDelete = true)
		{
			try
			{
				Product product = _productRepository.GetById(id);
				if (softDelete)
				{
					product.IsDeleted = true;
					_productRepository.Update(product);
				}
				else
				{
					_productRepository.Delete(product);
				}
				if (SaveProduct())
				{
					message = "Product deleted successfully ...";
					return true;
				}
				else
				{
					message = "Oops! Something went wrong. Please try later.";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool SaveProduct()
		{
			try
			{
				_unitOfWork.Commit();
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public bool PostExcelData(long VendorID
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
			, int StockStatus
			, bool IsFeatured
			, bool IsRecommended
			, bool IsManageStock
			, string ProductCategories
			, string ProductTags)
		{
			try
			{
				DateTime CreatedOn = Helpers.TimeZone.GetLocalDateTime();
				bool IsDeleted = false;
				string Status = "1";
				int ApprovalStatus = 1;
				bool isApproved = false;
				bool IsActive = true;

				if (_productRepository.InsertProduct(VendorID
													, Brand
													, SKU
													, Slug
													, Name
													, thumbnail
													, ShortDescription
													, LongDescription
													, MobileDescription
													, RegularPrice
													, SalePrice
													, SalePriceFrom
													, SalePriceTo
													, Stock
													, Threshold
													, Type
													, Weight
													, Length
													, Width
													, Height
													, IsSoldIndividually
													, AllowMultipleOrder
													, Status
													, StockStatus
													, IsFeatured
													, IsRecommended
													, IsManageStock
													, CreatedOn
													, IsActive
													, isApproved
													, ApprovalStatus
													, IsDeleted
													, ProductCategories
													, ProductTags))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public List<SP_CheckProductStockAvailability_Result> CheckProductStockAvailability(string productIds, string productVariationIds)
		{
			var ProductStock = _productRepository.CheckProductStockAvailability(productIds, productVariationIds);
			return ProductStock;
		}

		public int CheckProductStock(long productId, int quantity)
		{
			var ProductStock = _productRepository.CheckProductStock(productId, quantity);
			return ProductStock;
		}

		//public IEnumerable<SP_GetDetailProducts_Result> GetDetailProducts(string ImageServer, long? vendorId)
		//{
		//	var Product = _productRepository.SP_GetDetailProducts(ImageServer, vendorId);
		//	return Product;
		//}

		//public IEnumerable<SP_GetDetailProductsVariations_Result> GetDetailProductsVariations(string ImageServer, long? vendorId)
		//{
		//	var Product = _productRepository.SP_GetDetailProductsVariations(ImageServer, vendorId);
		//	return Product;
		//}

		public IEnumerable<Product> GetProductsUnApproved(long VendorID, bool isApproved)
		{
			var Product = _productRepository.GetProductsUnApprovedList(VendorID, isApproved);
			return Product;
		}

		public IEnumerable<Product> GetVendorProductsByVendorID(long vendorId)
		{
			var products = _productRepository.GetProductsByVendorID(vendorId);
			return products;
		}

		public bool UpdateProductApprovalStatus(long productID)
		{
			try
			{
				_productRepository.UpdateProductApprovalStatus(productID);
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		#endregion
	}

	public interface IProductService
	{
		Product GetProductbySKU(string SKU, long vendorId);
		Product GetProductbySlug(string slug);
		IEnumerable<Product> GetProducts();
		IEnumerable<SP_GetProducts_Result> GetVendorProducts(int displayLength, int displayStart, int sortCol, string sortDir, string search, long? vendorId, bool IsApproved = true);
		IEnumerable<SP_GetFilteredProducts_Result> GetFilteredProducts(string search, string categories, Nullable<long> vendorID, Nullable<long> brandID, Nullable<decimal> minPrice, Nullable<decimal> maxPrice, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string filters, string lang, string imageServer, bool? isFeatured, bool? isRecommended, bool? isSale, bool? isTrending);
		//IEnumerable<SP_GetPublishedProducts_Result> GetPublishedProducts(string search, Nullable<long> categoryID, Nullable<long> vendorID, Nullable<long> brandID, Nullable<decimal> minPrice, Nullable<decimal> maxPrice, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string filters, string lang, string imageServer, bool? isFeatured, bool? IsRecommended, bool? IsSale);
		IEnumerable<SP_GetCategorywiseProducts_Result> GetCategorywsieProducts(string search, Nullable<long> categoryID, string lang, string imageServer);
		IEnumerable<SP_GetRelatedProducts_Result> GetRelatedProducts(Nullable<long> productId, string lang, string imageServer);
		SP_GetProductDetails_Result GetProductDetails(long productId, string lang, string imageServer);
		IEnumerable<object> GetProductsForDropDown();
		Product GetProduct(long id);
		bool CreateProduct(Product product, ref string message);
		bool UpdateProduct(ref Product product, ref string message, bool Updater = true);
		bool DeleteProduct(long id, ref string message, bool softDelete = true);
		bool SaveProduct();
		IEnumerable<SP_GetProducts_Result> GetVendorProductsUnapproved(int displayLength, int displayStart, int sortCol, string sortDir, string search, long vendorId, bool IsApproved = false);
		List<SP_CheckProductStockAvailability_Result> CheckProductStockAvailability(string productIds, string productVariationIds);
		int CheckProductStock(long productId, int quantity);

		bool PostExcelData(long VendorID
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
		   , int StockStatus
		   , bool IsFeatured
		   , bool IsRecommended
		   , bool IsManageStock
		   , string ProductCategories
		   , string ProductTags);

		//IEnumerable<SP_GetDetailProducts_Result> GetDetailProducts(string ImageServer, long? vendorId);
		//IEnumerable<SP_GetDetailProductsVariations_Result> GetDetailProductsVariations(string ImageServer, long? vendorId);
		IEnumerable<Product> GetProductsUnApproved(long VendorID, bool isApproved);

		IEnumerable<Product> GetVendorProductsByVendorID(long vendorId);
		bool UpdateProductApprovalStatus(long productID);
	}
}
