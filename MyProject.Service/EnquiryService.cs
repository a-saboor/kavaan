using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
    class EnquiryService : IEnquiryService
    {
        private readonly IEnquiryRepository _enquiryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EnquiryService(IUnitOfWork unitOfWork, IEnquiryRepository enquiryRepository)
        {

            this._unitOfWork = unitOfWork;
            this._enquiryRepository = enquiryRepository;
        }
        #region Customer Enquiry
        public IEnumerable<Enquiry> GetCustomerEnquiry(DateTime FromDate, DateTime ToDate)
        {
            var amenity = this._enquiryRepository.GetFilteredCustomerEnquiry(FromDate, ToDate);
            return amenity;
        }

        public bool CreateCustomerEnquiry(ref Enquiry enquiry, ref string message)
        {
            try
            {
                enquiry.Type = "CustomerEnquiry";
                //enquiry.TypeAr = "استفسار العملاء";
                enquiry.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                //enquiry.Status = "";
                enquiry.EnquiryStatus = "Pending";
                //enquiry.Reply = "";

                this._enquiryRepository.Add(enquiry);

                if (SaveEnquiry())
                {
                    message = "Registration completed successfully ...";
                    return true;
                }
                else
                {
                    message = "Oops! Something went wrong. Please try later...";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }
        #endregion

        #region Contact Enquiry

        public IEnumerable<Enquiry> GetContactEnquiry(DateTime FromDate , DateTime ToDate)
        {
            var amenity = this._enquiryRepository.GetFilteredContactEnquiry(FromDate, ToDate);
            return amenity;
        }

   

        public Enquiry GetEnquiryById(long id)
        {
            Enquiry enquiry = this._enquiryRepository.GetById(id);
            return enquiry;
        }

        public bool CreateContactEnquiry(ref Enquiry enquiry, ref string message)
        {
            try
            {
                enquiry.Type = "ContactEnquiry";
                //enquiry.TypeAr = "استفسار الاتصال";
                enquiry.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                //enquiry.Status = true;
                enquiry.EnquiryStatus = "Pending";
                //enquiry.Reply = "";

                this._enquiryRepository.Add(enquiry);

                if (SaveEnquiry())
                {
                    message = "Enquiry submitted successfully ...";
                    return true;
                }
                else
                {
                    message = "Oops! Something went wrong. Please try later...";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }

        #endregion


        public bool SaveEnquiry()
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
    public interface IEnquiryService
    {
        IEnumerable<Enquiry> GetCustomerEnquiry(DateTime FromDate, DateTime ToDate);
        bool CreateCustomerEnquiry(ref Enquiry enquiry, ref string message);

        IEnumerable<Enquiry> GetContactEnquiry(DateTime FromDate, DateTime ToDate);
        bool CreateContactEnquiry(ref Enquiry enquiry, ref string message);
       
        bool SaveEnquiry();

        Enquiry GetEnquiryById(long id);

    }
}
