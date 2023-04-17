using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
	public class ProductVariationService : IProductVariationService
	{
		private readonly IProductVariationRepository _productVariationRepository;
		private readonly IUnitOfWork _unitOfWork;

		public ProductVariationService(IProductVariationRepository productVariationRepository, IUnitOfWork unitOfWork)
		{
			this._productVariationRepository = productVariationRepository;
			this._unitOfWork = unitOfWork;
		}

		#region IProductVariationService Members

		public IEnumerable<ProductVariation> GetProductVariations()
		{
			var productCategories = _productVariationRepository.GetAll().Where(i => i.IsDeleted == false).ToList();
			return productCategories;
		}

		public IEnumerable<ProductVariation> GetProductVariations(long productId)
		{
			var productCategories = _productVariationRepository.GetProductVariations(productId);
			return productCategories;
		}

		public ProductVariation GetProductVariation(long id)
		{
			var productVariation = _productVariationRepository.GetById(id);
			return productVariation;
		}

		public ProductVariation GetProductVariation(string sku)
		{
			var productVariation = _productVariationRepository.GetProductSKU(sku);
			return productVariation;
		}

		public bool CreateProductVariation(ref ProductVariation productVariation, ref string message)
		{
			try
			{
				if (_productVariationRepository.GetProductVariation((long)productVariation.ProductID, productVariation.SKU) == null)
				{
					productVariation.IsActive = productVariation.IsActive.HasValue ? productVariation.IsActive.Value : false;
					productVariation.IsManageStock = productVariation.IsManageStock.HasValue ? productVariation.IsManageStock.Value : false;
					productVariation.IsDeleted = productVariation.IsDeleted.HasValue ? productVariation.IsDeleted.Value : false;
					productVariation.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
					_productVariationRepository.Add(productVariation);
					if (SaveProductVariation())
					{
						message = "Product variation added successfully ...";
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
					message = "Variation with Same SKU already exist ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool UpdateProductVariation(ref ProductVariation productVariation, ref string message, bool updater = true)
		{
			try
			{
				if (_productVariationRepository.GetProductVariation((long)productVariation.ProductID, productVariation.SKU, productVariation.ID) == null)
				{
					if (updater)
					{
						var CurrentProductVariation = _productVariationRepository.GetById(productVariation.ID);

						CurrentProductVariation.ID = productVariation.ID;
						CurrentProductVariation.Thumbnail = productVariation.Thumbnail;
						CurrentProductVariation.SKU = productVariation.SKU;
						CurrentProductVariation.ProductID = productVariation.ProductID;
						CurrentProductVariation.RegularPrice = productVariation.RegularPrice;
						CurrentProductVariation.SalePrice = productVariation.SalePrice;
						CurrentProductVariation.SalePriceFrom = productVariation.SalePriceFrom;
						CurrentProductVariation.SalePriceTo = productVariation.SalePriceTo;
						CurrentProductVariation.Stock = productVariation.Stock;
						CurrentProductVariation.Threshold = productVariation.Threshold;
						CurrentProductVariation.StockStatus = productVariation.StockStatus;
						CurrentProductVariation.Weight = productVariation.Weight;
						CurrentProductVariation.Length = productVariation.Length;
						CurrentProductVariation.Width = productVariation.Width;
						CurrentProductVariation.Height = productVariation.Height;
						CurrentProductVariation.Description = productVariation.Description;						
						CurrentProductVariation.IsManageStock = productVariation.IsManageStock;
						CurrentProductVariation.SoldIndividually = productVariation.SoldIndividually;
						CurrentProductVariation.IsActive = productVariation.IsActive;
						
						productVariation = null;

						_productVariationRepository.Update(CurrentProductVariation);
						if (SaveProductVariation())
						{
							productVariation = CurrentProductVariation;
							message = "Product variation updated successfully ...";
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
						_productVariationRepository.Update(productVariation);
						if (SaveProductVariation())
						{
							message = "Product variation updated successfully ...";
							return true;
						}
						else
						{
							message = "Oops! Something went wrong. Please try later.";
							return false;
						}
					}
				}
				else
				{
					message = "Variation with Same SKU already exist ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool DeleteProductVariation(long id, ref string message, bool isSoft = true)
		{
			try
			{
				if (isSoft)
				{
					ProductVariation productVariation = _productVariationRepository.GetById(id);
					productVariation.IsDeleted = true;
					_productVariationRepository.Update(productVariation);
					if (SaveProductVariation())
					{
						message = "Product variation deleted successfully ...";
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
					ProductVariation productVariation = _productVariationRepository.GetById(id);
					_productVariationRepository.Delete(productVariation);
					if (SaveProductVariation())
					{
						message = "Product variation deleted successfully ...";
						return true;
					}
					else
					{
						message = "Oops! Something went wrong. Please try later.";
						return false;
					}
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool DeleteProductVariation(ProductVariation productVariation, ref string message)
		{
			try
			{
				ProductVariation currentProductVariation = _productVariationRepository.GetProductVariation((long)productVariation.ProductID, productVariation.SKU);
				productVariation = null;
				_productVariationRepository.Delete(currentProductVariation);
				if (SaveProductVariation())
				{
					message = "Product variation deleted successfully ...";
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

		public bool DeleteProductVariations(long id, ref string message)
		{
			try
			{
				_productVariationRepository.DeleteMany(id);
				if (SaveProductVariation())
				{
					message = "Product variation deleted successfully ...";
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

		public bool SaveProductVariation()
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

		public IEnumerable<SP_GetProductVaraitions_Result> GetProductVaraitions(long productId, string lang, string imageServer)
		{
			var currentDateTime = Helpers.TimeZone.GetLocalDateTime();
			var ProductVariations = _productVariationRepository.GetProductVaraitions(productId, lang, imageServer).ToList();
			return ProductVariations;
		}

		public ProductVariation GetProductbySKU(string SKU)
		{
			var products = _productVariationRepository.GetProductSKU(SKU);
			return products;
		}

		public bool PostExcelData(long ProductID, decimal RegularPrice, decimal? SalePrice, DateTime? SalePriceFrom, DateTime? SalePriceTo, int Stock, int Threshold, int StockStatus, string SKU, string thumbnail, float Weight, float Length, float Width, float Height, string Description, bool IsSoldIndividually, bool IsManageStock)
		{
			try
			{
				DateTime CreatedOn = Helpers.TimeZone.GetLocalDateTime();
				bool IsActive = true;

				_productVariationRepository.InsertVariationProduct(IsActive, CreatedOn, ProductID, RegularPrice, SalePrice, SalePriceFrom, SalePriceTo, Stock, Threshold, StockStatus, SKU, thumbnail, Weight, Length, Width, Height, Description, IsSoldIndividually, IsManageStock);
				//SaveProductVariation();
				return true;
			}
			catch (Exception ex)
			{
				//log.Error("Error", ex);
				//log.Error("Error", ex);
				return false;
			}
		}

		public int CheckProductVariationStock(long productId, int quantity)
		{
			var ProductVariationStock = _productVariationRepository.CheckProductVariationStock(productId, quantity);
			return ProductVariationStock;
		}

		#endregion
	}

	public interface IProductVariationService
	{
		ProductVariation GetProductbySKU(string SKU);
		bool PostExcelData(long ProductID, decimal RegularPrice, decimal? SalePrice, DateTime? SalePriceFrom, DateTime? SalePriceTo, int Stock, int Threshold, int StockStatus, string SKU, string thumbnail, float Weight, float Length, float Width, float Height, string Description, bool IsSoldIndividually, bool IsManageStock);
		IEnumerable<ProductVariation> GetProductVariations();
		IEnumerable<ProductVariation> GetProductVariations(long productId);
		ProductVariation GetProductVariation(long id);
		ProductVariation GetProductVariation(string sku);
		bool CreateProductVariation(ref ProductVariation productVariation, ref string message);
		bool UpdateProductVariation(ref ProductVariation productVariation, ref string message, bool updater = true);
		bool DeleteProductVariation(long id, ref string message, bool isSoft = true);
		bool DeleteProductVariation(ProductVariation productVariation, ref string message);
		bool DeleteProductVariations(long id, ref string message);
		bool SaveProductVariation();

		IEnumerable<SP_GetProductVaraitions_Result> GetProductVaraitions(long productId, string lang, string imageServer);
		int CheckProductVariationStock(long productId, int quantity);
	}
}
