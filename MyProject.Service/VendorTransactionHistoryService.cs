using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;


namespace MyProject.Service
{
    class VendorTransactionHistoryService : IVendorTransactionHistoryService
    {
        private readonly IVendorTransactionHistoryRepository _vendorTransactionHistoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public VendorTransactionHistoryService(IVendorTransactionHistoryRepository vendorTransactionHistoryRepository, IUnitOfWork unitOfWork)
        {
            this._vendorTransactionHistoryRepository = vendorTransactionHistoryRepository;
            this._unitOfWork = unitOfWork;
        }

        public IEnumerable<VendorTransactionHistory> GetVendorTransactionHistories()
        {
            var histories = _vendorTransactionHistoryRepository.GetAll();
            return histories;
        }

        public IEnumerable<VendorTransactionHistory> GetVendorTransactionHistoriesByVendor(long vendorId)
        {
            var histories = _vendorTransactionHistoryRepository.GetAll().Where(mod => mod.VendorID == vendorId);
            return histories;
        }

        public VendorTransactionHistory GetVendorTransactionHistory(long id)
        {
            var history = _vendorTransactionHistoryRepository.GetById(id);
            return history;
        }

        public bool CreateVendorTransactionHistory(ref VendorTransactionHistory vendorTransactionHistory, ref string message)
        {
            try
            {
                vendorTransactionHistory.Date = Helpers.TimeZone.GetLocalDateTime();
                _vendorTransactionHistoryRepository.Add(vendorTransactionHistory);
                if (SaveVendorTransactionHistory())
                {
                    message = "Vendor transaction history maintained...";
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

        public bool UpdateVendorTransactionHistory(ref VendorTransactionHistory vendorTransactionHistory, ref string message)
        {
            try
            {

                VendorTransactionHistory CurrentHistory = _vendorTransactionHistoryRepository.GetById(vendorTransactionHistory.ID);

                CurrentHistory.VendorID = vendorTransactionHistory.VendorID;
                CurrentHistory.VendorPackageID = vendorTransactionHistory.VendorPackageID;
                CurrentHistory.Price = vendorTransactionHistory.Price;
                CurrentHistory.Adjustment = vendorTransactionHistory.Adjustment;
                CurrentHistory.Date = vendorTransactionHistory.Date;


                _vendorTransactionHistoryRepository.Update(CurrentHistory);
                if (SaveVendorTransactionHistory())
                {
                    vendorTransactionHistory = CurrentHistory;
                    message = "History updated successfully ...";
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
        public bool SaveVendorTransactionHistory()
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
    }
    public interface IVendorTransactionHistoryService
    {
        IEnumerable<VendorTransactionHistory> GetVendorTransactionHistories();
        VendorTransactionHistory GetVendorTransactionHistory(long id);
        IEnumerable<VendorTransactionHistory> GetVendorTransactionHistoriesByVendor(long vendorId);
        bool CreateVendorTransactionHistory(ref VendorTransactionHistory vendorTransactionHistory, ref string message);
        bool UpdateVendorTransactionHistory(ref VendorTransactionHistory vendorTransactionHistory, ref string message);
        bool SaveVendorTransactionHistory();
    }
}
