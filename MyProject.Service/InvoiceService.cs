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
    class InvoiceService : IInvoiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInvoiceRepository _invoiceRepository;
        public InvoiceService(IUnitOfWork _unitOfWork, IInvoiceRepository _invoiceRepository)
        {
            this._unitOfWork = _unitOfWork;
            this._invoiceRepository = _invoiceRepository;
        }

        #region IInvoiceService Members

        public IEnumerable<Invoice> GetInvoices()
        {
            var invoices = _invoiceRepository.GetAll();
            return invoices;
        }
        public IEnumerable<Invoice> GetInvoices(long bookingId)
        {
            var invoices = _invoiceRepository.GetAllByBooking(bookingId);
            return invoices;
        }
        public Invoice GetInvoice(long id)
        {
            var invoice = _invoiceRepository.GetById(id);
            return invoice;
        }

        public Invoice GetInvoiceByInvoiceNo(string invoiceNo)
        {
            var invoice = _invoiceRepository.GetByInvoiceNo(invoiceNo);
            return invoice;
        }
        public Invoice GetInvoiceByBooking(long bookingId)
        {
            var invoice = _invoiceRepository.GetByBooking(bookingId);
            return invoice;
        }
        public bool CreateInvoice(Invoice invoice, ref string message)
        {
            try
            {
                invoice.IsDeleted = false;
                invoice.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                _invoiceRepository.Add(invoice);

                if (SaveInvoice())
                {
                    message = "Invoice added successfully ...";
                    return true;
                }
                else
                {
                    message = "Oops! Something went wrong. Please try later...";
                    return false;
                }
            }
            catch (Exception)
            {
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
            
        }

        public bool UpdateInvoice(ref Invoice invoice, ref string message)
        {
            try
            {
                _invoiceRepository.Update(invoice);

                if (SaveInvoice())
                {
                    message = "Invoice updated successfully ...";
                    return true;
                }
                else
                {
                    message = "Oops! Something went wrong. Please try later...";
                    return false;
                }
            }
            catch (Exception)
            {
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }

        public bool SaveInvoice()
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
    public interface IInvoiceService
    {
        IEnumerable<Invoice> GetInvoices();
		Invoice GetInvoice(long id);
		Invoice GetInvoiceByInvoiceNo(string invoiceNo);
        IEnumerable<Invoice> GetInvoices(long bookingId);
        Invoice GetInvoiceByBooking(long bookingId);
        bool CreateInvoice(Invoice invoice, ref string message);
		bool UpdateInvoice(ref Invoice invoice, ref string message);
        bool SaveInvoice();
    }
}
