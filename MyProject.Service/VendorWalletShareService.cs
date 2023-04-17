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
	public class VendorWalletShareService : IVendorWalletShareService
	{
		private readonly IVendorWalletShareRepository _vendorwalletshareRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IVendorRepository _vendorRepository;

		public VendorWalletShareService(IVendorWalletShareRepository vendorwalletshareRepository, IUnitOfWork unitOfWork)
		{
			this._vendorwalletshareRepository = vendorwalletshareRepository;
			this._unitOfWork = unitOfWork;
		}

		#region IVendorWalletShareService Members

		public IEnumerable<VendorWalletShare> GetVendorWalletShares()
		{
			var vendorwalletshares = _vendorwalletshareRepository.GetAll();
			return vendorwalletshares;
		}

		public VendorWalletShare GetVendorWalletShare(long id)
		{
			var vendorwalletshare = _vendorwalletshareRepository.GetById(id);
			return vendorwalletshare;
		}

		public VendorWalletShare GetWalletShareByVendor(long vendorId)
		{
			var vendorwalletShare = _vendorwalletshareRepository.GetByVendor(vendorId);
			return vendorwalletShare;
		}

		public bool CreateVendorWalletShare(VendorWalletShare vendorwalletshare, ref string message)
		{
			try
			{
				vendorwalletshare.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
				_vendorwalletshareRepository.Add(vendorwalletshare);
				if (SaveVendorWalletShare())
				{
					message = "VendorWalletShare added successfully ...";
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

		public bool UpdateVendorWalletShare(ref VendorWalletShare vendorwalletshare, ref string message)
		{
			try
			{

				VendorWalletShare CurrentVendorWalletShare = _vendorwalletshareRepository.GetById(vendorwalletshare.ID);

				CurrentVendorWalletShare.PendingAmount = vendorwalletshare.PendingAmount;
				CurrentVendorWalletShare.TotalEarning = vendorwalletshare.TotalEarning;
				CurrentVendorWalletShare.TransferedAmount = vendorwalletshare.TransferedAmount;
				vendorwalletshare = null;

				_vendorwalletshareRepository.Update(CurrentVendorWalletShare);
				if (SaveVendorWalletShare())
				{
					vendorwalletshare = CurrentVendorWalletShare;
					message = "VendorWalletShare updated successfully ...";
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

		public bool DeleteVendorWalletShare(long id, ref string message)
		{
			try
			{
				VendorWalletShare vendorwalletshare = _vendorwalletshareRepository.GetById(id);

				_vendorwalletshareRepository.Delete(vendorwalletshare);
				if (SaveVendorWalletShare())
				{
					message = "VendorWalletShare deleted successfully ...";
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

		public bool SaveVendorWalletShare()
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

		public VendorWalletShare GetDetails(long id)
		{
			var details = _vendorwalletshareRepository.GetByVendor(id);

			return details;
		}

		public bool UpdateVendorEarning(long orderID)
		{
			try
			{
				DateTime createdOn = Helpers.TimeZone.GetLocalDateTime();
				_vendorwalletshareRepository.UpdateVendorEarning(orderID, createdOn);
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}
		#endregion
	}

	public interface IVendorWalletShareService
	{

		VendorWalletShare GetDetails(long id);
		IEnumerable<VendorWalletShare> GetVendorWalletShares();
		VendorWalletShare GetVendorWalletShare(long id);
		bool CreateVendorWalletShare(VendorWalletShare vendorwalletshare, ref string message);
		bool UpdateVendorWalletShare(ref VendorWalletShare vendorwalletshare, ref string message);
		bool DeleteVendorWalletShare(long id, ref string message);
		bool SaveVendorWalletShare();
		VendorWalletShare GetWalletShareByVendor(long vendorId);
		bool UpdateVendorEarning(long orderID);
	}
}
