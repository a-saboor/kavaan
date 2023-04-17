using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
    
    class QuotationService : IQuotationService
    {
        private readonly IQuotationRepository _quotationRepository;
        private readonly IQuotationNoteService _quotationNoteService;
        private readonly IQuotationDetailService _quotationDetailService;
        private readonly IUnitOfWork _unitOfWork;
        public QuotationService(IUnitOfWork unitOfWork, IQuotationRepository quotationRepository, IQuotationDetailService quotationDetailService, IQuotationNoteService quotationNoteService)
        {
            this._unitOfWork = unitOfWork;
            this._quotationRepository = quotationRepository;
            this._quotationDetailService = quotationDetailService;
            this._quotationNoteService = quotationNoteService;
        }
        public IEnumerable<Quotation> GetQuotations()
        {
            var quotation = _quotationRepository.GetAll();
            return quotation;
        }
        public Quotation GetQuotation(long id)
        {
            var quotation = this._quotationRepository.GetById(id);
            return quotation;
        }
        public Quotation GetQuotationByServiceBookingID(long bookingID)
        {
            var quotation = this._quotationRepository.GetAll().Where(x=>x.BookingID==bookingID).FirstOrDefault();
            return quotation;
        }
        public bool CreateQuotation(ref Quotation quotation, ref string message)
        {
            try
            {

                quotation.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                _quotationRepository.Add(quotation);
                if (SaveQuotation())
                {
                    message = "Quotation added successfully...";
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
        public bool UpdateQuotation(ref Quotation quotation, ref string message)
        {
            try
            {
                Quotation currentquotation = _quotationRepository.GetById(quotation.ID);
                _quotationRepository.Update(quotation);
                if (SaveQuotation())
                {
                    quotation = currentquotation;
                    message = "Quotation updated successfully ...";
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
        public bool DeleteQuotation(long id, ref string message, ref bool hasChilds, bool softdelete)
        {

            if (softdelete)
            {
                try
                {
                    //soft delete
                    Quotation quotation = _quotationRepository.GetById(id);
                    //When department delete, its all refrences will be deleted
                    // delete code here.....
                    //*
                    _quotationRepository.Update(quotation);

                    if (SaveQuotation())
                    {
                        message = "Quotation deleted successfully ...";
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
            else
            {

                //hard delete
                try
                {
                    Quotation quotation = _quotationRepository.GetById(id);

                    var quotationDetail = _quotationDetailService.GetQuotationByServiceQuotationID(quotation.ID);
                    var quotationNote = _quotationNoteService.GetQuotationNotesByQuotationID(quotation.ID);

                    foreach(var item in quotationDetail) 
                    {
                        _quotationDetailService.DeleteQuotationDetail(item.ID, ref message, ref hasChilds, softdelete = false);
                    }
                    foreach (var item in quotationNote)
                    {
                        _quotationNoteService.DeleteQuotationNote(item.ID, ref message, ref hasChilds, softdelete = false);
                    }

                    _quotationRepository.Delete(quotation);

                    if (SaveQuotation())
                    {
                        message = "Quotation deleted successfully ...";
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
        }
        public bool SaveQuotation()
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
    public interface IQuotationService
    {
        Quotation GetQuotation(long id);
        IEnumerable<Quotation> GetQuotations();
        Quotation GetQuotationByServiceBookingID(long bookingID);
        bool CreateQuotation(ref Quotation quotation, ref string message);
        bool UpdateQuotation(ref Quotation quotation, ref string message);
        bool DeleteQuotation(long id, ref string message, ref bool hasContractorLink, bool softdelete);
        bool SaveQuotation();
    }
}
