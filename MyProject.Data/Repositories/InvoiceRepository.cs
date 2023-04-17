using MyProject.Data.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{
    class InvoiceRepository : RepositoryBase<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(IDbFactory dbFactory)
            : base(dbFactory) { }
        public IEnumerable<Invoice> GetAllByBooking(long recordID)
        {
            var invoices = this.DbContext.Invoices.Where(c => c.RecordID == recordID).ToList();
            return invoices;
        }
        public Invoice GetByBooking(long recordID)
        {
            try
            {
                var invoice = this.DbContext.Invoices.Where(i => i.RecordID == recordID && i.IsDeleted == false).FirstOrDefault();
                return invoice;
            }
            catch (System.Exception)
            {
                return null;
            }
        }
        public Invoice GetByInvoiceNo(string invoiceNo)
        {
            try
            {
                //var invoice = this.DbContext.Invoices.Where(i => i.InvoiceNo == invoiceNo && i.Status == "Unpaid" && i.IsDeleted == false).OrderByDescending(i => i.RecordID).FirstOrDefault();
                var invoice = this.DbContext.Invoices.Where(i => i.InvoiceNo == invoiceNo && i.Status == "Sent" && i.IsDeleted == false).OrderByDescending(i => i.RecordID).FirstOrDefault();
                return invoice;
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

    }
    public interface IInvoiceRepository : IRepository<Invoice>
    {
        IEnumerable<Invoice> GetAllByBooking(long bookingId);
        Invoice GetByBooking(long bookingId);
        Invoice GetByInvoiceNo(string invoiceNo);
    }
}
