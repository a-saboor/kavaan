using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;

namespace MyProject.Service
{

	public class ProductVariationImageService : IProductVariationImageService
	{
		private readonly IProductVariationImageRepository _productVariationImageRepository;
		private readonly IUnitOfWork _unitOfWork;

		public ProductVariationImageService(IProductVariationImageRepository productVariationImageRepository, IUnitOfWork unitOfWork)
		{
			this._productVariationImageRepository = productVariationImageRepository;
			this._unitOfWork = unitOfWork;
		}

		#region IProductVariationImageService Members

		public IEnumerable<ProductVariationImage> GetProductVariationImages()
		{
			var productVariationImages = _productVariationImageRepository.GetAll();
			return productVariationImages;
		}

		public IEnumerable<ProductVariationImage> GetProductVariationImages(long productId)
		{
			var productVariationImages = _productVariationImageRepository.GetProductVariationImages(productId);
			return productVariationImages;
		}

		public ProductVariationImage GetProductVariationImage(long id)
		{
			var productVariationImage = _productVariationImageRepository.GetById(id);
			return productVariationImage;
		}

		public bool CreateProductVariationImage(ref ProductVariationImage productVariationImage, ref string message)
		{
			try
			{
				_productVariationImageRepository.Add(productVariationImage);
				if (SaveProductVariationImage())
				{

					message = "Product gallery image added successfully ...";
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

		public bool UpdateProductVariationImage(ref ProductVariationImage productVariationImage, ref string message)
		{
			try
			{
				_productVariationImageRepository.Update(productVariationImage);
				if (SaveProductVariationImage())
				{
					message = "Product gallery image updated successfully ...";
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

		public bool DeleteProductVariationImage(long id, ref string message, ref string filepath)
		{
			try
			{
				ProductVariationImage productVariationImage = _productVariationImageRepository.GetById(id);
				filepath = productVariationImage.Image;
				_productVariationImageRepository.Delete(productVariationImage);
				if (SaveProductVariationImage())
				{
					message = "Product gallery image deleted successfully ...";
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

		public bool SaveProductVariationImage()
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

	public interface IProductVariationImageService
	{
		IEnumerable<ProductVariationImage> GetProductVariationImages();
		IEnumerable<ProductVariationImage> GetProductVariationImages(long productId);
		ProductVariationImage GetProductVariationImage(long id);
		bool CreateProductVariationImage(ref ProductVariationImage productVariationImage, ref string message);
		bool UpdateProductVariationImage(ref ProductVariationImage productVariationImage, ref string message);
		bool DeleteProductVariationImage(long id, ref string message, ref string filepath);
		bool SaveProductVariationImage();
	}
}
