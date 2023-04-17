using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Service
{

	public class VendorWalletShareHistoryOrdersService : IVendorWalletShareHistoryOrdersService
	{
		private readonly IVendorWalletShareHistoryOrdersRepository _vendorWalletShareHistoryOrdersRepository;
		private readonly IUnitOfWork _unitOfWork;

		public VendorWalletShareHistoryOrdersService(IVendorWalletShareHistoryOrdersRepository vendorWalletShareHistoryOrdersRepository, IUnitOfWork unitOfWork)
		{
			this._vendorWalletShareHistoryOrdersRepository = vendorWalletShareHistoryOrdersRepository;
			this._unitOfWork = unitOfWork;
		}

		#region IVendorWalletShareHistoryOrdersService Members

		public IEnumerable<VendorWalletShareHistoryOrder> GetVendorWalletShareHistoryOrders()
		{
			var vendorWalletShareHistoryOrders = _vendorWalletShareHistoryOrdersRepository.GetAll();
			return vendorWalletShareHistoryOrders;
		}


		public VendorWalletShareHistoryOrder GetVendorWalletShareHistoryOrders(long id)
		{
			var vendorWalletShareHistoryOrders = _vendorWalletShareHistoryOrdersRepository.GetById(id);
			return vendorWalletShareHistoryOrders;
		}

		public bool CreateVendorWalletShareHistoryOrder(VendorWalletShareHistoryOrder vendorWalletShareHistoryOrders, ref string message)
		{
			try
			{
				
				_vendorWalletShareHistoryOrdersRepository.Add(vendorWalletShareHistoryOrders);
				if (SaveVendorWalletShareHistoryOrders())
				{
					message = "VendorcWallet Share History Order added successfully ...";
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

		public bool UpdateVendorWalletShareHistoryOrder(ref VendorWalletShareHistoryOrder vendorWalletShareHistoryOrders, ref string message)
		{
			try
			{

				_vendorWalletShareHistoryOrdersRepository.Update(vendorWalletShareHistoryOrders);
				if (SaveVendorWalletShareHistoryOrders())
				{
					message = "VendorcWallet Share History Order updated successfully ...";
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

		//public bool DeleteVendorWalletShareHistoryOrders(long id, ref string message)
		//{
		//	try
		//	{
		//		VendorWalletShareHistoryOrder vendorWalletShareHistoryOrders = _vendorWalletShareHistoryOrdersRepository.GetById(id);

		//		_vendorWalletShareHistoryOrdersRepository.Delete(vendorWalletShareHistoryOrders);
		//		if (SaveVendorWalletShareHistoryOrders())
		//		{
		//			message = "VendorcWallet Share History Order deleted successfully ...";
		//			return true;
		//		}
		//		else
		//		{
		//			message = "Oops! Something went wrong. Please try later.";
		//			return false;
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		message = "Oops! Something went wrong. Please try later.";
		//		return false;
		//	}
		//}

		public bool SaveVendorWalletShareHistoryOrders()
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

	public interface IVendorWalletShareHistoryOrdersService
	{
		IEnumerable<VendorWalletShareHistoryOrder> GetVendorWalletShareHistoryOrders();
		VendorWalletShareHistoryOrder GetVendorWalletShareHistoryOrders(long id);
		bool CreateVendorWalletShareHistoryOrder(VendorWalletShareHistoryOrder vendorWalletShareHistoryOrders, ref string message);
		bool UpdateVendorWalletShareHistoryOrder(ref VendorWalletShareHistoryOrder vendorWalletShareHistoryOrders, ref string message);
		//bool DeleteVendorWalletShareHistoryOrders(long id, ref string message);
		bool SaveVendorWalletShareHistoryOrders();
	}
}
