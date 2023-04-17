using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;

namespace MyProject.Service
{
	public class ProductRatingService : IProductRatingService
	{
		private readonly IProductRatingRepository _productRatingRepository;
		private readonly IUnitOfWork _unitOfWork;

		public ProductRatingService(IProductRatingRepository productRatingRepository, IUnitOfWork unitOfWork)
		{
			this._productRatingRepository = productRatingRepository;
			this._unitOfWork = unitOfWork;
		}

		#region IProductRatingService Members

		public IEnumerable<ProductRating> GetProductRatings()
		{
			var productRatings = _productRatingRepository.GetAll();
			return productRatings;
		}

		public IEnumerable<SP_GetProductRatings_Result> GetProductRatings(long productId, string ImageServer)
		{
			var productRatings = _productRatingRepository.GetProductRatings(productId, ImageServer);
			return productRatings;
		}

		public IEnumerable<SP_GetVendorRatings_Result> GetVendorRatings(long vendorId, long pageNumber, string ImageServer)
		{
			var vendorRatings = _productRatingRepository.GetVendorRatings(vendorId, pageNumber, ImageServer);
			return vendorRatings;
		}

		public ProductRating GetProductRating(long id)
		{
			var productRating = _productRatingRepository.GetById(id);
			return productRating;
		}

		public ProductRating GetProductRating(long orderDetailId, long ProductId)
		{
			var productRating = _productRatingRepository.GetProductRating(orderDetailId, ProductId);
			return productRating;
		}

		public ProductRating GetOrderDetailRating(long orderDetailId)
		{
			var productRating = _productRatingRepository.GetOrderDetailRating(orderDetailId);
			return productRating;
		}

		public bool CreateProductRating(ref ProductRating productRating, ref string message)
		{
			try
			{
				if (_productRatingRepository.GetProductRating((long)productRating.OrderDetailID, (long)productRating.CustomerID) == null)
				{
					productRating.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
					_productRatingRepository.Add(productRating);
					if (SaveProductRating())
					{
						message = "ProductRating added successfully ...";
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
					message = "ProductRating already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool UpdateProductRating(ref ProductRating productRating, ref string message)
		{
			try
			{
				if (_productRatingRepository.GetProductRating((long)productRating.OrderDetailID, (long)productRating.CustomerID, productRating.ID) == null)
				{
					ProductRating CurrentProductRating = _productRatingRepository.GetById(productRating.ID);

					CurrentProductRating.Rating = productRating.Rating;
					CurrentProductRating.Remarks = productRating.Remarks;
					productRating = null;

					_productRatingRepository.Update(CurrentProductRating);
					if (SaveProductRating())
					{
						productRating = CurrentProductRating;
						message = "Product Rating updated successfully ...";
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
					message = "ProductRating already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool DeleteProductRating(long id, ref string message)
		{
			try
			{
				ProductRating productRating = _productRatingRepository.GetById(id);

				_productRatingRepository.Delete(productRating);

				if (SaveProductRating())
				{
					message = "ProductRating deleted successfully ...";
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

		public bool SaveProductRating()
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

	public interface IProductRatingService
	{
		IEnumerable<ProductRating> GetProductRatings();
		IEnumerable<SP_GetProductRatings_Result> GetProductRatings(long productId, string ImageServer);
		IEnumerable<SP_GetVendorRatings_Result> GetVendorRatings(long vendorId, long pageNumber, string ImageServer);
		ProductRating GetProductRating(long id);
		ProductRating GetProductRating(long orderDetailId, long ProductId);
		ProductRating GetOrderDetailRating(long orderDetailId);
		bool CreateProductRating(ref ProductRating productRating, ref string message);
		bool UpdateProductRating(ref ProductRating productRating, ref string message);
		bool DeleteProductRating(long id, ref string message);
		bool SaveProductRating();
	}
}
