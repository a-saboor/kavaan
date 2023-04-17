using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;


namespace MyProject.Service
{
	public class ProductReturnService : IProductReturnService
	{
		private readonly IProductReturnRepository _productReturnRepository;
		private readonly IUnitOfWork _unitOfWork;

		public ProductReturnService(IProductReturnRepository productReturnRepository, IUnitOfWork unitOfWork)
		{
			this._productReturnRepository = productReturnRepository;
			this._unitOfWork = unitOfWork;
		}

		#region IProductReturnService Members

		public IEnumerable<ProductReturn> GetProductReturns()
		{
			var productReturns = _productReturnRepository.GetAll();
			return productReturns;
		}
		public List<ProductReturn> GetProductReturnsDateWise(DateTime FromDate, DateTime ToDate)
		{
			var productReturns = _productReturnRepository.GetFilteredProductReturnOrders(FromDate, ToDate);
			if (productReturns.Count != 0)
				productReturns = productReturns.OrderByDescending(x => x.CreatedOn).ToList();
			return productReturns;
		}

		public ProductReturn GetProductReturn(long id)
		{
			var productReturn = _productReturnRepository.GetById(id);
			return productReturn;
		}

		public ProductReturn GetProductReturn(long orderDetailId, long ProductId)
		{
			var productReturn = _productReturnRepository.GetProductReturn(orderDetailId, ProductId);
			return productReturn;
		}

		public ProductReturn GetOrderDetailReturn(long orderDetailId)
		{
			var productReturn = _productReturnRepository.GetOrderDetailReturn(orderDetailId);
			return productReturn;
		}

		public bool CreateProductReturn(ref ProductReturn productReturn, ref string message)
		{
			try
			{
				if (_productReturnRepository.GetProductReturn((long)productReturn.OrderDetailID, (long)productReturn.CustomerID) == null)
				{
					productReturn.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
					_productReturnRepository.Add(productReturn);
					if (SaveProductReturn())
					{
						message = "Product return added successfully ...";
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
					message = "Product return already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool UpdateProductReturn(ref ProductReturn productReturn, ref string message)
		{
			try
			{
				if (_productReturnRepository.GetProductReturn((long)productReturn.OrderDetailID, (long)productReturn.CustomerID, productReturn.ID) == null)
				{
					ProductReturn CurrentProductReturn = _productReturnRepository.GetById(productReturn.ID);

					CurrentProductReturn.ReturnMethod = productReturn.ReturnMethod;
					CurrentProductReturn.Reason = productReturn.Reason;
					CurrentProductReturn.Status = productReturn.Status;
					productReturn = null;

					_productReturnRepository.Update(CurrentProductReturn);
					if (SaveProductReturn())
					{
						productReturn = CurrentProductReturn;
						message = "Product return updated successfully ...";
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
					message = "Product return already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool DeleteProductReturn(long id, ref string message)
		{
			try
			{
				ProductReturn productReturn = _productReturnRepository.GetById(id);

				_productReturnRepository.Delete(productReturn);

				if (SaveProductReturn())
				{
					message = "Product return deleted successfully ...";
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

		public bool SaveProductReturn()
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

		//public IEnumerable<SP_GetCustomerProductReturns_Result> GetCustomerReturns(long customerId, string status, int pageNumber = 1, int sortBy = 1, string lang = "en")
		//{
		//	var orders = _productReturnRepository.GetCustomerReturns(customerId, status, pageNumber, sortBy, lang);
		//	return orders;
		//}

		#endregion
	}

	public interface IProductReturnService
	{
		IEnumerable<ProductReturn> GetProductReturns();
		ProductReturn GetProductReturn(long id);
		ProductReturn GetProductReturn(long orderDetailId, long ProductId);
		ProductReturn GetOrderDetailReturn(long orderDetailId);
		bool CreateProductReturn(ref ProductReturn productReturn, ref string message);
		bool UpdateProductReturn(ref ProductReturn productReturn, ref string message);
		bool DeleteProductReturn(long id, ref string message);
		bool SaveProductReturn();
		//IEnumerable<SP_GetCustomerProductReturns_Result> GetCustomerReturns(long customerId, string status, int pageNumber = 1, int sortBy = 1, string lang = "en");
		List<ProductReturn> GetProductReturnsDateWise(DateTime FromDate, DateTime ToDate);
	}
}
