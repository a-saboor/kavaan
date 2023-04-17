using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;

namespace MyProject.Service
{
	public class ProductTagService : IProductTagService
	{
		private readonly IProductTagRepository _productTagRepository;
		private readonly IUnitOfWork _unitOfWork;

		public ProductTagService(IProductTagRepository productTagRepository, IUnitOfWork unitOfWork)
		{
			this._productTagRepository = productTagRepository;
			this._unitOfWork = unitOfWork;
		}

		#region IProductTagService Members

		public IEnumerable<ProductTag> GetProductTags()
		{
			var productCategories = _productTagRepository.GetAll();
			return productCategories;
		}

		public IEnumerable<ProductTag> GetProductTags(long productId)
		{
			var productCategories = _productTagRepository.GetProductTags(productId);
			return productCategories;
		}

		public ProductTag GetProductTag(long id)
		{
			var productTag = _productTagRepository.GetById(id);
			return productTag;
		}

		public bool CreateProductTag(ref ProductTag productTag, ref string message)
		{
			try
			{
				if (_productTagRepository.GetProductTag((long)productTag.ProductID, (long)productTag.TagID) == null)
				{
					_productTagRepository.Add(productTag);
					if (SaveProductTag())
					{

						message = "Product tag added successfully ...";
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
					message = "Product tag already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool UpdateProductTag(ref ProductTag productTag, ref string message)
		{
			try
			{
				if (_productTagRepository.GetProductTag((long)productTag.ProductID, (long)productTag.TagID, productTag.ID) == null)
				{
					_productTagRepository.Update(productTag);
					if (SaveProductTag())
					{
						message = "Product tag updated successfully ...";
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
					message = "Product tag already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool DeleteProductTag(long id, ref string message)
		{
			try
			{
				ProductTag productTag = _productTagRepository.GetById(id);
				_productTagRepository.Delete(productTag);
				if (SaveProductTag())
				{
					message = "Product tag deleted successfully ...";
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

		public bool DeleteProductTag(ProductTag productTag, ref string message)
		{
			try
			{
				ProductTag currentProductTag = _productTagRepository.GetProductTag((long)productTag.ProductID, (long)productTag.TagID);
				productTag = null;
				_productTagRepository.Delete(currentProductTag);
				if (SaveProductTag())
				{
					message = "Product tag deleted successfully ...";
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

		public bool SaveProductTag()
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
        public bool PostExcelData(long ProductID,string ProductTags)
        {
            try
            {
                DateTime CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                _productTagRepository.InsertProductTags(ProductID, ProductTags);
                //SaveProductTag();
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

	public interface IProductTagService
	{
        bool PostExcelData(long ProductID, string ProductTags);

        IEnumerable<ProductTag> GetProductTags();
		IEnumerable<ProductTag> GetProductTags(long productId);
		ProductTag GetProductTag(long id);
		bool CreateProductTag(ref ProductTag productTag, ref string message);
		bool UpdateProductTag(ref ProductTag productTag, ref string message);
		bool DeleteProductTag(long id, ref string message);
		bool DeleteProductTag(ProductTag productTag, ref string message);
		bool SaveProductTag();
	}
}
