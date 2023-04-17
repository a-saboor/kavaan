using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;

namespace MyProject.Service
{
	public class ProductReturnImageService : IProductReturnImageService
	{
		private readonly IProductReturnImageRepository _productReturnImageRepository;
		private readonly IUnitOfWork _unitOfWork;

		public ProductReturnImageService(IProductReturnImageRepository productReturnImageRepository, IUnitOfWork unitOfWork)
		{
			this._productReturnImageRepository = productReturnImageRepository;
			this._unitOfWork = unitOfWork;
		}

		#region IProductReturnImageService Members

		public IEnumerable<ProductReturnImage> GetProductReturnImages()
		{
			var productReturnImages = _productReturnImageRepository.GetAll();
			return productReturnImages;
		}

		public IEnumerable<ProductReturnImage> GetProductReturnImages(long productReturnId)
		{
			var productReturnImages = _productReturnImageRepository.GetProductReturnImages(productReturnId);
			return productReturnImages;
		}

		public ProductReturnImage GetProductReturnImage(long id)
		{
			var productReturnImage = _productReturnImageRepository.GetById(id);
			return productReturnImage;
		}

		public bool CreateProductReturnImage(ref ProductReturnImage productReturnImage, ref string message)
		{
			try
			{
				_productReturnImageRepository.Add(productReturnImage);
				if (SaveProductReturnImage())
				{

					message = "Product return image added successfully ...";
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

		public bool UpdateProductReturnImage(ref ProductReturnImage productReturnImage, ref string message)
		{
			try
			{
				_productReturnImageRepository.Update(productReturnImage);
				if (SaveProductReturnImage())
				{
					message = "Product return image updated successfully ...";
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

		public bool DeleteProductReturnImage(long id, ref string message, ref string filepath)
		{
			try
			{
				ProductReturnImage productReturnImage = _productReturnImageRepository.GetById(id);
				filepath = productReturnImage.Image;
				_productReturnImageRepository.Delete(productReturnImage);
				if (SaveProductReturnImage())
				{
					message = "Product return image deleted successfully ...";
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

		public bool SaveProductReturnImage()
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

	public interface IProductReturnImageService
	{
		IEnumerable<ProductReturnImage> GetProductReturnImages();
		IEnumerable<ProductReturnImage> GetProductReturnImages(long productReturnId);
		ProductReturnImage GetProductReturnImage(long id);
		bool CreateProductReturnImage(ref ProductReturnImage productReturnImage, ref string message);
		bool UpdateProductReturnImage(ref ProductReturnImage productReturnImage, ref string message);
		bool DeleteProductReturnImage(long id, ref string message, ref string filepath);
		bool SaveProductReturnImage();
	}
}
