using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{

    class QuotationDetailService : IQuotationDetailService
    {
        private readonly IQuotationDetailsRepository _quotationDetailRepository;
        private readonly IUnitOfWork _unitOfWork;
        public QuotationDetailService(IUnitOfWork unitOfWork, IQuotationDetailsRepository quotationDetailRepository)
        {
            this._unitOfWork = unitOfWork;
            this._quotationDetailRepository = quotationDetailRepository;
        }
        public IEnumerable<QuotationDetail> GetQuotationDetails()
        {
            var quotationDetail = _quotationDetailRepository.GetAll();
            return quotationDetail;
        }
        public QuotationDetail GetQuotationDetail(long id)
        {
            var quotationDetail = this._quotationDetailRepository.GetById(id);
            return quotationDetail;
        }
        public IEnumerable<QuotationDetail> GetQuotationByServiceQuotationID(long quotationId)
        {
            var quotation = this._quotationDetailRepository.GetAll().Where(x => x.QuotationID == quotationId);
            return quotation;
        }
        public bool CreateQuotationDetail(QuotationDetail quotationDetail, ref string message)
        {
            try
            {

                _quotationDetailRepository.Add(quotationDetail);
                if (SaveQuotationDetail())
                {
                    message = "Quotation Detail added successfully...";
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
        public bool UpdateQuotationDetail(ref QuotationDetail quotationDetail, ref string message)
        {
            try
            {
                QuotationDetail currentquotationDetail = _quotationDetailRepository.GetById(quotationDetail.ID);
                _quotationDetailRepository.Update(quotationDetail);
                if (SaveQuotationDetail())
                {
                    quotationDetail = currentquotationDetail;
                    message = "Quotation Detail updated successfully ...";
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
        public bool DeleteQuotationDetail(long id, ref string message, ref bool hasChilds, bool softdelete)
        {

            if (softdelete)
            {
                try
                {
                    //soft delete
                    QuotationDetail quotationDetail = _quotationDetailRepository.GetById(id);
                    //When department delete, its all refrences will be deleted
                    // delete code here.....
                    //*
                    _quotationDetailRepository.Update(quotationDetail);

                    if (SaveQuotationDetail())
                    {
                        message = "Quotation Detail deleted successfully ...";
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
                    QuotationDetail quotationDetail = _quotationDetailRepository.GetById(id);

                    _quotationDetailRepository.Delete(quotationDetail);

                    if (SaveQuotationDetail())
                    {
                        message = "Quotation Detail deleted successfully ...";
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
        public bool SaveQuotationDetail()
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
    public interface IQuotationDetailService
    {
        QuotationDetail GetQuotationDetail(long id);
        IEnumerable<QuotationDetail> GetQuotationDetails();
        IEnumerable<QuotationDetail> GetQuotationByServiceQuotationID(long quotationId);
        bool CreateQuotationDetail(QuotationDetail quotation, ref string message);
        bool UpdateQuotationDetail(ref QuotationDetail quotation, ref string message);
        bool DeleteQuotationDetail(long id, ref string message, ref bool hasContractorLink, bool softdelete);
        bool SaveQuotationDetail();
    }
}
