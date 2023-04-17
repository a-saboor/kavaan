using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{

    class QuotationNoteService : IQuotationNoteService
    {
        private readonly IQuotationNoteRepository _quotationNoteRepository;
        private readonly IUnitOfWork _unitOfWork;
        public QuotationNoteService(IUnitOfWork unitOfWork, IQuotationNoteRepository quotationNoteRepository)
        {
            this._unitOfWork = unitOfWork;
            this._quotationNoteRepository = quotationNoteRepository;
        }
        public IEnumerable<QuotationNote> GetQuotationNotes()
        {
            var quotationNote = _quotationNoteRepository.GetAll();
            return quotationNote;
        }
        public IEnumerable<QuotationNote> GetQuotationNotesByQuotationID(long id)
        {
            var quotationNote = _quotationNoteRepository.GetAll().Where(x=>x.QuotationID==id);
            return quotationNote;
        }
        public QuotationNote GetQuotationNote(long id)
        {
            var quotationNote = this._quotationNoteRepository.GetById(id);
            return quotationNote;
        }
        public bool CreateQuotationNote(QuotationNote quotationNote, ref string message)
        {
            try
            {

                quotationNote.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                _quotationNoteRepository.Add(quotationNote);
                if (SaveQuotationNote())
                {
                    message = "Quotation Note added successfully...";
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
        public bool UpdateQuotationNote(ref QuotationNote quotationNote, ref string message)
        {
            try
            {
                QuotationNote currentquotationnote = _quotationNoteRepository.GetById(quotationNote.ID);
                _quotationNoteRepository.Update(quotationNote);
                if (SaveQuotationNote())
                {
                    quotationNote = currentquotationnote;
                    message = "Quotation Note updated successfully ...";
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
        public bool DeleteQuotationNote(long id, ref string message, ref bool hasChilds, bool softdelete)
        {

            if (softdelete)
            {
                try
                {
                    //soft delete
                    QuotationNote quotationNote = _quotationNoteRepository.GetById(id);
                    //When department delete, its all refrences will be deleted
                    // delete code here.....
                    //*
                    _quotationNoteRepository.Update(quotationNote);

                    if (SaveQuotationNote())
                    {
                        message = "Quotation Note deleted successfully ...";
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
                    QuotationNote quotationNote = _quotationNoteRepository.GetById(id);

                    _quotationNoteRepository.Delete(quotationNote);

                    if (SaveQuotationNote())
                    {
                        message = "Quotation Note deleted successfully ...";
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
        public bool SaveQuotationNote()
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
    public interface IQuotationNoteService
    {
        QuotationNote GetQuotationNote(long id);
        IEnumerable<QuotationNote> GetQuotationNotesByQuotationID(long id);
        IEnumerable<QuotationNote> GetQuotationNotes();
        bool CreateQuotationNote(QuotationNote quotation, ref string message);
        bool UpdateQuotationNote(ref QuotationNote quotation, ref string message);
        bool DeleteQuotationNote(long id, ref string message, ref bool hasContractorLink, bool softdelete);
        bool SaveQuotationNote();
    }
}
