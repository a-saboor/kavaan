using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{
    class EnquiryRepository : RepositoryBase<Enquiry>, IEnquiryRepository
    {
        public EnquiryRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public List<Enquiry> GetFilteredContactEnquiry(DateTime FromDate, DateTime ToDate)
        {
            List<Enquiry> enquiries = new List<Enquiry>();
            enquiries = this.DbContext.Enquiries.Where(i => i.Type == "ContactEnquiry" && i.CreatedOn >= FromDate && i.CreatedOn <= ToDate).ToList();
            return enquiries;
        }
        public List<Enquiry> GetFilteredCustomerEnquiry(DateTime FromDate, DateTime ToDate)
        {
            List<Enquiry> enquiries = new List<Enquiry>();
            enquiries = this.DbContext.Enquiries.Where(i => i.Type == "CustomerEnquiry" && i.CreatedOn >= FromDate && i.CreatedOn <= ToDate).ToList();
            return enquiries;
        }
    }
    public interface IEnquiryRepository : IRepository<Enquiry>
    {
        List<Enquiry> GetFilteredContactEnquiry(DateTime FromDate, DateTime ToDate);
        List<Enquiry> GetFilteredCustomerEnquiry(DateTime FromDate, DateTime ToDate);
    }
}
