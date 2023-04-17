using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;

namespace MyProject.Service
{

	public class ProductImageService : IProductImageService
	{
		private readonly IProductImageRepository _productImageRepository;
		private readonly IUnitOfWork _unitOfWork;

		public ProductImageService(IProductImageRepository productImageRepository, IUnitOfWork unitOfWork)
		{
			this._productImageRepository = productImageRepository;
			this._unitOfWork = unitOfWork;
		}

		#region IProductImageService Members

		public IEnumerable<ProductImage> GetProductImages()
		{
			var productImages = _productImageRepository.GetAll();
			return productImages;
		}

		public IEnumerable<ProductImage> GetProductImages(long productId)
		{
			var productImages = _productImageRepository.GetProductImages(productId);
			return productImages;
		}

		public ProductImage GetProductImage(long id)
		{
			var productImage = _productImageRepository.GetById(id);
			return productImage;
		}

		public bool CreateProductImage(ref ProductImage productImage, ref string message)
		{
			try
			{
				_productImageRepository.Add(productImage);
				if (SaveProductImage())
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

		public bool UpdateProductImage(ref ProductImage productImage, ref string message)
		{
			try
			{
				_productImageRepository.Update(productImage);
				if (SaveProductImage())
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

		public bool DeleteProductImage(long id, ref string message,ref string filepath)
		{
			try
			{
				ProductImage productImage = _productImageRepository.GetById(id);
				filepath = productImage.Image;
				_productImageRepository.Delete(productImage);
				if (SaveProductImage())
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

		public bool SaveProductImage()
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

	public interface IProductImageService
	{
		IEnumerable<ProductImage> GetProductImages();
		IEnumerable<ProductImage> GetProductImages(long productId);
		ProductImage GetProductImage(long id);
		bool CreateProductImage(ref ProductImage productImage, ref string message);
		bool UpdateProductImage(ref ProductImage productImage, ref string message);
		bool DeleteProductImage(long id, ref string message, ref string filepath);
		bool SaveProductImage();
	}
}
