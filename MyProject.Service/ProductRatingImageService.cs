using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RawsomeFoods.Service
{

	public class ProductRatingImageService : IProductRatingImageService
	{
		private readonly IProductRatingImageRepository _productRatingImageRepository;
		private readonly IUnitOfWork _unitOfWork;

		public ProductRatingImageService(IProductRatingImageRepository productRatingImageRepository, IUnitOfWork unitOfWork)
		{
			this._productRatingImageRepository = productRatingImageRepository;
			this._unitOfWork = unitOfWork;
		}

		#region IProductRatingImageService Members

		public IEnumerable<ProductRatingImage> GetProductRatingImages()
		{
			var productRatingImages = _productRatingImageRepository.GetAll();
			return productRatingImages;
		}

		public IEnumerable<ProductRatingImage> GetProductRatingImages(long productRatingId)
		{
			var productRatingImages = _productRatingImageRepository.GetProductRatingImages(productRatingId);
			return productRatingImages;
		}

		public ProductRatingImage GetProductRatingImage(long id)
		{
			var productRatingImage = _productRatingImageRepository.GetById(id);
			return productRatingImage;
		}

		public bool CreateProductRatingImage(ref ProductRatingImage productRatingImage, ref string message)
		{
			try
			{
				_productRatingImageRepository.Add(productRatingImage);
				if (SaveProductRatingImage())
				{

					message = "Product Rating image added successfully ...";
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

		public bool UpdateProductRatingImage(ref ProductRatingImage productRatingImage, ref string message)
		{
			try
			{
				_productRatingImageRepository.Update(productRatingImage);
				if (SaveProductRatingImage())
				{
					message = "Product Rating image updated successfully ...";
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

		public bool DeleteProductRatingImage(long id, ref string message, ref string filepath)
		{
			try
			{
				ProductRatingImage productRatingImage = _productRatingImageRepository.GetById(id);
				filepath = productRatingImage.Image;
				_productRatingImageRepository.Delete(productRatingImage);
				if (SaveProductRatingImage())
				{
					message = "Product Rating image deleted successfully ...";
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

		public bool SaveProductRatingImage()
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

	public interface IProductRatingImageService
	{
		IEnumerable<ProductRatingImage> GetProductRatingImages();
		IEnumerable<ProductRatingImage> GetProductRatingImages(long productRatingId);
		ProductRatingImage GetProductRatingImage(long id);
		bool CreateProductRatingImage(ref ProductRatingImage productRatingImage, ref string message);
		bool UpdateProductRatingImage(ref ProductRatingImage productRatingImage, ref string message);
		bool DeleteProductRatingImage(long id, ref string message, ref string filepath);
		bool SaveProductRatingImage();
	}
}
