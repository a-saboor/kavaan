using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
	public class ProductVariationAttributeService : IProductVariationAttributeService
	{
		private readonly IProductVariationAttributeRepository _productVariationAttributeRepository;
		private readonly IUnitOfWork _unitOfWork;

		public ProductVariationAttributeService(IProductVariationAttributeRepository productVariationAttributeRepository, IUnitOfWork unitOfWork)
		{
			this._productVariationAttributeRepository = productVariationAttributeRepository;
			this._unitOfWork = unitOfWork;
		}

		#region IProductVariationAttributeService Members

		public IEnumerable<ProductVariationAttribute> GetProductVariationAttributes()
		{
			var productCategories = _productVariationAttributeRepository.GetAll().Where(i => i.IsDeleted == false).ToList();
			return productCategories;
		}

		public IEnumerable<ProductVariationAttribute> GetVariationAttributesByProduct(long productId)
		{
			var productCategories = _productVariationAttributeRepository.GetVariationAttributesByProduct(productId);
			return productCategories;
		}

		public IEnumerable<ProductVariationAttribute> GetProductVariationAttributes(long productVariatId)
		{
			var productCategories = _productVariationAttributeRepository.GetProductVariationAttributes(productVariatId);
			return productCategories;
		}

		public ProductVariationAttribute GetProductVariationAttribute(long productVariationId, long productattributeId)
		{
			var productCategories = _productVariationAttributeRepository.GetProductVariationAttribute(productVariationId, productattributeId);
			return productCategories;
		}

		public ProductVariationAttribute GetProductVariationAttribute(long id)
		{
			var productVariationAttribute = _productVariationAttributeRepository.GetById(id);
			return productVariationAttribute;
		}

		public bool CreateProductVariationAttribute(ref ProductVariationAttribute productVariationAttribute, ref string message)
		{
			try
			{
				if (_productVariationAttributeRepository.GetProductVariationAttribute((long)productVariationAttribute.ProductVariationID, (long)productVariationAttribute.ProductAttributeID) == null)
				{
					productVariationAttribute.IsDeleted = false;
					productVariationAttribute.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
					_productVariationAttributeRepository.Add(productVariationAttribute);
					if (SaveProductVariationAttribute())
					{

						message = "Product attribute added successfully ...";
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
					message = "Product attribute already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool UpdateProductVariationAttribute(ref ProductVariationAttribute productVariationAttribute, ref string message)
		{
			try
			{
				if (_productVariationAttributeRepository.GetProductVariationAttribute((long)productVariationAttribute.ProductVariationID, (long)productVariationAttribute.ProductAttributeID, productVariationAttribute.ID) == null)
				{
					_productVariationAttributeRepository.Update(productVariationAttribute);
					if (SaveProductVariationAttribute())
					{
						message = "Product attribute updated successfully ...";
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
					message = "Product attribute already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool DeleteProductVariationAttribute(long id, ref string message)
		{
			try
			{
				ProductVariationAttribute productVariationAttribute = _productVariationAttributeRepository.GetById(id);
				_productVariationAttributeRepository.Delete(productVariationAttribute);
				if (SaveProductVariationAttribute())
				{
					message = "Product attribute deleted successfully ...";
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

		public bool DeleteProductVariationAttribute(ProductVariationAttribute productVariationAttribute, ref string message)
		{
			try
			{
				_productVariationAttributeRepository.Delete(productVariationAttribute);
				if (SaveProductVariationAttribute())
				{
					message = "Product attribute deleted successfully ...";
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

		public bool DeleteProductVariationAttributes(long productVariationId, ref string message)
		{
			try
			{
				_productVariationAttributeRepository.DeleteMany(productVariationId);
				if (SaveProductVariationAttribute())
				{
					message = "Product attribute deleted successfully ...";
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

		public bool SaveProductVariationAttribute()
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
		public bool PostExcelData(long ProductID, string AttributeName, string AttributeValue, long ProductVariationID)
		{
			try
			{
				_productVariationAttributeRepository.InsertProductVariantAttributes(ProductID, AttributeName, AttributeValue, ProductVariationID);
				//SaveProductVariationAttribute();
				return true;
			}
			catch (Exception ex)
			{
				//log.Error("Error", ex);
				//log.Error("Error", ex);
				return false;
			}
		}

		#endregion
	}

	public interface IProductVariationAttributeService
	{
		bool PostExcelData(long ProductID, string AttributeName, string AttributeValue, long ProductVariationID);
		IEnumerable<ProductVariationAttribute> GetProductVariationAttributes();
		IEnumerable<ProductVariationAttribute> GetVariationAttributesByProduct(long productId);
		IEnumerable<ProductVariationAttribute> GetProductVariationAttributes(long productVariatId);
		ProductVariationAttribute GetProductVariationAttribute(long id);
		ProductVariationAttribute GetProductVariationAttribute(long productVariationId, long productattributeId);
		bool CreateProductVariationAttribute(ref ProductVariationAttribute productVariationAttribute, ref string message);
		bool UpdateProductVariationAttribute(ref ProductVariationAttribute productVariationAttribute, ref string message);
		bool DeleteProductVariationAttribute(long id, ref string message);
		bool DeleteProductVariationAttribute(ProductVariationAttribute productVariationAttribute, ref string message);
		bool DeleteProductVariationAttributes(long productVariationId, ref string message);
		bool SaveProductVariationAttribute();
	}
}
