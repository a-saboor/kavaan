using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;

namespace MyProject.Service
{

	public class ProductAttributeSettingService : IProductAttributeSettingService
	{
		private readonly IProductAttributeSettingRepository _productAttributeSettingRepository;
		private readonly IUnitOfWork _unitOfWork;

		public ProductAttributeSettingService(IProductAttributeSettingRepository productAttributeSettingRepository, IUnitOfWork unitOfWork)
		{
			this._productAttributeSettingRepository = productAttributeSettingRepository;
			this._unitOfWork = unitOfWork;
		}

		#region IProductAttributeSettingService Members

		public IEnumerable<ProductAttributeSetting> GetProductAttributeSettings()
		{
			var productCategories = _productAttributeSettingRepository.GetAll();
			return productCategories;
		}

		public IEnumerable<ProductAttributeSetting> GetProductAttributesSetting(long productId)
		{
			var productAttributeSettings = _productAttributeSettingRepository.GetProductAttributesSetting(productId);
			return productAttributeSettings;
		}

		public ProductAttributeSetting GetProductAttributeSetting(long id)
		{
			var productAttributeSetting = _productAttributeSettingRepository.GetById(id);
			return productAttributeSetting;
		}

		public bool CreateProductAttributeSetting(ref ProductAttributeSetting productAttributeSetting, ref string message)
		{
			try
			{
				if (_productAttributeSettingRepository.GetProductAttributeSetting((long)productAttributeSetting.ProductID, (long)productAttributeSetting.AttributeID) == null)
				{
					_productAttributeSettingRepository.Add(productAttributeSetting);
					if (SaveProductAttributeSetting())
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

		public bool UpdateProductAttributeSetting(ref ProductAttributeSetting productAttributeSetting, ref string message)
		{
			try
			{
				if (_productAttributeSettingRepository.GetProductAttributeSetting((long)productAttributeSetting.ProductID, (long)productAttributeSetting.AttributeID, productAttributeSetting.ID) == null)
				{
					_productAttributeSettingRepository.Update(productAttributeSetting);
					if (SaveProductAttributeSetting())
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

		public bool DeleteProductAttributeSetting(long id, ref string message)
		{
			try
			{
				ProductAttributeSetting productAttributeSetting = _productAttributeSettingRepository.GetById(id);
				_productAttributeSettingRepository.Delete(productAttributeSetting);
				if (SaveProductAttributeSetting())
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

		public bool DeleteProductAttributeSetting(ProductAttributeSetting productAttributeSetting, ref string message)
		{
			try
			{
				ProductAttributeSetting currentProductAttributeSetting = _productAttributeSettingRepository.GetProductAttributeSetting((long)productAttributeSetting.ProductID, (long)productAttributeSetting.AttributeID);
				productAttributeSetting = null;
				_productAttributeSettingRepository.Delete(currentProductAttributeSetting);
				if (SaveProductAttributeSetting())
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

		public bool SaveProductAttributeSetting()
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

		#endregion
	}

	public interface IProductAttributeSettingService
	{
		IEnumerable<ProductAttributeSetting> GetProductAttributeSettings();
		IEnumerable<ProductAttributeSetting> GetProductAttributesSetting(long productId);
		ProductAttributeSetting GetProductAttributeSetting(long id);
		bool CreateProductAttributeSetting(ref ProductAttributeSetting productAttributeSetting, ref string message);
		bool UpdateProductAttributeSetting(ref ProductAttributeSetting productAttributeSetting, ref string message);
		bool DeleteProductAttributeSetting(long id, ref string message);
		bool DeleteProductAttributeSetting(ProductAttributeSetting productAttributeSetting, ref string message);
		bool SaveProductAttributeSetting();
	}
}
