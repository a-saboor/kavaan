using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;

namespace MyProject.Service
{
	public class VendorWalletShareHistoryService : IVendorWalletShareHistoryService
	{
		private readonly IVendorWalletShareHistoryRepository _vendorwalletsharehistoryRepository;
		private readonly IUnitOfWork _unitOfWork;

		public VendorWalletShareHistoryService(IVendorWalletShareHistoryRepository vendorwalletsharehistoryRepository, IUnitOfWork unitOfWork)
		{
			this._vendorwalletsharehistoryRepository = vendorwalletsharehistoryRepository;
			this._unitOfWork = unitOfWork;
		}

		#region IVendorWalletShareHistoryService Members

		public IEnumerable<VendorWalletShareHistory> GetVendorWalletShareHistories()
		{
			var vendorwalletsharehistories = _vendorwalletsharehistoryRepository.GetAll();
			return vendorwalletsharehistories;
		}
		public VendorWalletShareHistory GetVendorWalletShareHistory(long id)
		{
			var vendorwalletsharehistory = _vendorwalletsharehistoryRepository.GetById(id);
			return vendorwalletsharehistory;
		}
		public IEnumerable<VendorWalletShareHistory> GetVendorWalletShareHistorybyVendorID(long id)
		{
			DateTime ToDate = Helpers.TimeZone.GetLocalDateTime();

			DateTime FromDate = ToDate.AddDays(-(int)ToDate.DayOfWeek - 6);
			var vendorwalletsharehistory = _vendorwalletsharehistoryRepository.GetFilteredWalletHistory(FromDate, ToDate, id);
			return vendorwalletsharehistory;
		}
		public IEnumerable<VendorWalletShareHistory> GetVendorWalletShareHistorybyVendorIDDateWise(long id, DateTime fromDate, DateTime ToDate)
		{
			var vendorwalletsharehistory = _vendorwalletsharehistoryRepository.GetFilteredWalletHistory(fromDate, ToDate, null);
			return vendorwalletsharehistory;
		}
		public List<VendorWalletShareHistory> GetHistoryDateWise(DateTime? FromDate, DateTime? ToDate, long VendorID)
		{
			var data = _vendorwalletsharehistoryRepository.GetFilteredWalletHistory(FromDate, ToDate, VendorID);
			return data;
		}

		public bool CreateVendorWalletShareHistory(ref VendorWalletShareHistory vendorwalletsharehistory, ref string message)
		{
			try
			{
				vendorwalletsharehistory.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
				_vendorwalletsharehistoryRepository.Add(vendorwalletsharehistory);
				if (SaveVendorWalletShareHistory())
				{
					message = "VendorWalletShareHistory added successfully ...";
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

		public bool DeleteVendorWalletShareHistory(long id, ref string message)
		{
			try
			{
				VendorWalletShareHistory vendorwalletsharehistory = _vendorwalletsharehistoryRepository.GetById(id);

				_vendorwalletsharehistoryRepository.Delete(vendorwalletsharehistory);
				if (SaveVendorWalletShareHistory())
				{
					message = "VendorWalletShareHistory deleted successfully ...";
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

		public bool SaveVendorWalletShareHistory()
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

	public interface IVendorWalletShareHistoryService
	{
		List<VendorWalletShareHistory> GetHistoryDateWise(DateTime? FromDate, DateTime? ToDate, long VendorID);

		IEnumerable<VendorWalletShareHistory> GetVendorWalletShareHistorybyVendorID(long id);
		IEnumerable<VendorWalletShareHistory> GetVendorWalletShareHistorybyVendorIDDateWise(long id, DateTime fromDate, DateTime ToDate);
		IEnumerable<VendorWalletShareHistory> GetVendorWalletShareHistories();
		VendorWalletShareHistory GetVendorWalletShareHistory(long id);
		bool CreateVendorWalletShareHistory(ref VendorWalletShareHistory vendorwalletsharehistory, ref string message);
		bool DeleteVendorWalletShareHistory(long id, ref string message);
		bool SaveVendorWalletShareHistory();
	}
}
