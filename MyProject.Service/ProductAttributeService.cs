using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
	public class ProductAttributeService : IProductAttributeService
	{
		private readonly IProductAttributeRepository _productAttributeRepository;
		private readonly IUnitOfWork _unitOfWork;

		public ProductAttributeService(IProductAttributeRepository productAttributeRepository, IUnitOfWork unitOfWork)
		{
			this._productAttributeRepository = productAttributeRepository;
			this._unitOfWork = unitOfWork;
		}

		#region IProductAttributeService Members

		public IEnumerable<ProductAttribute> GetProductAttributes()
		{
			var productCategories = _productAttributeRepository.GetAll().Where(c => c.IsDeleted == false).ToList();
			return productCategories;
		}

		public IEnumerable<ProductAttribute> GetProductAttributes(long productId)
		{
			var productCategories = _productAttributeRepository.GetProductAttributes(productId);
			return productCategories;
		}

		public ProductAttribute GetProductAttribute(long id)
		{
			var productAttribute = _productAttributeRepository.GetAll().Where(c => c.ID == id && c.IsDeleted == false).SingleOrDefault();
			return productAttribute;
		}

		public ProductAttribute GetProductAttribute(long productId, long attributeId, string value)
		{
			var productAttribute = _productAttributeRepository.GetProductAttribute(productId, attributeId, value);
			return productAttribute;
		}

		public ProductAttribute GetProductAttribute(long productId, string attribute, string value)
		{
			var productAttribute = _productAttributeRepository.GetProductAttribute(productId, attribute, value);
			return productAttribute;
		}

		public bool CreateProductAttribute(ref ProductAttribute productAttribute, ref string message, ref bool isAlreadyExist)
		{
			try
			{
				isAlreadyExist = false;
				if (_productAttributeRepository.GetProductAttribute((long)productAttribute.ProductID, (long)productAttribute.AttributeID, productAttribute.Value) == null)
				{
					productAttribute.IsDeleted = false;
					_productAttributeRepository.Add(productAttribute);
					if (SaveProductAttribute())
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
					isAlreadyExist = true;
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

		public bool UpdateProductAttribute(ref ProductAttribute productAttribute, ref string message)
		{
			try
			{
				if (_productAttributeRepository.GetProductAttribute((long)productAttribute.ProductID, (long)productAttribute.AttributeID, productAttribute.Value, productAttribute.ID) == null)
				{
					_productAttributeRepository.Update(productAttribute);
					if (SaveProductAttribute())
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

		public bool DeleteProductAttribute(long id, ref string message, bool softDelete = true)
		{
			try
			{
				ProductAttribute productAttribute = _productAttributeRepository.GetById(id);
				if (softDelete)
				{
					productAttribute.IsDeleted = true;
					_productAttributeRepository.Update(productAttribute);
				}
				else
				{
					_productAttributeRepository.Delete(productAttribute);
				}
				if (SaveProductAttribute())
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

		public bool DeleteProductAttribute(ProductAttribute productAttribute, ref string message, ref long id, bool softDelete = true)
		{
			try
			{
				ProductAttribute currentProductAttribute = _productAttributeRepository.GetProductAttribute((long)productAttribute.ProductID, (long)productAttribute.AttributeID, productAttribute.Value);
				productAttribute = null;

				if (softDelete)
				{
					id = currentProductAttribute.ID;
					currentProductAttribute.IsDeleted = true;
					_productAttributeRepository.Update(currentProductAttribute);
				}
				else
				{
					_productAttributeRepository.Delete(currentProductAttribute);
				}

				if (SaveProductAttribute())
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

		public bool DeleteProductAttributes(ProductAttribute productAttribute, ref string message)
		{
			try
			{
				_productAttributeRepository.DeleteMany(productAttribute);
				if (SaveProductAttribute())
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

		public bool SaveProductAttribute()
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

		public int PostExcelData(long ProductID, string AttributeName, string AttributeValue, bool variationUsage)
		{
			try
			{
				DateTime CreatedOn = Helpers.TimeZone.GetLocalDateTime();


				var result = _productAttributeRepository.InsertProductAttributes(ProductID, AttributeName, AttributeValue, CreatedOn, variationUsage);
				//SaveProductAttribute();
				return result;
			}
			catch (Exception ex)
			{
				//log.Error("Error", ex);
				//log.Error("Error", ex);
				return -1;
			}
		}

		public IEnumerable<SP_GetProductAttributes_Result> GetProductAttributes(long productId, string lang)
		{
			var ProductAttributes = _productAttributeRepository.GetProductAttributes(productId, lang);
			return ProductAttributes;
		}

		public bool UpdateProductAttributesVariationUsage()
		{
			try
			{
				_productAttributeRepository.UpdateProductAttributesVariationUsage();
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}
		#endregion
	}

	public interface IProductAttributeService
	{
		int PostExcelData(long ProductID, string AttributeName, string AttributeValue, bool variationUsage);
		IEnumerable<ProductAttribute> GetProductAttributes();
		IEnumerable<ProductAttribute> GetProductAttributes(long productId);
		ProductAttribute GetProductAttribute(long id);
		ProductAttribute GetProductAttribute(long productId, long attributeId, string value);
		ProductAttribute GetProductAttribute(long productId, string attribute, string value);
		bool CreateProductAttribute(ref ProductAttribute productAttribute, ref string message, ref bool isAlreadyExist);
		bool UpdateProductAttribute(ref ProductAttribute productAttribute, ref string message);
		bool DeleteProductAttribute(long id, ref string message, bool softDelete = true);
		bool DeleteProductAttribute(ProductAttribute productAttribute, ref string message, ref long id, bool softDelete = true);
		bool DeleteProductAttributes(ProductAttribute productAttribute, ref string message);
		bool SaveProductAttribute();

		IEnumerable<SP_GetProductAttributes_Result> GetProductAttributes(long productId, string lang);
		bool UpdateProductAttributesVariationUsage();
	}
}
