using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
    public class VendorWithdrawalRequestService : IVendorWithdrawalRequestService
    {
        private readonly IVendorWithdrawalrequestRepository _VendorWithdrawalrequestRepository;
        private readonly IUnitOfWork _unitOfWork;

        public VendorWithdrawalRequestService(IVendorWithdrawalrequestRepository VendorWithdrawalrequestRepository, IUnitOfWork unitOfWork)
        {
            this._VendorWithdrawalrequestRepository = VendorWithdrawalrequestRepository;
            this._unitOfWork = unitOfWork;
        }

        public IEnumerable<VendorWithdrawalRequest> GetVendorWithdrawalRequest()
        {
            var pendingRequest = _VendorWithdrawalrequestRepository.GetAll().Where(x => x.Status == "Pending");
            return pendingRequest;
        }
        public IEnumerable<VendorWithdrawalRequest> GetVendorWithdrawalRequestByVendorID(long VendorID)
        {
            var pendingRequest = _VendorWithdrawalrequestRepository.GetAll().Where(x => x.VendorID == VendorID);
            return pendingRequest;
        }
        public IEnumerable<VendorWithdrawalRequest> GetVendorWithdrawalRequestByVendorIDAndDateRange(DateTime? FromDate, DateTime? ToDate, long VendorID)
        {
            var pendingRequest = _VendorWithdrawalrequestRepository.GetAll().Where(c => ( c.VendorID == VendorID) && (FromDate == null || c.CreatedOn >= FromDate) && (ToDate == null || c.CreatedOn <= ToDate));
            return pendingRequest;
        }
        public VendorWithdrawalRequest GetWithdrawalRequest(long id)
        {
            var request = _VendorWithdrawalrequestRepository.GetById(id);
            return request;
        }
        public bool CreateRequest(VendorWithdrawalRequest req, ref string message)
        {
            try
            {
                req.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                _VendorWithdrawalrequestRepository.Add(req);
                if (SaveRequest())
                {
                    message = "Request generated successfully ...";
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
        public bool UpdateRequest(ref VendorWithdrawalRequest request, ref string message, bool updater = true)
        {
            try
            {
                VendorWithdrawalRequest CurrentRequest = new VendorWithdrawalRequest();
                if (updater == true)
                {
                    CurrentRequest = _VendorWithdrawalrequestRepository.GetById(request.ID);

                    CurrentRequest.Status = request.Status;
                    CurrentRequest.Remarks = request.Remarks;

                    request = null;
                    _VendorWithdrawalrequestRepository.Update(CurrentRequest);
                }

                if (SaveRequest())
                {
                    if (updater == true)
                    {
                        request = CurrentRequest;
                    }
                    request = request == null ? CurrentRequest : request;
                    message = "Request updated successfully ...";
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
        public bool SaveRequest()
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
    public interface IVendorWithdrawalRequestService
    {
        bool CreateRequest(VendorWithdrawalRequest req, ref string message);
        bool SaveRequest();
        IEnumerable<VendorWithdrawalRequest> GetVendorWithdrawalRequest();
        VendorWithdrawalRequest GetWithdrawalRequest(long id);
        bool UpdateRequest(ref VendorWithdrawalRequest request, ref string message, bool updater = true);
        IEnumerable<VendorWithdrawalRequest> GetVendorWithdrawalRequestByVendorID(long VendorID);
        IEnumerable<VendorWithdrawalRequest> GetVendorWithdrawalRequestByVendorIDAndDateRange(DateTime? FromDate, DateTime? ToDate, long VendorID);
    }
}
