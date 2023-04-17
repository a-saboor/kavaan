using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;

namespace MyProject.Service
{
	public class ProductCategoryService : IProductCategoryService
	{
		private readonly IProductCategoryRepository _productCategoryRepository;
		private readonly IUnitOfWork _unitOfWork;

		public ProductCategoryService(IProductCategoryRepository productCategoryRepository, IUnitOfWork unitOfWork)
		{
			this._productCategoryRepository = productCategoryRepository;
			this._unitOfWork = unitOfWork;
		}

		#region IProductCategoryService Members

		public IEnumerable<ProductCategory> GetProductCategories()
		{
			var productCategories = _productCategoryRepository.GetAll();
			return productCategories;
		}

		public IEnumerable<ProductCategory> GetProductCategories(long productId)
		{
			var productCategories = _productCategoryRepository.GetProductCategories(productId);
			return productCategories;
		}

		public ProductCategory GetProductCategory(long id)
		{
			var productCategory = _productCategoryRepository.GetById(id);
			return productCategory;
		}

		public bool CreateProductCategory(ref ProductCategory productCategory, ref string message)
		{
			try
			{
				if (_productCategoryRepository.GetProductCategory((long)productCategory.ProductID, (long)productCategory.ProductCategoryID) == null)
				{
					_productCategoryRepository.Add(productCategory);
					if (SaveProductCategory())
					{

						message = "Product Category added successfully ...";
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
					message = "ProductCategory already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool UpdateProductCategory(ref ProductCategory productCategory, ref string message)
		{
			try
			{
				if (_productCategoryRepository.GetProductCategory((long)productCategory.ProductID, (long)productCategory.ProductCategoryID, productCategory.ID) == null)
				{
					_productCategoryRepository.Update(productCategory);
					if (SaveProductCategory())
					{
						message = "ProductCategory updated successfully ...";
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
					message = "ProductCategory already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool DeleteProductCategory(long id, ref string message)
		{
			try
			{
				ProductCategory productCategory = _productCategoryRepository.GetById(id);
				_productCategoryRepository.Delete(productCategory);
				if (SaveProductCategory())
				{
					message = "Product Category deleted successfully ...";
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

		public bool SaveProductCategory()
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
		public bool PostExcelData(long ProductID, string ProductCategories)
		{
			try
			{
				_productCategoryRepository.InsertProductCategories(ProductID, ProductCategories);
				//SaveProductCategory();
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

	public interface IProductCategoryService
	{
		bool PostExcelData(long ProductID, string ProductCategories);

		IEnumerable<ProductCategory> GetProductCategories();
		IEnumerable<ProductCategory> GetProductCategories(long productId);
		ProductCategory GetProductCategory(long id);
		bool CreateProductCategory(ref ProductCategory productCategory, ref string message);
		bool UpdateProductCategory(ref ProductCategory productCategory, ref string message);
		bool DeleteProductCategory(long id, ref string message);
		bool SaveProductCategory();
	}
}
